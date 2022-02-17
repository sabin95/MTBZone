locals{
  function_name = "${var.service_name}Lambda"
}

resource "aws_iam_role" "LambdaRole" {
  name = "${var.service_name}LambdaRole"

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

resource "aws_iam_policy" "LambdaPolicy" {
  name        = "${var.service_name}LambdaPolicy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = concat([
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
              "${aws_cloudwatch_log_group.LambdaLogGroup.arn}:*"
          ]
      }
    ],var.extra_lambda_permissions)
  })
}

resource "aws_iam_policy_attachment" "LambdaPolicyAttachment" {
  name       = "${var.service_name}LambdaPolicyAttachment"
  roles      = [aws_iam_role.LambdaRole.name]
  policy_arn = aws_iam_policy.LambdaPolicy.arn
}

resource "aws_lambda_function" "Lambda" {
  depends_on = [
    aws_iam_policy_attachment.LambdaPolicyAttachment
  ]
  filename      = var.zip_path
  function_name = local.function_name
  role          = aws_iam_role.LambdaRole.arn
  handler       = "bootstrap"
  runtime = "provided.al2"
  timeout = 30

  source_code_hash = filebase64sha256(var.zip_path)
  vpc_config {
    subnet_ids = var.subnet_ids
    security_group_ids = var.security_group_ids
  }
  environment {
    variables = merge({
      ConnectionString = "Server=${var.db_server_address};Database=MTBZone; user id=${var.db_username};password=${var.db_password};",
      "LAMBDA_NET_SERIALIZER_DEBUG" = true
    },var.additional_environment_variables)
  }
}

resource "aws_cloudwatch_log_group" "LambdaLogGroup" {
  name = "/aws/lambda/${local.function_name}"

  retention_in_days = 7
}