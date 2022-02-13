resource "aws_sns_topic" "CartAPITopic" {
  name = "CartAPITopic"
}

resource "aws_sqs_queue" "OrdersAPICartsQueue" {
  name = "OrdersAPICartsQueue"
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

resource "aws_sns_topic" "OdersAPITopic" {
  name = "OdersAPITopic"
}

resource "aws_sqs_queue" "CatalogAPIOrdersQueue" {
  name = "CatalogAPIOrdersQueue"  
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