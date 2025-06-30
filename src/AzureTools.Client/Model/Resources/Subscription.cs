// Subscription.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model.Resources
{
    using System.Collections.Generic;

    public class Subscription: ModelBase
    {
        /// <summary>
        /// The resource Id for the subscription.
        /// </summary>
        public string? Id { get; set; } = string.Empty;
        /// <summary>
        /// The unique identifier for the subscription.
        /// </summary>
        public string? SubscriptionId { get; set; } = string.Empty;
        /// <summary>
        /// The name of the subscription.
        /// </summary>
        public string? DisplayName { get; set; } = string.Empty;
        /// <summary>
        /// The subscription state. Possible values are Enabled, Warned, PastDue, Disabled, and Deleted.
        /// </summary>
        public string? State { get; set; } = string.Empty;
        /// <summary>
        /// Subscription policies.
        /// </summary>
        public Dictionary<string, string>? SubscriptionPolicies { get; set; } = new();
        /// <summary>
        /// How authorization is configured for the subscription.
        /// </summary>
        public string? AuthorizationSource { get; set; } = string.Empty;
        /// <summary>
        /// Information about a tenant managing the subscription.
        /// </summary>
        public List<KeyValuePair<string,string>>? ManagedByTenants { get; set; } = new ();
        /// <summary>
        /// Tags added to the subscritpoin.
        /// </summary>
        public Dictionary<string, string>? Tags { get; set; } = new ();

        public override string ToString()
        {
            return $"Subscription: {DisplayName} ({SubscriptionId}), State: {State}, Tenant: {TenantId}";
        }
    }
}
