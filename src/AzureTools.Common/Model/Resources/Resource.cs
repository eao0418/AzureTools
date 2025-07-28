// Resource.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AzureTools.Utility;

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

        public static Resource CreateFromReader(IDataReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var resource = new Resource
            {
                Id = reader["Id"] is not DBNull ? reader["Id"] as string : string.Empty,
                Name = reader["Name"] is not DBNull ? reader["Name"] as string : string.Empty,
                Type = reader["Type"] is not DBNull ? reader["Type"] as string : string.Empty,
                Location = reader["Location"] is not DBNull ? reader["Location"] as string : string.Empty,
                Kind = reader["Kind"] is not DBNull ? reader["Kind"] as string : string.Empty,
                ManagedBy = reader["ManagedBy"] is not DBNull ? reader["ManagedBy"] as string : string.Empty,
                CreatedTime = reader["CreatedTime"] as DateTime?,
                ChangedTime = reader["ChangedTime"] as DateTime?,
                ProvisioningState = reader["ProvisioningState"] is not DBNull ? reader["ProvisioningState"] as string : string.Empty,
                TenantId = reader["TenantId"] is not DBNull ? reader["TenantId"] as string : string.Empty,
            };
            if (reader["Tags"] is not DBNull && reader["Tags"] is string tagsJson && !string.IsNullOrWhiteSpace(tagsJson))
            {
                resource.Tags = JsonUtil.Deserialize<Dictionary<string, string>>(tagsJson) ?? new Dictionary<string, string>();
            }
            else
            {
                resource.Tags = new Dictionary<string, string>();
            }
            if (reader["Properties"] is not DBNull)
            {
                var props = reader["Properties"].ToString() ?? string.Empty;
                resource.Properties = JsonUtil.Deserialize<Dictionary<string, object>>(props) ?? new Dictionary<string, object>();
            }
            else
            {
                resource.Properties = new Dictionary<string, object>();
            }
            if (reader["Plan"] is not DBNull)
            {
                resource.Plan = reader["Plan"] as Plan;
            }
            if (reader["Sku"] is not DBNull)
            {
                resource.Sku = reader["Sku"] as Sku;
            }
            if (reader["Identity"] is not DBNull)
            {
                resource.Identity = reader["Identity"] as Identity;
            }
            if (reader["ExtendedLocation"] is not DBNull)
            {
                resource.ExtendedLocation = reader["ExtendedLocation"] as ExtendedLocation;
            }
            return resource;
        }
    }

}
