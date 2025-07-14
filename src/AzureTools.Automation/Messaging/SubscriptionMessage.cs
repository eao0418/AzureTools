// SubscriptionMessage.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Messaging
{
    public sealed class SubscriptionMessage : ObjectMessage
    {
        public string? SubscriptionId { get; set; }
    }
}
