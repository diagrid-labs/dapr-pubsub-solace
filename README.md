# Dapr pub/sub using Solace Cloud

In this demo, you'll run publisher and subscriber microservices locally, and send/receive messages using [Solace Cloud](https://solace.com/products/platform/cloud/), to demonstrate how Dapr enables a [publish-subscribe](https://docs.dapr.io/developing-applications/building-blocks/pubsub/) pattern.

The publisher service (`checkout`) will generate messages and send these to a specific topic (`orders`), while the subscriber service (`order-processor`) will listen for messages at the same topic. See the [Dapr pub-sub overview](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/) to understand when this pattern might be a good choice for your software architecture.

With Dapr, your application code is using set of common building block APIs. The pub/sub API is such a building block. Dapr allows to switch between different pub/sub implementations by using component files. These are yaml files that contain resource specific information. This means your application code is not coupled to the implementation of the pub/sub resource. In this demo the [Solace AMQP component](https://docs.dapr.io/reference/components-reference/supported-pubsub/setup-solace-amqp/) is used.

## Prerequisites

1. [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
3. Solace Cloud account. If you don't have one, you can [sign up for a free trial](https://console.solace.cloud/login/new-account).

## 1. Setup Solace Cloud

1. In the Solace Cloud portal, go to Cluster Manager and create a new service.
2. Provide a service name, service type, and choose a cloud where the service will be deployed.
3. Click Create Service and wait until the service is deployed.
4. Once the service is deployed, navigate to the Connect tab of the service and expand AMQP section. Note the information underneath the Connection Details. This will be needed in the Dapr pubsub component file.

![Solcate AMQP Connection Details](/images/solace_cloud_amqp_connectiondetails.png)

## 2. Update the Dapr pubsub component file

1. Clone this repository locally and open it in your IDE.
2. Rename the `resources/pubsub.yaml.template` file to `resources/pubsub.yaml`. This is the Dapr pub/sub component file that will contain the details to connect to Solace Cloud.

    > The `pubsub.yaml` file has been added to `.gitignore` to prevent accidental check-in of credentials for this demo. For production use, the yaml files should be checked into source control and [secret store references](https://docs.dapr.io/operations/components/component-secrets/) should be used, instead of plain text values.

3. Copy the username, password, and the secured AMQP host values from the Solace AMPQ Connection Details to the ``pubsub.yaml` file.

    The result should look like this:

    ```yaml
    apiVersion: dapr.io/v1alpha1
    kind: Component
    metadata:
    name: pubsub-component
    spec:
    type: pubsub.solace.amqp
    version: v1
    metadata:
    - name: url
        value: "amqps://<SOLACE_SECURED_AMQP_HOST>"
    - name: username
        value: "solace-cloud-client"
    - name: password
        value: "<SOLACE_PASSWORD>"
    ```

## 3. Run Dotnet message subscriber with Dapr

1. Navigate to the `order-processor` directory and build the project:

    ```bash
    cd ./order-processor
    dotnet build
    ```

2. Run the .NET subscriber app with Dapr:

    ```bash
    dapr run --app-id order-processor --resources-path ../resources/ --app-port 7002 -- dotnet run
    ```

## 4. Run Dotnet message publisher with Dapr

1. Navigate to the `checkout` directory  and build the project:

    ```bash
    cd ./checkout
    dotnet restore
    dotnet build
    ```

2. Run the Dotnet publisher app with Dapr:

    ```bash
    dapr run --app-id checkout-sdk --resources-path ../resources/ -- dotnet run
    ```

3. The following output should be visible in the terminal:

    *Publisher output:*

    ```bash
    == APP == Published data: Order { OrderId = 1 }
    == APP == Published data: Order { OrderId = 2 }
    == APP == Published data: Order { OrderId = 3 }
    == APP == Published data: Order { OrderId = 4 }
    == APP == Published data: Order { OrderId = 5 }
    == APP == Published data: Order { OrderId = 6 }
    == APP == Published data: Order { OrderId = 7 }
    == APP == Published data: Order { OrderId = 8 }
    == APP == Published data: Order { OrderId = 9 }
    == APP == Published data: Order { OrderId = 10 }
    ```

    *Subscriber output:*

    ```bash
    == APP == Subscriber received : Order { OrderId = 1 }
    == APP == Subscriber received : Order { OrderId = 2 }
    == APP == Subscriber received : Order { OrderId = 3 }
    == APP == Subscriber received : Order { OrderId = 4 }
    == APP == Subscriber received : Order { OrderId = 5 }
    == APP == Subscriber received : Order { OrderId = 6 }
    == APP == Subscriber received : Order { OrderId = 7 }
    == APP == Subscriber received : Order { OrderId = 8 }
    == APP == Subscriber received : Order { OrderId = 9 }
    == APP == Subscriber received : Order { OrderId = 10 }
    ```
