using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheJituMessageBus
{

   
    public  class MessageBus:IMessageBus
    {
        public string ConnectionString = "Endpoint=sb://thejitu.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=o9gPW+LLDpGBQq3HaC/U7frxHQDI2uELz+ASbB4meYc=";

        public async Task PublishMessage(object message, string queue_topic_name)
        {
            var serviceBus = new ServiceBusClient(this.ConnectionString);
            var sender = serviceBus.CreateSender(queue_topic_name);

            var jsonMessage=JsonConvert.SerializeObject(message);

            var theMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {

                //Give a unique iDentifier
                CorrelationId = Guid.NewGuid().ToString(),
            };


            //send the Message
            await sender.SendMessageAsync(theMessage);
            //clean up Resource
            await serviceBus.DisposeAsync();
        }
    }
}
