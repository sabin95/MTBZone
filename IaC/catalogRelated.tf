# locals{
#   CatalogAPI_zipName = "../CatalogAPI/bin/Release/net6.0/CatalogAPI.zip"
#   CatalogAPIFunctionName = "CatalogAPILambda"
# }

# resource "aws_iam_role" "CatalogAPILambdaRole" {
#   name = "CatalogAPILambdaRole"

#   assume_role_policy = <<EOF
# {
#   "Version": "2012-10-17",
#   "Statement": [
#     {
#       "Action": "sts:AssumeRole",
#       "Principal": {
#         "Service": "lambda.amazonaws.com"
#       },
#       "Effect": "Allow",
#       "Sid": ""
#     }
#   ]
# }
# EOF
# }

# resource "aws_iam_policy" "CatalogAPILambdaPolicy" {
#   name        = "CatalogAPILambdaPolicy"

#   policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Action = [
#           "ec2:CreateNetworkInterface",
#           "ec2:DescribeNetworkInterfaces",
#           "ec2:DeleteNetworkInterface"
#         ]
#         Effect   = "Allow"
#         Resource = "*"
#       },
#       {
#           "Effect": "Allow",
#           "Action": [
#               "logs:CreateLogStream",
#               "logs:PutLogEvents"
#           ],
#           "Resource": [
#               "${aws_cloudwatch_log_group.CatalogAPILambdaLogGroup.arn}:*"
#           ]
#       }
#     ]
#   })
# }

# resource "aws_iam_policy_attachment" "CatalogAPILambdaPolicyAttachment" {
#   name       = "CatalogAPILambdaPolicyAttachment"
#   roles      = [aws_iam_role.CatalogAPILambdaRole.name]
#   policy_arn = aws_iam_policy.CatalogAPILambdaPolicy.arn
# }



# resource "aws_lambda_function" "CatalogAPILambda" {
#   depends_on = [
#     aws_iam_policy_attachment.CatalogAPILambdaPolicyAttachment
#   ]
#   filename      = local.CatalogAPI_zipName
#   function_name = local.CatalogAPIFunctionName
#   role          = aws_iam_role.CatalogAPILambdaRole.arn
#   handler       = "bootstrap"
#   runtime = "provided.al2"
#   timeout = 60

#   source_code_hash = filebase64sha256(local.CatalogAPI_zipName)
#   vpc_config {
#     subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
#     security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
#   }
#   environment {
#     variables = {
#       ConnectionString = "Server=${aws_db_instance.MTBZoneDB.address};Database=MTBZone; user id=${var.db_username};password=${var.db_password};"
#       "LAMBDA_NET_SERIALIZER_DEBUG" = true
#       ordersReceiverQueue = aws_sqs_queue.CatalogAPIOrdersQueue.arn
#     }
#   }
# }

# resource "aws_cloudwatch_log_group" "CatalogAPILambdaLogGroup" {
#   name = "/aws/lambda/${local.CatalogAPIFunctionName}"

#   retention_in_days = 7
# }


# resource "aws_apigatewayv2_api" "CatalogAPIGW" {
#   name          = "CatalogAPIGW"
#   protocol_type = "HTTP"
# }

# resource "aws_apigatewayv2_stage" "CatalogAPIGWStage" {
#   api_id = aws_apigatewayv2_api.CatalogAPIGW.id

#   name        = "dev"
#   auto_deploy = true

#   access_log_settings {
#     destination_arn = aws_cloudwatch_log_group.CatalogAPIGWLogGroup.arn

#     format = jsonencode({
#       requestId               = "$context.requestId"
#       sourceIp                = "$context.identity.sourceIp"
#       requestTime             = "$context.requestTime"
#       protocol                = "$context.protocol"
#       httpMethod              = "$context.httpMethod"
#       resourcePath            = "$context.resourcePath"
#       routeKey                = "$context.routeKey"
#       status                  = "$context.status"
#       responseLength          = "$context.responseLength"
#       integrationErrorMessage = "$context.integrationErrorMessage"
#       }
#     )
#   }
# }

# resource "aws_cloudwatch_log_group" "CatalogAPIGWLogGroup" {
#   name = "/aws/api_gw/${aws_apigatewayv2_api.CatalogAPIGW.name}"

#   retention_in_days = 7
# }

# resource "aws_apigatewayv2_integration" "CatalogAPIGWIntegration" {
#   api_id           = aws_apigatewayv2_api.CatalogAPIGW.id
#   integration_type = "AWS_PROXY"  

#   integration_method        = "POST"
#   integration_uri           = aws_lambda_function.CatalogAPILambda.invoke_arn
#   payload_format_version    = "2.0"
# }

# resource "aws_apigatewayv2_route" "CatalogAPIDefaultRoute" {
#   api_id    = aws_apigatewayv2_api.CatalogAPIGW.id
#   route_key = "$default"

#   target = "integrations/${aws_apigatewayv2_integration.CatalogAPIGWIntegration.id}"
# }

# resource "aws_lambda_permission" "CatalogAPIGWPermission" {
#   statement_id  = "CatalogAPIGWPermission"
#   action        = "lambda:InvokeFunction"
#   function_name = aws_lambda_function.CatalogAPILambda.function_name
#   principal     = "apigateway.amazonaws.com"

#   source_arn = "${aws_apigatewayv2_api.CatalogAPIGW.execution_arn}/*/*"
# }