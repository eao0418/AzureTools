// Identity.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model.Resources
{
    using System.Collections.Generic;

    /// <summary>
    /// Identity for the resource.
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// The identity type.
        /// </summary>
        public string? Type { get; set; }
        
        /// <summary>
        /// The list of user identities associated with the resource.
        /// </summary>
        public Dictionary<string, UserAssignedIdentity> UserAssignedIdentities { get; set; } = new();
        
        /// <summary>
        /// The principal ID of resource identity.
        /// </summary>
        public string? PrincipalId { get; set; }
        
        /// <summary>
        /// The tenant ID of resource.
        /// </summary>
        public string? TenantId { get; set; }
    }

    /// <summary>
    /// User assigned identity for the resource.
    /// </summary>
    public class UserAssignedIdentity
    {
        /// <summary>
        /// The principal ID of the user assigned identity.
        /// </summary>
        public string? PrincipalId { get; set; }

        /// <summary>
        /// The client ID of the user assigned identity.
        /// </summary>
        public string? ClientId { get; set; }
    }
}
