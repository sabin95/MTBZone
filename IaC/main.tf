terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "3.74.0"
    }
  }
}

locals{
  CatalogAPI_zipName = "CatalogAPI.zip"
}

provider "aws" {
  region = "eu-central-1"
}

variable "db_username" {
  
}

variable "db_password" {
  
}

data "aws_availability_zones" "available" {
  state = "available"
}

resource "aws_vpc" "MTBZoneVPC" {
  cidr_block = "10.0.0.0/16"
  tags = {
    Name = "MTBZoneVPC"
  }
  enable_dns_hostnames = true
  enable_dns_support = true
}

resource "aws_internet_gateway" "MTBZoneiGW" {
  vpc_id = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneiGW"
  }
}

resource "aws_subnet" "MTBZoneDBSubnet" {
  count = 3
  vpc_id     = aws_vpc.MTBZoneVPC.id
  cidr_block = "10.0.${count.index}.0/24"
  availability_zone = data.aws_availability_zones.available.names[count.index]
  tags = {
    Name = "MTBZoneDBSubnet"
  }
}

resource "aws_subnet" "MTBZoneLambdaSubnet" {
  vpc_id     = aws_vpc.MTBZoneVPC.id
  cidr_block = "10.0.4.0/24"

  tags = {
    Name = "MTBZoneLambdaSubnet"
  }
}

resource "aws_security_group" "MTBZoneDBSecurityGroup" {
  name        = "MTBZoneDBSecurityGroup"
  vpc_id      = aws_vpc.MTBZoneVPC.id

  ingress {
    from_port        = 1433
    to_port          = 1433
    protocol         = "tcp"
    cidr_blocks      = [aws_subnet.MTBZoneLambdaSubnet.cidr_block]
  }

  tags = {
    Name = "MTBZoneDBSecurityGroup"
  }
}

resource "aws_security_group" "MTBZoneLambdaSecurityGroup" {
  name        = "MTBZoneLambdaSecurityGroup"
  vpc_id      = aws_vpc.MTBZoneVPC.id

  tags = {
    Name = "MTBZoneLambdaSecurityGroup"
  }
}

resource "aws_db_subnet_group" "MTBZoneDBSubnetGroup" {
  name       = "mtbzone-db-subnet-group"
  subnet_ids = aws_subnet.MTBZoneDBSubnet[*].id

  tags = {
    Name = "mtbzone-db-subnet-group"
  }
}

resource "aws_iam_role" "CatalogAPILambdaRole" {
  name = "CatalogAPILambdaRole"

  assume_role_policy = <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Effect": "Allow",
      "Sid": ""
    }
  ]
}
EOF
}

resource "aws_iam_policy" "CatalogAPILambdaPolicy" {
  name        = "CatalogAPILambdaPolicy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = [
          "ec2:CreateNetworkInterface",
          "ec2:DescribeNetworkInterfaces",
          "ec2:DeleteNetworkInterface"
        ]
        Effect   = "Allow"
        Resource = "*"
      },
    ]
  })
}

resource "aws_iam_policy_attachment" "CatalogAPILambdaPolicyAttachment" {
  name       = "CatalogAPILambdaPolicyAttachment"
  roles      = [aws_iam_role.CatalogAPILambdaRole.name]
  policy_arn = aws_iam_policy.CatalogAPILambdaPolicy.arn
}

resource "aws_db_instance" "MTBZoneDB" {
  allocated_storage    = 20
  engine               = "sqlserver-ex"
  engine_version       = "14.00.3401.7.v1"
  instance_class       = "db.t2.micro"
  username             = var.db_username
  password             = var.db_password
  skip_final_snapshot  = true
  license_model = "license-included"
  publicly_accessible = true
  identifier = "mtbzone-db"
  db_subnet_group_name   = aws_db_subnet_group.MTBZoneDBSubnetGroup.name
  vpc_security_group_ids = [aws_security_group.MTBZoneDBSecurityGroup.id]
}

resource "aws_lambda_function" "CatalogAPILambda" {
  filename      = local.CatalogAPI_zipName
  function_name = "CatalogAPILambda"
  role          = aws_iam_role.CatalogAPILambdaRole.arn
  handler       = "bootstrap"
  runtime = "provided.al2"

  source_code_hash = filebase64sha256(local.CatalogAPI_zipName)
  vpc_config {
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  }
}