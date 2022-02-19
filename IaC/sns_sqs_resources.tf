resource "aws_sns_topic" "CartAPITopic" {
  name = "CartAPITopic"
}

resource "aws_sqs_queue" "OrdersAPICartsQueue" {
  name                       = "OrdersAPICartsQueue"
  visibility_timeout_seconds = 60
}

resource "aws_sqs_queue_policy" "OrdersAPICartsQueuePolicy" {
  queue_url = aws_sqs_queue.OrdersAPICartsQueue.id

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Id": "sqspolicy",
  "Statement": [
    {
      "Sid": "First",
      "Effect": "Allow",
      "Principal": "*",
      "Action": "sqs:SendMessage",
      "Resource": "${aws_sqs_queue.OrdersAPICartsQueue.arn}",
      "Condition": {
        "ArnEquals": {
          "aws:SourceArn": "${aws_sns_topic.CartAPITopic.arn}"
        }
      }
    }
  ]
}
POLICY
}

resource "aws_sns_topic_subscription" "OrdersAPICartsQueueSubscription" {
  topic_arn = aws_sns_topic.CartAPITopic.arn
  protocol  = "sqs"
  endpoint  = aws_sqs_queue.OrdersAPICartsQueue.arn
}

module "OrdersAPIEventHandlersLambda" {
  source             = "./lambda_module"
  service_name       = "OrdersAPIEventHandlers"
  subnet_ids         = aws_subnet.MTBZoneLambdaSubnet[*].id
  security_group_ids = [aws_security_group.MTBZoneLambdaSecurityGroup.id]
  db_server_address  = aws_db_instance.MTBZoneDB.address
  additional_environment_variables = {
    cartsReceiverQueue     = aws_sqs_queue.OrdersAPICartsQueue.arn
    cartsReceiverExchange  = aws_sns_topic.CartAPITopic.arn
    odersExchange          = aws_sns_topic.OdersAPITopic.arn
    ASPNETCORE_ENVIRONMENT = "Production"
  }
  db_password = var.db_password
  db_username = var.db_username
  src_path    = "../OrdersAPI.EventHandlers"
  extra_lambda_permissions = [
    {
      "Effect" : "Allow",
      "Action" : [
        "sqs:ReceiveMessage",
        "sqs:DeleteMessage",
        "sqs:GetQueueAttributes"
      ],
      "Resource" : [
        aws_sqs_queue.OrdersAPICartsQueue.arn
      ]
    }
  ]
}

resource "aws_lambda_permission" "APIGWPermission" {
  statement_id  = "OrdersAPISQSPermission"
  action        = "lambda:InvokeFunction"
  function_name = module.OrdersAPIEventHandlersLambda.lambda_name
  principal     = "sqs.amazonaws.com"

  source_arn = aws_sqs_queue.OrdersAPICartsQueue.arn
}

resource "aws_lambda_event_source_mapping" "OrdersAPIEventHandlersLambdaEventSource" {
  event_source_arn = aws_sqs_queue.OrdersAPICartsQueue.arn
  function_name    = module.OrdersAPIEventHandlersLambda.lambda_name
}

resource "aws_sns_topic" "OdersAPITopic" {
  name = "OdersAPITopic"
}

resource "aws_sqs_queue" "CatalogAPIOrdersQueue" {
  name                       = "CatalogAPIOrdersQueue"
  visibility_timeout_seconds = 60
}

resource "aws_sqs_queue_policy" "CatalogAPIOrdersQueuePolicy" {
  queue_url = aws_sqs_queue.CatalogAPIOrdersQueue.id

  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Id": "sqspolicy",
  "Statement": [
    {
      "Sid": "First",
      "Effect": "Allow",
      "Principal": "*",
      "Action": "sqs:SendMessage",
      "Resource": "${aws_sqs_queue.CatalogAPIOrdersQueue.arn}",
      "Condition": {
        "ArnEquals": {
          "aws:SourceArn": "${aws_sns_topic.OdersAPITopic.arn}"
        }
      }
    }
  ]
}
POLICY
}

resource "aws_sns_topic_subscription" "CatalogAPIOrdersQueueSubscription" {
  topic_arn = aws_sns_topic.OdersAPITopic.arn
  protocol  = "sqs"
  endpoint  = aws_sqs_queue.CatalogAPIOrdersQueue.arn
}
