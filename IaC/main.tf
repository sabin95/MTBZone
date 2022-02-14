module "CatalogAPILambda"{
    source = "./lambda_api_module"
    api_name = "CatalogAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
}

module "CartAPILambda"{
    source = "./lambda_api_module"
    api_name = "CartAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
}


module "OrdersAPILambda"{
    source = "./lambda_api_module"
    api_name = "OrdersAPI"
    subnet_ids = [aws_subnet.MTBZoneLambdaSubnet.id]
    security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
    db_server_address = aws_db_instance.MTBZoneDB.address
}
