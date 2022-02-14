locals{
  API_zipName = "../${var.api_name}/bin/Release/net6.0/${var.api_name}.zip"
  API_function_name = "${var.api_name}Lambda"
}

resource "aws_iam_role" "LambdaRole" {
  name = "${var.api_name}LambdaRole"

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
  name        = "${var.api_name}LambdaPolicy"

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
  name       = "${var.api_name}LambdaPolicyAttachment"
  roles      = [aws_iam_role.LambdaRole.name]
  policy_arn = aws_iam_policy.LambdaPolicy.arn
}



resource "aws_lambda_function" "APILambda" {
  depends_on = [
    aws_iam_policy_attachment.LambdaPolicyAttachment
  ]
  filename      = local.API_zipName
  function_name = local.API_function_name
  role          = aws_iam_role.LambdaRole.arn
  handler       = "bootstrap"
  runtime = "provided.al2"
  timeout = 60

  source_code_hash = filebase64sha256(local.API_zipName)
  vpc_config {
    subnet_ids = var.subnet_ids
    security_group_ids = var.security_group_ids
  }
  environment {
    variables = {
      ConnectionString = "Server=${var.db_server_address};Database=MTBZone; user id=${var.db_username};password=${var.db_password};"
      "LAMBDA_NET_SERIALIZER_DEBUG" = true
      ordersReceiverQueue = aws_sqs_queue.CatalogAPIOrdersQueue.arn // to edit here
    }
  }
}

resource "aws_cloudwatch_log_group" "LambdaLogGroup" {
  name = "/aws/lambda/${local.API_function_name}"

  retention_in_days = 7
}


resource "aws_apigatewayv2_api" "APIGW" {
  name          = "${var.api_name}GW"
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

resource "aws_apigatewayv2_integration" "APIGWIntegration" {
  api_id           = aws_apigatewayv2_api.APIGW.id
  integration_type = "AWS_PROXY"  

  integration_method        = "POST"
  integration_uri           = aws_lambda_function.APILambda.invoke_arn
  payload_format_version    = "2.0"
}

resource "aws_apigatewayv2_route" "APIDefaultRoute" {
  api_id    = aws_apigatewayv2_api.APIGW.id
  route_key = "$default"

  target = "integrations/${aws_apigatewayv2_integration.APIGWIntegration.id}"
}

resource "aws_lambda_permission" "APIGWPermission" {
  statement_id  = "${var.api_name}GWPermission"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.APILambda.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.APIGW.execution_arn}/*/*"
}