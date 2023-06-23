using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connectionString = "Endpoint=sb://ziegweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5kT4rJiT6/HMzz1NVRUqZ06MUoCKlkdkF+ASbBRkij4=";
        //private string connectionString = "Endpoint=sb://ziegweb.servicebus.windows.net/;SharedAccessKeyName=Root;SharedAccessKey=Y6LvG22ChhYUvs0371F3kNOII9oTb5pHG+ASbHdw1ek=;EntityPath=emailshoppingcart";
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessge = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };
            await sender.SendMessageAsync(finalMessge);
            await client.DisposeAsync();
        }
    }
}
