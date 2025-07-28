// KafkaMessage.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Arm.Messaging
{
    using System;

    public class KafkaMessage
    {
        public string Topic { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Partition { get; set; }
        public long Offset { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
