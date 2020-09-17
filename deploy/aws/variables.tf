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

variable "app_domain" {
  default = "aguang.xyz"
}

variable "api_domain" {
  default = "comments-api.aguang.xyz"
}

variable "route53_zone_id" {
  default = "Z00344641F4M8H77T0IIU"
}

variable "aws_ssl_cert_id" {
  default = "arn:aws:acm:ap-southeast-1:421292287272:certificate/78995108-020e-4865-8a97-c4c3e9fff100"
}
