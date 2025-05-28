// DtoBase.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository.Model
{
    public class DtoBase
    {
        /// <summary>
        /// /// The time the object info was collected and converted from the Graph API.
        /// </summary>
        public DateTime CollectionTime = DateTime.UtcNow;

        /// <summary>
        /// The unique identifier for this data run.
        /// </summary>
        public string ExecutionId { get; set; } = string.Empty;
    }
}
