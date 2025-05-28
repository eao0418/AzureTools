using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureTools.Triggers;
public class QueueTriggers
{
    private readonly ILogger<QueueTriggers> _logger;

    public QueueTriggers(ILogger<QueueTriggers> logger)
    {
        _logger = logger;
    }

    [Function(nameof(QueueTriggers))]
    public void Run([EventHubTrigger("eventhubtrigger", Connection = "connectionstring")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            _logger.LogInformation("Event Body: {body}", @event.Body);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }
    }
}