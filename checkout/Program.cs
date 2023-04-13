using System;
using Dapr.Client;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

int i = 1;
while (true) {
    var order = new Order(i);
    using var client = new DaprClientBuilder().Build();

    // Publish an event/message using Dapr PubSub
    await client.PublishEventAsync("pubsub-component", "topic:marc/orders", order);
    Console.WriteLine("Published data: " + order);

    i++;
    await Task.Delay(TimeSpan.FromSeconds(1));
}

public record Order([property: JsonPropertyName("orderId")] int OrderId);
