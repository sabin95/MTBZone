output "APIUrl" {
  value = aws_apigatewayv2_stage.APIGWStage.invoke_url
}