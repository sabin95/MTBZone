using Amazon.SQS;
using Amazon.SQS.Model;
using MTBZone.Messaging.Receiver;
using Newtonsoft.Json;

namespace MTBZone.Messaging.Receiver
{
    internal class AWSMessage
    {
        public string Message { get; set; }
    }
    public class SQSReceiver : IReceiver
    {
        private AmazonSQSClient sqsClient = new AmazonSQSClient();
        public void Receive<TMessage, THandler>(THandler handler, string queue, string exchange) where THandler : IHandler<TMessage>
        {
            var t = new Thread(async () =>
            {
                while (true)
                {
                    await Task.Delay(5000);
                    var response = await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest() { QueueUrl = queue });
                    if (response == null || response.Messages.Count == 0)
                    {
                        continue;
                    }
                    foreach (var message in response.Messages)
                    {
                        var messageBody = message.Body;
                        var messageObject = JsonConvert.DeserializeObject<AWSMessage>(messageBody);
                        var messageData = JsonConvert.DeserializeObject<TMessage>(messageObject.Message);
                        await handler.Handle(messageData);
                        await sqsClient.DeleteMessageAsync(queue, message.ReceiptHandle);
                    }
                }
            });
            t.Start();            
        }
    }
}
