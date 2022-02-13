module "CatalogAPILambda"{
    source = "lamda_module_template.tf" // sau "", pt ca e in acelasi folder?
    function_name = "CatalogAPILambda"
    file_name = "../CatalogAPI/bin/Release/net6.0/CatalogAPI.zip"
    aws_iam_role_name = "CatalogAPILambdaRole"
    aws_iam_policy_name = "CatalogAPILambdaPolicy"
    aws_iam_policy_attachment_name = "CatalogAPILambdaPolicyAttachment"
    aws_apigatewayv2_api_name = "CatalogAPIGW"
    aws_lambda_permission_statement_name = "CatalogAPIGWPermission"
}

module "CartAPILambda"{
    source = "lamda_module_template.tf" // sau "", pt ca e in acelasi folder?
    function_name = "CartAPILambda"
    file_name = "../CartAPI/bin/Release/net6.0/CartAPI.zip"
    aws_iam_role_name = "CartAPILambdaRole"
    aws_iam_policy_name = "CartAPILambdaPolicy"
    aws_iam_policy_attachment_name = "CartAPILambdaPolicyAttachment"
    aws_apigatewayv2_api_name = "CartAPIGW"
    aws_lambda_permission_statement_name = "CartAPIGWPermission"
}

module "OrdersAPILambda"{
    source = "lamda_module_template.tf" // sau "", pt ca e in acelasi folder?
    function_name = "OrdersAPILambda"
    file_name = "../OrdersAPI/bin/Release/net6.0/OrdersAPI.zip"
    aws_iam_role_name = "OrdersAPILambdaRole"
    aws_iam_policy_name = "OrdersAPILambdaPolicy"
    aws_iam_policy_attachment_name = "OrdersAPILambdaPolicyAttachment"
    aws_apigatewayv2_api_name = "OrdersAPIGW"
    aws_lambda_permission_statement_name = "OrdersAPIGWPermission"
}