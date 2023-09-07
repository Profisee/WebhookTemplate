# Profisee Sample .NET Webhook Template using Azure Functions
Use this sample .NET template as a quick outline for setting up a webhook to work with Profisee's Workflow or Event Subscription service through Azure Functions. This is helpful when working with a Platform as a Service (PaaS) instance of Profisee. A Profisee workflow can be configured to send a customized payload to a webhook endpoint where further logic can be implemented to process the incoming payload, and a response can be sent back and utilized by the Workflow Activity. A Profisee event can be configured to send information about an event related to a specific entity.

## Requirements

- .NET Framework 6 runtime
- .NET Framework 6 SDK
- Active Azure Subscription

This sample template is configured for running the Azure Function and does not require a web server technology. This also assumes you are running the Profisee instance as a PaaS offering. There is also no use of Azure Logic apps for this template. If you want more information on deploying Azure Functions and Azure Logic Apps to work with Profisee's Webhook infrastructure, please view the training video on "Advanced Webhooks" [online guide](https://support.profisee.com/lms/courseinfo?id=00u00000000004T00aM)

## Optional

- Visual Studio
- Visual Studio Windows Workflow Foundation component
- Profisee SDK

Included in this project is a webhook activity library with a sample webhook activity. If you want to compile it and register it with the workflow system, you must have Visual Studio installed with the Windows Workflow Foundation component. You must also have the Profisee SDK. To read more about installing the Profisee SDK, visit our [online guide](https://support.profisee.com/wikis/2022_r1_support/profisee_sdk_installation). If you do not wish to do this, you can import the SimpleWebhookActivity.wf workflow file definition.

## Webhook Setup

Compile the the *WebhookTemplate* project and publish it to the Azure Portal, from here you can create a new Azure Function App, or you can publish it to one you already created. Once that is created, you can then go to the Azure Portals and on your Function App's configuration settings, and create new application settings for:

- LoggingFilePath (Optional) - Provides an additional file output location for logging information. Regardless of what is provided here, logging information will be sent to the console and to a file local to the executable location called *Logs/Log.log*.
- ServiceUrl (Required) - Set this to the value of your Profisee Service Url. This URL can be found in your Kubernetes Service under the configuration map "profisee-settings". The url will be named "ProfiseeExternalDNSUrl.

Please note that these values also exist under the "local.settings.json" file in the Azure Function project. You can directly modify the ServiceUrl and LoggingFilePath there to point to a local instance of profisee when locally testing/debugging, and the function app will still use the global setting you set in your configuration settings.

## Workflow Setup

To test against the provided workflow, you must restore the model, archive, and workflow provided in the **Additional Files** folder. Configure the workflow's **Endpoint URL** value to the URL for the webhook. E.g, https://azurefunctionappURL.azurewebsites.net/api/WorkflowUpdateEntityDescription. It can also be set to a local host directory for debugging purposes

## Event Subscriber Setup

1. In FastApp Studio, navigate to the **Real-Time Event Processing** tab.
2. Disable real-time event processing.
3. In the **Subscribers** page, ensure the Webhook Subscriber is under the **Registered Subscribers** list.
4. In the **subscriber-configuration** page, create a new configuration. Give it a name, ensure the subscriber is set to the webhook subscriber, configure the *run-as* user under the properties section, then set the url for your webhook subscriber endpoint. E.g, https://azurefunctionappURL.azurewebsites.net/api/Subscriber
5. Create an event. To do so, you must have a deployed model with an entity to associate your event with. A sample model and archive has been included in the *Additional Files* folder to restore if necessary. Ensure the event is enabled and the subscriber configuration created in the previous step is selected.
6. Enable real-time event processing.

## Azure Portal Setup

1. In Azure Portal, create a function app
2. Once the function is created, go to configuration and add in a “ServiceUrl” application setting and make that URL the application url (ex: https://corpltr11.corp.profisee.com/profisee/). Note App URL should have “/profisee/” at the end
3. Download the template and publish the Azure function app
    - Publish to an Azure target → Azure Function App → select the azure function app you created
    - At this point you can modify the 2 functions to what you need it to do

## Running the functions

1. In postman, create the necessary collection for the instance of Profisee you are running
    - Access Token URL with come from your platform’s AKS cluster’s configuration, the ProfiseeExternalDNSUrl

2. Create Http Post calls for the different endpoints
    - This can be grabbed through the Azure Portal by going into each function, going to code+test and then grabbing the “function Url”

## Endpoints

Two endpoints are available in this application. The URL for these endpoints can be found in the Azure Portal by going into the Azure Function App. From there, select the specific function and you should get taken to the function overview. Once you are there, go to the "Code+Test" tab and get the function URL. They operate in the following manner:

- "api/WorkflowUpdateEntityDescription" - This endpoint will recieve a workflow payload, load a selected entity and record, update the description and pushes the update back to the profisee service. It grabs the Authorization from the header, validates the JWT, binds the content recieved in the http request body to a Workflow Paload object, and will push back the update request to the profisee service. This should be a good foundation for building more complex functionality.
- "api/Subscriber" - This endpoint is to be used with eventing. This endpoint receives a SubscriberPayload and updates the description of the selected record on the entity. It's similar to the workflow function, only working as a subscriber.

## Additional Notes

Files to note:

- **WebhookTemplate.AzureFunction\Functions\UpdateEntityDescription.cs** - This is where the UpdateEntityDescription endpoint is located.
- **WebhookTemplate.AzureFunction\Functions\Subscribers.cs** - This is where the Subscribers endpoint is located.
- **WebhookTemplate.AzureFunction\local.settings.json** - This is where the ServiceUrl can be inputted.
- **WebhookTemplate.AzureFunction\Startup.cs** - This is where the core setup and configuration logic is located. Additional configuration is handled within the extensions folder.