module "CatalogAPILambda"{
    source = "./lambda_api_module"
    api_name = "CatalogAPI"
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
}

module "CartAPILambda"{
    source = "./lambda_api_module"
    api_name = "CartAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
    additional_environment_variables = {
                                        cartExchange = aws_sns_topic.CartAPITopic.arn
                                        ASPNETCORE_ENVIRONMENT = "Production"
                                        }
    db_password = var.db_password
    db_username = var.db_username
}


module "OrdersAPILambda"{
    source = "./lambda_api_module"
    api_name = "OrdersAPI"
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
}
