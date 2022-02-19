output "DBHost" {
  value = aws_db_instance.MTBZoneDB.address
}

output "CatalogAPIUrl" {
  value = module.CatalogAPIGateway.APIUrl
}

output "CartsAPIUrl" {
  value = module.CartsAPIGateway.APIUrl
}

output "OrdersAPIUrl" {
  value = module.OrdersAPIGateway.APIUrl
}

output "CatalogAPIOrdersQueue" {
  value = aws_sqs_queue.CatalogAPIOrdersQueue.url
}

output "OdersAPITopic" {
  value = aws_sns_topic.OdersAPITopic.arn
}

output "CartsAPITopic" {
  value = aws_sns_topic.CartsAPITopic.arn
}

output "OrdersAPICartsQueue" {
  value = aws_sqs_queue.OrdersAPICartsQueue.url
}
