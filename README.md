# Dapr pub/sub using Solace Cloud

In this demo, you'll run a publisher microservice and a subscriber microservice to demonstrate how Dapr enables a publish-subcribe pattern. The publisher will generate messages of a specific topic, while subscribers will listen for messages of specific topics. See [Why Pub-Sub](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) to understand when this pattern might be a good choice for your software architecture.

Visit [this](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) link for more information about Dapr and Pub-Sub.

This demo includes one publisher:

- Dotnet client message generator `checkout`

And one subscriber:

- Dotnet subscriber `order-processor`

The pub/sub system is [Solace Cloud](https://solace.com/products/platform/cloud/).

## Run Dotnet message subscriber with Dapr

1. Navigate to the directory and build the project to install the dependencies: 

```bash
cd ./order-processor
dotnet build
```

2. Run the Dotnet subscriber app with Dapr: 

```bash
dapr run --app-id order-processor --resources-path ../resources/ --app-port 7002 -- dotnet run
```

## Run Dotnet message publisher with Dapr

1. Navigate to the directory and install dependencies:

```bash
cd ./checkout
dotnet restore
dotnet build
```

2. Run the Dotnet publisher app with Dapr:

```bash
dapr run --app-id checkout-sdk --resources-path ../resources/ -- dotnet run
```

```bash
dapr stop --app-id order-processor
dapr stop --app-id checkout-sdk
```
