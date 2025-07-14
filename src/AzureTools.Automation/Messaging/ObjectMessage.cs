// ObjectMessage.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Messaging
{
    public class ObjectMessage
    {
        public string TenantId { get; set; } = string.Empty;
        public string ExecutionId { get; set; } = string.Empty;
        public string AuthSettingsKey { get; set; } = string.Empty;
    }
}
