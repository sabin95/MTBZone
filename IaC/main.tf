module "CatalogAPILambda" {
  source             = "./lambda_module"
  service_name       = "CatalogAPI"
  subnet_ids         = aws_subnet.MTBZoneLambdaSubnet[*].id
  security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  db_server_address  = aws_db_instance.MTBZoneDB.address
  additional_environment_variables = {
    ordersReceiverQueue    = aws_sqs_queue.CatalogAPIOrdersQueue.arn
    ordersReceiverExchange = aws_sns_topic.OrdersAPITopic.arn
    ASPNETCORE_ENVIRONMENT = "Production"
  }
  db_password = var.db_password
  db_username = var.db_username
  src_path    = "../CatalogAPI"
}

module "CatalogAPIGateway" {
  source      = "./lambda_api_gateway_module"
  api_name    = "CatalogAPI"
  lambda_arn  = module.CatalogAPILambda.lambda_arn
  lambda_name = module.CatalogAPILambda.lambda_name
}

module "CartsAPILambda" {
  source             = "./lambda_module"
  service_name       = "CartsAPI"
  subnet_ids         = aws_subnet.MTBZoneLambdaSubnet[*].id
  security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  db_server_address  = aws_db_instance.MTBZoneDB.address
  additional_environment_variables = {
    cartsExchange          = aws_sns_topic.CartsAPITopic.arn
    ASPNETCORE_ENVIRONMENT = "Production"
  }
  extra_lambda_permissions = [
    {
      "Effect" : "Allow",
      "Action" : [
        "sns:Publish"
      ],
      "Resource" : [
        aws_sns_topic.CartsAPITopic.arn
      ]
    }
  ]
  db_password = var.db_password
  db_username = var.db_username
  src_path    = "../CartsAPI"
}

module "CartsAPIGateway" {
  source      = "./lambda_api_gateway_module"
  api_name    = "CartsAPI"
  lambda_arn  = module.CartsAPILambda.lambda_arn
  lambda_name = module.CartsAPILambda.lambda_name
}


module "OrdersAPILambda" {
  source             = "./lambda_module"
  service_name       = "OrdersAPI"
  subnet_ids         = aws_subnet.MTBZoneLambdaSubnet[*].id
  security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  db_server_address  = aws_db_instance.MTBZoneDB.address
  additional_environment_variables = {
    cartsReceiverQueue     = aws_sqs_queue.OrdersAPICartsQueue.arn
    cartsReceiverExchange  = aws_sns_topic.CartsAPITopic.arn
    ordersExchange         = aws_sns_topic.OrdersAPITopic.arn
    ASPNETCORE_ENVIRONMENT = "Production"
  }  
  db_password = var.db_password
  db_username = var.db_username
  src_path    = "../OrdersAPI"
}

module "OrdersAPIGateway" {
  source      = "./lambda_api_gateway_module"
  api_name    = "OrdersAPI"
  lambda_arn  = module.OrdersAPILambda.lambda_arn
  lambda_name = module.OrdersAPILambda.lambda_name
}



