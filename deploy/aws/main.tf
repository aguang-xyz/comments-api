terraform {
  required_providers {
    null = {
      source  = "hashicorp/null"
      version = "~> 2.1.2"
    }

    random = {
      source = "hashicorp/random"
      version = "~> 2.3.0"
    }

    local = {
      source = "hashicorp/local"
      version = "~> 1.4.0"
    }

    aws = {
      source  = "hashicorp/aws"
      version = "~> 2.70"
    }
  }
}

provider "aws" {
  region  = var.aws_region
}

# Randomly generate a string as the RDS password.
resource "random_string" "rds_password" {
  length = 16
}

# Create a security group for the RDS instance.
resource "aws_security_group" "rds" {
  description = "Allow MySQL traffic"

  ingress {
    description = "MySQL"
    from_port   = 3306
    to_port     = 3306
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Create the RDS instance.
resource "aws_db_instance" "rds" {
  allocated_storage    = 20
  storage_type         = "gp2"
  engine               = "mysql"
  engine_version       = "5.7"
  instance_class       = "db.t2.micro"
  name                 = "comments"
  username             = "root"
  password             = random_string.rds_password.result
  parameter_group_name = "default.mysql5.7"

  skip_final_snapshot  = true
  publicly_accessible  = true

	vpc_security_group_ids = [
		aws_security_group.rds.id
	]
}

# Generate appsettings.Production.json and compile the source project.
resource "null_resource" "build_project" {
  provisioner "local-exec" {
    command = "./scripts/build-project"

    environment = {
      APP_ENVIRONMENT = var.app_environment
      RDS_HOST        = aws_db_instance.rds.address
      RDS_PORT        = aws_db_instance.rds.port
      RDS_DATABASE    = aws_db_instance.rds.name
      RDS_USERNAME    = aws_db_instance.rds.username
      RDS_PASSWORD    = aws_db_instance.rds.password
    }
  }
}

# Execute database migrations.
resource "null_resource" "execute_migrations" {
  provisioner "local-exec" {
    command = "./scripts/execute-migrations"

    environment = {
      APP_ENVIRONMENT = var.app_environment
    }
  }

  depends_on = [
    null_resource.build_project
  ]
}

# Generate SSH keys locally.
resource "null_resource" "generate_ssh_keys" {
  provisioner "local-exec" {
    command = "./scripts/generate-ssh-keys"
  }
}

# The local file "keys/ec2".
data "local_file" "ec2_private_key" {
  filename   = "./keys/ec2"
  depends_on = [null_resource.generate_ssh_keys]
}

# The local file "keys/ec2.pub".
data "local_file" "ec2_public_key" {
  filename   = "./keys/ec2.pub"
  depends_on = [null_resource.generate_ssh_keys]
}

# Create the AWS SSH key pair for EC2 instances.
resource "aws_key_pair" "ec2" {
  key_name   = "ec2"
  public_key = data.local_file.ec2_public_key.content
}

# Create a security group for EC2 instances.
resource "aws_security_group" "ec2" {
  description = "Allow HTTP, HTTPS and SSH traffic"

  ingress {
    description = "SSH"
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    description = "HTTP"
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Create EC2 instances (with nginx installed).
resource "aws_instance" "ec2" {
  count         = var.aws_instances
  key_name      = aws_key_pair.ec2.key_name
  ami           = var.aws_amis[var.aws_region]
  instance_type = "t2.micro"

	vpc_security_group_ids = [
		aws_security_group.ec2.id
	]

	connection {
    type        = "ssh"
    user        = "ubuntu"
    private_key = data.local_file.ec2_private_key.content
    host        = self.public_ip
	}

  # Upload the executable binary.
  provisioner "file" {
    source      = "../../src/bin/${var.app_environment}/netcoreapp3.1"
    destination = "~/comments-api"
  }

  provisioner "remote-exec" {
    inline = [
      "sudo apt update",
      "sudo apt install -y wget apt-transport-https",
      "wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb",
      "sudo dpkg -i packages-microsoft-prod.deb",
      "rm packages-microsoft-prod.deb",
      "sudo apt update",
      "sudo apt install -y dotnet-sdk-3.1 aspnetcore-runtime-3.1",
      "sudo chmod +x ~/comments-api/CommentsApi",
      "cd ~/comments-api && (ASPNETCORE_ENVIRONMENT=${var.app_environment} nohup ~/comments-api/CommentsApi > ~/comments-api.log &)",
      "sleep 5"
    ]
  }

  # Deploy instances after the app settings and binaries have been built.
  depends_on = [
    null_resource.build_project
  ]
}

# Create a ELB instance.
resource "aws_elb" "elb" {
  availability_zones = aws_instance.ec2.*.availability_zone

  listener {
    instance_port     = 8080
    instance_protocol = "http"
    lb_port           = 80
    lb_protocol       = "http"
  }

  instances = aws_instance.ec2.*.id
}
