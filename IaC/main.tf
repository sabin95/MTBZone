terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "3.74.0"
    }
    http = {
      source = "hashicorp/http"
      version = "2.1.0"
    }
  }
}

locals{
  CatalogAPI_zipName = "../CatalogAPI/bin/Release/net6.0/CatalogAPI.zip"
}

provider "http" {
}

provider "aws" {
  region = "eu-central-1"
}

variable "db_username" {
  
}

variable "db_password" {
  
}

data "http" "myip" {
  url = "http://ipv4.icanhazip.com"
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

resource "aws_route_table" "MTBZoneRouteTable" {
  vpc_id = aws_vpc.MTBZoneVPC.id  

  tags = {
    Name = "MTBZoneRouteTable"
  }
}

resource "aws_route" "MTBZoneRouteTableIGWRoute" {
  route_table_id          = aws_route_table.MTBZoneRouteTable.id
  destination_cidr_block  = "0.0.0.0/0"
  gateway_id              = aws_internet_gateway.MTBZoneiGW.id
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

resource "aws_route_table_association" "MTBZoneRouteTableToDBSubnet" {
  count = length(aws_subnet.MTBZoneDBSubnet)
  route_table_id = aws_route_table.MTBZoneRouteTable.id
  subnet_id      = aws_subnet.MTBZoneDBSubnet[count.index].id
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

resource "aws_security_group_rule" "MTBZoneDBSecurityGroupLambdaIngressRule" {
  type              = "ingress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneLambdaSecurityGroup.id
}

resource "aws_security_group_rule" "MTBZoneDBSecurityGroupMyipIngressRule" {
  type              = "ingress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
  cidr_blocks = ["${chomp(data.http.myip.body)}/32"]
}

resource "aws_security_group_rule" "MTBZoneDBSecurityGroupMyipEgressRule" {
  type              = "egress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
  cidr_blocks = ["${chomp(data.http.myip.body)}/32"]
}

resource "aws_security_group_rule" "MTBZoneLambdaSecurityGroupDBEgressRule" {
  type              = "egress"
  from_port         = 1433
  to_port           = 1433
  protocol          = "tcp"
  security_group_id = aws_security_group.MTBZoneLambdaSecurityGroup.id
  source_security_group_id = aws_security_group.MTBZoneDBSecurityGroup.id
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
      {
          "Effect": "Allow",
          "Action": [
              "logs:CreateLogStream",
              "logs:PutLogEvents"
          ],
          "Resource": [
              "${aws_cloudwatch_log_group.CatalogAPILambdaLogGroup.arn}:*"
          ]
      }
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
  timeout = 60

  source_code_hash = filebase64sha256(local.CatalogAPI_zipName)
  vpc_config {
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  }
  environment {
    variables = {
      ConnectionString = "Server=${aws_db_instance.MTBZoneDB.address};Database=MTBZone; user id=${var.db_username};password=${var.db_password};"
      "LAMBDA_NET_SERIALIZER_DEBUG" = true
    }
  }
}

resource "aws_cloudwatch_log_group" "CatalogAPILambdaLogGroup" {
  name = "/aws/lambda/${aws_lambda_function.CatalogAPILambda.function_name}"

  retention_in_days = 7
}


resource "aws_apigatewayv2_api" "CatalogAPIGW" {
  name          = "CatalogAPIGW"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "CatalogAPIGWStage" {
  api_id = aws_apigatewayv2_api.CatalogAPIGW.id

  name        = "dev"
  auto_deploy = true

  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.CatalogAPIGWLogGroup.arn

    format = jsonencode({
      requestId               = "$context.requestId"
      sourceIp                = "$context.identity.sourceIp"
      requestTime             = "$context.requestTime"
      protocol                = "$context.protocol"
      httpMethod              = "$context.httpMethod"
      resourcePath            = "$context.resourcePath"
      routeKey                = "$context.routeKey"
      status                  = "$context.status"
      responseLength          = "$context.responseLength"
      integrationErrorMessage = "$context.integrationErrorMessage"
      }
    )
  }
}

resource "aws_cloudwatch_log_group" "CatalogAPIGWLogGroup" {
  name = "/aws/api_gw/${aws_apigatewayv2_api.CatalogAPIGW.name}"

  retention_in_days = 7
}

resource "aws_apigatewayv2_integration" "CatalogAPIGWIntegration" {
  api_id           = aws_apigatewayv2_api.CatalogAPIGW.id
  integration_type = "AWS_PROXY"  

  integration_method        = "POST"
  integration_uri           = aws_lambda_function.CatalogAPILambda.invoke_arn
  payload_format_version    = "2.0"
}

resource "aws_lambda_permission" "CatalogAPIGWPermission" {
  statement_id  = "CatalogAPIGWPermission"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.CatalogAPILambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.CatalogAPIGW.execution_arn}/*/*"
}
