variable "aws_region" {
  default = "ap-southeast-1"
}

variable "aws_amis" {
  default = {
    "ap-southeast-1" = "ami-0b44582c8c5b24a49"
  }
}

variable "aws_instances" {
  default = 2
}

variable "app_environment" {
  default = "Development"
}
