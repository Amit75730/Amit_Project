using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using EdiWebAPI.Models;

namespace EdiWebAPI.Services
{
    public class ServiceBusReceiverService
    {
        private readonly string _connectionString;
        private readonly string _topicName;
        private readonly string _subscriptionName;

        public ServiceBusReceiverService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureServiceBus:ConnectionString"];
            _topicName = configuration["AzureServiceBus:TopicName"];
            _subscriptionName = configuration["AzureServiceBus:SubscriptionName"];
        }

        public async Task ReceiveMessagesAsync()
        {
            // Create the client and receiver for the Service Bus Topic and Subscription
            var client = new ServiceBusClient(_connectionString);
            var receiver = client.CreateReceiver(_topicName, _subscriptionName);

            try
            {
                // continuous message reception
                await foreach (ServiceBusReceivedMessage message in receiver.ReceiveMessagesAsync())// Receive messages from the subscription
                {
                    // Deserialize the message body to an object
                    var messageBody = Encoding.UTF8.GetString(message.Body);
                    var messagePayload = JsonConvert.DeserializeObject<dynamic>(messageBody);

                    // Extract relevant information from the message payload
                    string containerNumber = messagePayload.ContainerNumber;
                    decimal feesDue = messagePayload.DemurrageFees.FeesDue;
                    decimal feesPaid = messagePayload.DemurrageFees.FeesPaid;

                    Console.WriteLine($"Received message for container {containerNumber}: Fees Due - {feesDue}, Fees Paid - {feesPaid}");

                    // Mark the message as complete so it’s removed from the subscription
                    //await receiver.CompleteMessageAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
            }
            finally
            {
                // Clean up resources
                await receiver.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
