locals{
  API_zipName = "../${var.api_name}/bin/Release/net6.0/${var.api_name}.zip"
  API_function_name = "${var.api_name}Lambda"
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
  integration_uri           = var.lambda_arn
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
  function_name = var.lambda_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.APIGW.execution_arn}/*/*"
}