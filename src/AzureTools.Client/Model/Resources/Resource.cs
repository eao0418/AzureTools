// Resource.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model.Resources
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class for resource information.
    /// See <a href="https://learn.microsoft.com/en-us/rest/api/resources/resources/list?view=rest-resources-2021-04-01#identity">Resources - List</a> for more information.
    /// </summary>
    public class Resource: ModelBase
    {
        /// <summary>
        /// The resource Id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The resource name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The resource type.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// The resource location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// The resource tags.
        /// </summary>
        public Dictionary<string, string> Tags { get; set; } = new();

        /// <summary>
        /// The kind of the resource.
        /// </summary>
        public string? Kind { get; set; }

        /// <summary>
        /// ID of the resource that manages this resource.
        /// </summary>
        public string? ManagedBy { get; set; }

        /// <summary>
        /// The resource properties.
        /// Some property values are a JSON object, and some are a primitive value.
        /// Handle them all generically as objects. Cast them to the appropriate type as needed.
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new();

        /// <summary>
        /// The plan of the resource.
        /// </summary>
        public Plan? Plan { get; set; }

        /// <summary>
        /// The SKU of the resource.
        /// </summary>
        public Sku? Sku { get; set; }

        /// <summary>
        /// The identity of the resource.
        /// </summary>
        public Identity? Identity { get; set; }

        /// <summary>
        /// Resource extended location.
        /// </summary>
        public ExtendedLocation? ExtendedLocation { get; set; }

        /// <summary>
        /// The created time of the resource. This is only present if requested via the $expand query parameter.
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// The changed time of the resource. This is only present if requested via the $expand query parameter.
        /// </summary>
        public DateTime? ChangedTime { get; set; }

        /// <summary>
        /// The provisioning state of the resource. This is only present if requested via the $expand query parameter.
        /// </summary>
        public string? ProvisioningState { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Type: {Type}, Id: {Id}";
        }
    }

}
