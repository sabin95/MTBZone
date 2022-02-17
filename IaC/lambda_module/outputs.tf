output "lambda_arn"{
    value = aws_lambda_function.Lambda.invoke_arn
}

output "lambda_name"{
    value = aws_lambda_function.Lambda.function_name
}