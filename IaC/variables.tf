variable "db_username" {
  
}

variable "db_password" {
  
}

variable "function_name" {

}

variable "file_name" {

}

variable "aws_iam_role_name"{

}

variable "aws_iam_policy_name" {

}

variable "aws_cloudwatch_log_group_name" {
    default     = "/aws/lambda/${var.function_name}"
}

variable "aws_iam_policy_attachment_name"{

}

variable "aws_apigatewayv2_api_name"{

}

variable "aws_lambda_permission_statement_name"{

}