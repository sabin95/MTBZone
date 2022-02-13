resource "aws_iam_role" "LambdaRole" {
  name = var.aws_iam_role_name

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
  name        = var.aws_iam_policy_name

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
              "${aws_cloudwatch_log_group.LambdaLogGroup.arn}:*"
          ]
      }
    ]
  })
}

resource "aws_iam_policy_attachment" "LambdaPolicyAttachment" {
  name       = var.aws_iam_policy_attachment_name
  roles      = [aws_iam_role.LambdaRole.name]
  policy_arn = aws_iam_policy.LambdaPolicy.arn
}

resource "aws_cloudwatch_log_group" "LambdaLogGroup" {
  name = var.aws_cloudwatch_log_group_name

  retention_in_days = 7
}

resource "aws_lambda_function" "LambdaFunction" {
    depends_on = [
        aws_iam_policy_attachment.LambdaPolicyAttachment
    ]

  filename                       = var.file_name
  function_name                  = var.function_name
  role                           = aws_iam_role.LambdaRole.arn
  handler                        = "bootstrap"
  runtime                        = "provided.al2"
  timeout                        = 60


  
  source_code_hash = filebase64sha256(local.file_name) 
  vpc_config {
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_iam_role.LambdaRole.id]
  }
  environment {
    variables = {
      ConnectionString = "Server=${aws_db_instance.MTBZoneDB.address};Database=MTBZone; user id=${var.db_username};password=${var.db_password};"
      "LAMBDA_NET_SERIALIZER_DEBUG" = true
      ordersReceiverQueue = aws_sqs_queue.CatalogAPIOrdersQueue.arn //need to see what to do here
    }
  }
}

resource "aws_apigatewayv2_api" "APIGW" {
  name          = var.aws_apigatewayv2_api_name
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "APIGWStage" {
  api_id = aws_apigatewayv2_api.APIGW.id

  name        = "dev"
  auto_deploy = true

  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.GWLogGroup.arn

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

resource "aws_cloudwatch_log_group" "GWLogGroup" {
  name = "/aws/api_gw/${aws_apigatewayv2_api.APIGW.name}"

  retention_in_days = 7
}

resource "aws_apigatewayv2_integration" "GWIntegration" {
  api_id           = aws_apigatewayv2_api.APIGW.id
  integration_type = "AWS_PROXY"  

  integration_method        = "POST"
  integration_uri           = aws_lambda_function.LambdaFunction.invoke_arn
  payload_format_version    = "2.0"
}

resource "aws_apigatewayv2_route" "GWDefaultRoute" {
  api_id    = aws_apigatewayv2_api.APIGW.id
  route_key = "$default"

  target = "integrations/${aws_apigatewayv2_integration.GWIntegration.id}"
}

resource "aws_lambda_permission" "GWPermission" {
  statement_id  = var.aws_lambda_permission_statement_name
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.LambdaFunction.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.APIGW.execution_arn}/*/*"
}