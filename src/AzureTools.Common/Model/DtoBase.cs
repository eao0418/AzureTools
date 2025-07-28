// DtoBase.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model
{
    /// <summary>
    /// A base class for writing properties that are common to all Data Transfer Objects (DTOs) used in the Azure Tools repository.
    /// </summary>
    public class DtoBase
    {
        /// <summary>
        /// /// The time the object info was collected and converted from the Graph API.
        /// </summary>
        public DateTime RecordGenerateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The unique identifier for this data run.
        /// </summary>
        public string ExecutionId { get; set; } = string.Empty;
    }
}
