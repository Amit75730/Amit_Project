using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using EdiWebAPI.Models;

namespace EdiWebAPI.Services
{
    public class ServiceBusSenderService
    {
        private readonly string _connectionString;
        private readonly string _topicName;

        public ServiceBusSenderService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"];
            _topicName = configuration["AzureServiceBus:TopicName"];
        }

        public async Task SendMessageAsync(DemurrageFees demurrageFees, string containerNumber)
        {
            // Create the client and sender for Service Bus Topic
            var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_topicName);

            try
            {
                // Create the payload message to be sent
                var messagePayload = new
                {
                    ContainerNumber = containerNumber,
                    DemurrageFees = demurrageFees
                };

                // Serialize the message payload into JSON
                var messageBody = JsonConvert.SerializeObject(messagePayload);

                // Create the ServiceBusMessage with the serialized JSON payload
                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

                // Send the message to the topic
                await sender.SendMessageAsync(message);
                Console.WriteLine("Message sent to Service Bus topic.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
            finally
            {
                // Clean up resources
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
