using MQTTnet;
using MQTTnet.Client;
using System;

namespace MQTTClientApp.Publisher 
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            Console.WriteLine("Publisher starting ...");

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var options = new MqttClientOptionsBuilder()
                   .WithClientId("PublisherClient")
                   .WithTcpServer("localhost", 1883)
                   .WithCleanSession()
                   .Build();

                mqttClient.ConnectedAsync += (s) =>
                {
                    Console.WriteLine("Publisher connected !");

                    return Task.CompletedTask;
                };

                mqttClient.DisconnectedAsync += (s) =>
                {
                    Console.WriteLine("Publisher disconnected !");

                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(options, CancellationToken.None);

                Console.WriteLine("Press any key to publish a message");

                Console.ReadLine();

                PublishMessage(mqttClient, "Salam");

                Console.ReadLine();
            }

        }

        private static void PublishMessage(IMqttClient client ,string messageText)
        {
            var message = new MqttApplicationMessageBuilder()
            .WithTopic("tests/console")
            .WithPayload(messageText)
            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

            if(client.IsConnected)
            {
                client.PublishAsync(message, CancellationToken.None);

                Console.WriteLine("Message published");
            }
            else
            {
                Console.WriteLine("Message not published : client is disconnected!");
            }
        }




    }
}