// ARMEndpoints.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    public static class ARMEndpoints
    {
        private const string ResourceManagementVersion = "2022-12-01";
        private const string ResourcesVersion = "2025-04-01";
        private const string GetResourceAPIVersion = "2021-04-01";

        /// <summary>
        /// Gets the tenants for your account.
        /// </summary>
        public static string ListTenantsEndpoint => $"/tenants?api-version={ResourceManagementVersion}";

        /// <summary>
        /// Gets all subscriptions for a tenant.
        /// </summary>
        public static string ListSubscriptionsEndpoint => $"/subscriptions?api-version=2016-06-01";

        public static string ResourcesApiVersionParameter => $"?api-version={ResourcesVersion}";

        /// <summary>
        /// Gets the details of a specific subscription.
        /// </summary>
        public static string ListResourcesForSubscription => $"/subscriptions/{{subscriptionId}}/resources?api-version={ResourcesVersion}&$expand=createdTime,changedTime";

        public static string ListApiVersionForResourceProvider = $"/subscriptions/{{subscriptionId}}/providers/{{resourceProviderNamespace}}?api-version={GetResourceAPIVersion}";

    }
}
