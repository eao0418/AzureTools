// ModelBase.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model
{
    public class ModelBase : DtoBase
    {
        /// <summary>
        /// The unique identifier for the tenant this object belongs to.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
    }
}
