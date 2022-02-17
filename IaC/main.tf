module "CatalogAPILambda"{
    source = "./lambda_module"
    service_name = "CatalogAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
    additional_environment_variables = {
                                        ordersReceiverQueue = aws_sqs_queue.CatalogAPIOrdersQueue.arn
                                        ordersReceiverExchange = aws_sns_topic.OdersAPITopic.arn
                                        ASPNETCORE_ENVIRONMENT = "Production"
                                        }
    db_password = var.db_password
    db_username = var.db_username
    zip_path = "../CatalogAPI/bin/Release/net6.0/CatalogAPI.zip"
}

module "CatalogAPIGateway"{
    source = "./lambda_api_gateway_module"
    api_name = "CatalogAPI"
    lambda_arn = module.CatalogAPILambda.lambda_arn
    lambda_name = module.CatalogAPILambda.lambda_name
}

module "CartAPILambda"{
    source = "./lambda_module"
    service_name = "CartAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
    additional_environment_variables = {
                                        cartExchange = aws_sns_topic.CartAPITopic.arn
                                        ASPNETCORE_ENVIRONMENT = "Production"
                                        }
    db_password = var.db_password
    db_username = var.db_username
    zip_path = "../CartAPI/bin/Release/net6.0/CartAPI.zip"
}

module "CartAPIGateway"{
    source = "./lambda_api_gateway_module"
    api_name = "CartAPI"
    lambda_arn = module.CartAPILambda.lambda_arn
    lambda_name = module.CartAPILambda.lambda_name
}


module "OrdersAPILambda"{
    source = "./lambda_module"
    service_name = "OrdersAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
    additional_environment_variables = {
                                        cartsReceiverQueue = aws_sqs_queue.OrdersAPICartsQueue.arn
                                        cartsReceiverExchange = aws_sns_topic.CartAPITopic.arn
                                        odersExchange = aws_sns_topic.OdersAPITopic.arn
                                        ASPNETCORE_ENVIRONMENT = "Production"
                                        }
    db_password = var.db_password
    db_username = var.db_username
    zip_path = "../OrdersAPI/bin/Release/net6.0/OrdersAPI.zip" 
}

module "OrdersAPIGateway"{
    source = "./lambda_api_gateway_module"
    api_name = "OrdersAPI"
    lambda_arn = module.OrdersAPILambda.lambda_arn
    lambda_name = module.OrdersAPILambda.lambda_name
}



