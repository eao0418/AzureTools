// ProviderResourceType.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Resources
{
    using System.Collections.Generic;

    public sealed class ProviderResourceType
    {
        /// <summary>
        /// The resource type of the resource.
        /// </summary>
        public string? ResourceType { get; set; }

        /// <summary>
        /// The collection of locations where this resource type can be created.
        /// </summary>
        public List<string>? Locations { get; set; }

        /// <summary>
        /// The API versions supported by this resource type.
        /// </summary>
        public List<string>? ApiVersions { get; set; }

        /// <summary>
        /// The default API version.
        /// </summary>
        public string? DefaultApiVersion { get; set; }

        /// <summary>
        /// The additional capabilities offered by this resource type.
        /// </summary>
        public string? Capabilities { get; set; }

    }
}
