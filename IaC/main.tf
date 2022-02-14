module "CatalogAPILambda"{
    source = "./lambda_api_module"
    api_name = "CatalogAPI"
}

module "CartAPILambda"{
    source = "./lambda_api_module"
    api_name = "CartAPI"
}


module "OrdersAPILambda"{
    source = "./lambda_api_module"
    api_name = "OrdersAPI"
}
