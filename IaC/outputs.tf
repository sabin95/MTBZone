output "DBHost" {
  value = aws_db_instance.MTBZoneDB.address
}

output "CatalogAPIUrl" {
  value = "${module.CatalogAPILambda.APIUrl}"
}

output "CartAPIUrl" {
  value = "${module.CartAPILambda.APIUrl}"
}

output "OrdersAPIUrl" {
  value = "${module.OrdersAPILambda.APIUrl}"
}

output "CatalogAPIOrdersQueue" {
  value = aws_sqs_queue.CatalogAPIOrdersQueue.url
}

output "OdersAPITopic" {
  value = aws_sns_topic.OdersAPITopic.arn
}

output "CartAPITopic" {
  value = aws_sns_topic.CartAPITopic.arn
}

output "OrdersAPICartsQueue" {
  value = aws_sqs_queue.OrdersAPICartsQueue.url
}