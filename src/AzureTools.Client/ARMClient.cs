// ARMClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Cache;
    using AzureTools.Authentication.Settings;
    using AzureTools.Client.Model;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Threading;
    using AzureTools.Client.Model.Resources;
    using System.Collections.Generic;

    public class ARMClient
    {
        private readonly ILogger<ARMClient> _logger;
        private readonly ITokenCache _tokenCache;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ARMClient(
            ITokenCache tokenCache,
            HttpClient httpClient,
            ILogger<ARMClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenCache = tokenCache ?? throw new ArgumentNullException(nameof(tokenCache));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://management.azure.com");

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = SerializableContext.Default,
            };
        }

        /// <summary>
        /// Retrieves a list of tenants from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<Tenant>?> GetTenantsAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation("Getting tenants from the ARM API with execution ID: {ExecutionId} with endpoint {e}", executionId, ARMEndpoints.ListTenantsEndpoint);

            return await GetARMObjectsAsync<Tenant>(settings, ARMEndpoints.ListTenantsEndpoint, executionId, stopToken);
        }

        /// <summary>
        /// Retrieves a list of subscriptions from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<Subscription>?> GetSubscriptionsAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation("Getting subscriptions from the ARM API with execution ID: {ExecutionId} with endpoint {e}", executionId, ARMEndpoints.ListSubscriptionsEndpoint);

            return await GetARMObjectsAsync<Subscription>(settings, ARMEndpoints.ListSubscriptionsEndpoint, executionId, stopToken);
        }

        /// <summary>
        /// Retrieves a list of subscriptions from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="subscriptionId"> The subscription ID to use for the request.</param>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<Resource>?> GetResourcesForSubscriptionAsync(AuthenticationSettings settings, string subscriptionId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new ArgumentException("Subscription ID cannot be null or empty.", nameof(subscriptionId));
            }
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var endpoint = ARMEndpoints.ListResourcesForSubscription.Replace("{subscriptionId}", subscriptionId);

            _logger.LogInformation("Getting resources from the ARM API with execution ID: {ExecutionId} with endpoint {e} from subscription {s}", executionId, endpoint, subscriptionId);

            return await GetARMObjectsAsync<Resource>(settings, endpoint, executionId, stopToken);
        }

        /// <summary>
        /// Retrieves a list of subscriptions from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="resourceId"> The resource ID to retrieve properties for.</param>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<Resource>?> GetResourcePropertiesAsync(AuthenticationSettings settings, string resourceId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                throw new ArgumentException("resource ID cannot be null or empty.", nameof(resourceId));
            }
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var endpoint = $"{resourceId}{ARMEndpoints.ResourcesApiVersionParameter}";

            _logger.LogInformation("Getting resource properties from the ARM API with execution ID: {ExecutionId} from resource {s}", executionId, endpoint);

            var resource = await GetARMResponseAsync<Resource>(endpoint, settings, stopToken);

            if (resource != null)
            {
                resource.ExecutionId = executionId;
                resource.TenantId = settings.TenantId;
            }

            // Create a response object to return the resource
            // The API only returns one resource for a GET request to a specific resource ID.
            // Create the Response object for consistency and to make life easier down the road.
            // The small amount of overhead is worth the consistency.
            var response = new ARMResponse<Resource>
            {
                Value = resource != null ? new List<Resource>() { resource } : new(),
            };

            return response;
        }

        /// <summary>
        /// Retrieves a list of subscriptions from the Azure Management API using the provided authentication settings and execution ID.
        /// Requires a previous authentication for the tenant. The token should be cached under the provided settings key.
        /// </summary>
        /// <param name="settingsKey">The authentication settings key to look up the token from the cache.</param>
        /// <param name="resourceId"> The resource ID to retrieve properties for.</param>
        /// <param name="tenantId"> The tenant ID for the request.</param>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<Resource>?> GetResourcePropertiesAsync(string settingsKey, string resourceId, string tenantId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                throw new ArgumentException("resource ID cannot be null or empty.", nameof(resourceId));
            }
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var endpoint = $"{resourceId}{ARMEndpoints.ResourcesApiVersionParameter}";

            _logger.LogInformation("Getting resource properties from the ARM API with execution ID: {ExecutionId} from resource {s}", executionId, endpoint);

            var resource = await GetARMResponseAsync<Resource>(endpoint, settingsKey, stopToken);

            if (resource != null)
            {
                resource.ExecutionId = executionId;
                resource.TenantId = tenantId;
            }

            // Create a response object to return the resource
            // The API only returns one resource for a GET request to a specific resource ID.
            // Create the Response object for consistency and to make life easier down the road.
            // The small amount of overhead is worth the consistency.
            var response = new ARMResponse<Resource>
            {
                Value = resource != null ? new List<Resource> () { resource } : new(),
            };

            return response;
        }

        /// <summary>
        /// Retrieves ARM objects from the specified endpoint using the provided authentication settings and execution ID.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="settings">The <see cref="AuthenticationSettings"/>.</param>
        /// <param name="endpoint">The endpoint for the request.</param>
        /// <param name="executionId">The unique identifier for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<T>?> GetARMObjectsAsync<T>(AuthenticationSettings settings, string endpoint, string executionId, CancellationToken stopToken)
            where T : ModelBase
        {
            var objectResponse = await GetARMResponseAsync<ARMResponse<T>>(endpoint, settings, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            if (typeof(T) == typeof(Tenant))
            {
                // If the type is Tenant, we don't set TenantId, because this should be deserialized from the response already.
                foreach (var val in objectResponse.Value)
                {
                    val.ExecutionId = executionId;
                }
                return objectResponse;
            }
            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = settings.TenantId;
            }

            return objectResponse;
        }

        /// <summary>
        /// Retrieves ARM objects from the specified URL using the provided authentication settings key, tenant ID, and execution ID.
        /// This method requires a previous authentication token to be cached under the provided settings key.
        /// </summary>
        /// <typeparam name="T">The type of object to request.</typeparam>
        /// <param name="url">The URI for the request.</param>
        /// <param name="settingKey">The authentication settings key to look up the token from the cache.</param>
        /// <param name="tenantId"> The tenant ID for the request.</param>
        /// <param name="executionId">The unique identifier for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task<ARMResponse<T>?> GetARMObjectsAsync<T>(string url, string settingKey, string tenantId, string executionId, CancellationToken stopToken)
            where T : ModelBase
        {
            var objectResponse = await GetARMResponseAsync<ARMResponse<T>>(url, settingKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            if (typeof(T) == typeof(Tenant))
            {
                // If the type is Tenant, we don't set TenantId, because this should be deserialized from the response already.
                foreach (var val in objectResponse.Value)
                {
                    val.ExecutionId = executionId;
                }
                return objectResponse;
            }
            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
            }

            return objectResponse;
        }

        /// <summary>
        /// Retrieves an ARM response from the specified URL using the provided authentication settings.
        /// </summary>
        /// <typeparam name="T">The type to convert the response to.</typeparam>
        /// <param name="url">The URI for the request.</param>
        /// <param name="settings">The <see cref="AuthenticationSettings"/> to use in the token request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<T> GetARMResponseAsync<T>(string url, AuthenticationSettings settings, CancellationToken stopToken = default)
            where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetOrAddTokenAsync(settings, stopToken));
            T response;
            try
            {
                var httpResponse = await _httpClient.GetAsync(url, stopToken);

                string content;

                if (!httpResponse.IsSuccessStatusCode)
                {
                    content = await httpResponse.Content.ReadAsStringAsync(stopToken);
                    _logger.LogError("Failed to retrieve OData response from {Url}. Status code: {StatusCode}", url, httpResponse.StatusCode);
                    _logger.LogError("Response content: {Content}", content);
                    throw new HttpRequestException($"Request to {url} failed with status code {httpResponse.StatusCode}");
                }

                _logger.LogInformation($"status code: {httpResponse.StatusCode}");
                content = await httpResponse.Content.ReadAsStringAsync(stopToken);
                _logger.LogInformation("Response content: {Content}", content);

                response = await httpResponse.Content.ReadFromJsonAsync<T>(_jsonSerializerOptions, stopToken)
                           ?? throw new InvalidOperationException("Failed to deserialize OData response.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving OData response from {Url}", url);
                throw;
            }

            if (response == null)
            {
                throw new InvalidOperationException("Failed to retrieve OData response.");
            }

            return response;
        }

        /// <summary>
        /// Retrieves an ARM response from the specified URL using the provided authentication settings key.
        /// </summary>
        /// <typeparam name="T">The type for the request.</typeparam>
        /// <param name="url">The URI for the request.</param>
        /// <param name="authSettingsKey">The settings key representing the cached token.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        /// <exception cref="InvalidOperationException">If the request is not successful.</exception>
        private async Task<T> GetARMResponseAsync<T>(string url, string authSettingsKey, CancellationToken stopToken = default)
            where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetTokenAsync(authSettingsKey, stopToken));

            var response = await _httpClient.GetFromJsonAsync<T>(url, _jsonSerializerOptions, stopToken);

            if (response == null)
            {
                throw new InvalidOperationException("Failed to retrieve ARM response.");
            }

            return response;
        }
    }
}
