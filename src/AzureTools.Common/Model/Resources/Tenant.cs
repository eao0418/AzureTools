// Tenant.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Resources
{
    using System.Collections.Generic;

    public class Tenant : ModelBase
    {
        /// <summary>
        /// The ARM resource Id of the tenant.
        /// </summary>
        public string? Id { get; set; } = string.Empty;
        /// <summary>
        /// The tenant country code.
        /// </summary>
        public string? CountryCode { get; set; } = string.Empty;
        /// <summary>
        /// The tenant display name.
        /// </summary>
        public string? DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// The tenant category, such as "ManagedBy" or "ManagedByMicrosoft". This is used to indicate how the tenant is managed.
        /// </summary>
        public string? TenantCategory { get; set; } = string.Empty;
        /// <summary>
        /// The default domain for the tenant, which is typically the primary domain used for authentication and other tenant-level operations.
        /// </summary>
        public string? DefaultDomain { get; set; } = string.Empty;
        /// <summary>
        /// The type of tenant, such as "AAD" for Azure Active Directory. This indicates the type of identity management system used by the tenant.
        /// </summary>
        public string? TenantType { get; set; } = string.Empty;
        /// <summary>
        /// The list of domains for the tenant.
        /// </summary>
        public List<string> Domains { get; set; } = new();

        // Generate a ToString() method
        public override string ToString()
        {
            return $"{DisplayName} ({Id}) - {CountryCode} - {TenantCategory} - {DefaultDomain} - {TenantType}";
        }
    }
}