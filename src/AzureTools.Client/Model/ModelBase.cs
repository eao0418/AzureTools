// ModelBase.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using AzureTools.Repository.Model;
    using System;

    public class ModelBase : DtoBase
    {
        /// <summary>
        /// The unique identifier for the tenant this object belongs to.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
    }
}
