output "public_address" {
  value = "http://${aws_elb.elb.dns_name}"
}

output "ec2_public_ips" {
  value = aws_instance.ec2.*.public_ip
}

output "rds_address" {
  value = "mysql://${aws_db_instance.rds.endpoint}/comments?user=root&password=${random_string.rds_password.result}"
}
