using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;

namespace MQTTClientApp.Listener
{
    public class Program
    {
       public static async Task Main(string[] args)
        {
            Console.WriteLine("Listener starting ...");

            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithClientId("ListenerClient")
                    .WithTcpServer("localhost", 1883)
                    .Build();

                // Setup message handling before connecting so that queued messages
                // are also handled properly. When there is no event handler attached all
                // received messages get lost.
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {
                    Console.WriteLine("Received application message : " + Encoding.UTF8.GetString(e.ApplicationMessage.Payload));

                    return Task.CompletedTask;
                };

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f =>
                    { 
                        f.WithTopic("tests/console"); 

                    }).Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                Console.WriteLine("MQTT client subscribed to topic.");

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }

        }

       
    }
}