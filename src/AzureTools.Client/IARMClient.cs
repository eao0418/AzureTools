// IARMClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Client.Model;
    using AzureTools.Client.Model.Resources;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IARMClient
    {
        Task<ARMResponse<T>?> GetARMObjectsAsync<T>(AuthenticationSettings settings, string endpoint, string executionId, CancellationToken stopToken) where T : ModelBase;
        Task<ARMResponse<T>?> GetARMObjectsAsync<T>(string url, string settingKey, string tenantId, string executionId, CancellationToken stopToken) where T : ModelBase;
        Task<Provider?> GetProviderApiVersionAsync(AuthenticationSettings settings, string subscriptionId, string resourceProvider, string? executionId = null, CancellationToken stopToken = default);
        Task<Provider?> GetProviderApiVersionAsync(string settingskey, string subscriptionId, string resourceProvider, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Resource>?> GetResourcePropertiesAsync(AuthenticationSettings settings, string resourceId, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Resource>?> GetResourcePropertiesAsync(string settingsKey, string resourceId, string tenantId, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Resource>?> GetResourcesForSubscriptionAsync(AuthenticationSettings settings, string subscriptionId, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Resource>?> GetResourcesForSubscriptionAsync(string settingsKey, string subscriptionId, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Subscription>?> GetSubscriptionsAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
        Task<ARMResponse<Tenant>?> GetTenantsAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
    }
}