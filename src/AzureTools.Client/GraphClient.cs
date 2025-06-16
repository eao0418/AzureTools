// GraphClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Authentication.Cache;
    using System;
    using System.Threading.Tasks;
    using System.Threading;
    using AzureTools.Client.Model;
    using AzureTools.Client.Model.Application;
    using System.Net.Http;
    using System.Net.Http.Json;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;

    public sealed class GraphClient : IGraphClient
    {
        private readonly ITokenCache _tokenCache;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GraphClient> _logger;
        private readonly JsonSerializerOptions _addPasswordSerializerOptions;

        public GraphClient(
            ITokenCache tokenCache,
            HttpClient httpClient,
            ILogger<GraphClient> logger)
        {
            _tokenCache = tokenCache ?? throw new ArgumentNullException(nameof(tokenCache));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient.BaseAddress = new Uri("https://graph.microsoft.com");

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            serializerOptions.Converters.Add(new Utility.Converter.EmptyStringToNullConverter());
            _addPasswordSerializerOptions = serializerOptions;
        }

        public async Task<ODataResponse<User>?> GetUsersAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation("Getting users from Graph API with execution ID: {ExecutionId} with endpoint {e}", executionId, Endpoints.UsersEndpoint);

            return await GetGraphObjectsAsync<User>(settings, Endpoints.UsersEndpoint, executionId, stopToken);
        }

        public async Task<ODataResponse<Group>?> GetGroupsAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            return await GetGraphObjectsAsync<Group>(settings, Endpoints.GroupsEndpoint, executionId, stopToken);
        }

        public async Task<ODataResponse<ServicePrincipal>?> GetServicePrincipalsAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            return await GetGraphObjectsAsync<ServicePrincipal>(settings, Endpoints.ServicePrincipalsEndpoint, executionId, stopToken);
        }

        public async Task<ODataResponse<ApplicationRegistration>?> GetApplicationRegistrationsAsync(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            return await GetGraphObjectsAsync<ApplicationRegistration>(settings, Endpoints.ApplicationRegistrationEndpoint, executionId, stopToken);
        }

        public async Task<ODataResponse<GroupMember>?> GetGroupMembershipAsync(string authSettingsKey, string tenantId, string groupId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var groupMembersEndpoint = Endpoints.GroupMembersEndpoint.Replace("{groupId}", groupId);

            var objectResponse = await GetODataResponseAsync<GroupMember>(groupMembersEndpoint, authSettingsKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
                val.GroupId = groupId;
            }

            return objectResponse;
        }

        public async Task<ODataResponse<GroupMember>?> GetGroupMembershipAsync(string url, string authSettingsKey, string tenantId, string groupId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));
            }

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var nextResponse = await GetODataResponseAsync<GroupMember>(url, authSettingsKey, stopToken);

            if (nextResponse == null || nextResponse.Value == null)
            {
                return nextResponse;
            }

            foreach (var nextVal in nextResponse.Value)
            {
                nextVal.ExecutionId = executionId;
                nextVal.TenantId = tenantId;
                nextVal.GroupId = groupId;
            }

            return nextResponse;
        }

        public async Task<ODataResponse<ApplicationOwner>?> GetApplicationRegistrationOwnersAsync(string authSettingsKey, string tenantId, string appId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var appOwnerEndpoint = Endpoints.ApplicationOwnersEndpoint.Replace("{applicationId}", appId);

            var objectResponse = await GetODataResponseAsync<ApplicationOwner>(appOwnerEndpoint, authSettingsKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
                val.ApplicationId = appId;
            }

            return objectResponse;
        }

        public async Task<ODataResponse<ApplicationOwner>?> GetApplicationRegistrationOwnersAsync(string url, string authSettingsKey, string tenantId, string appId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var nextResponse = await GetODataResponseAsync<ApplicationOwner>(url, authSettingsKey, stopToken);

            if (nextResponse == null || nextResponse.Value == null)
            {
                return nextResponse;
            }

            foreach (var nextVal in nextResponse.Value)
            {
                nextVal.ExecutionId = executionId;
                nextVal.TenantId = tenantId;
                nextVal.ApplicationId = appId;
            }

            return nextResponse;
        }

        public async Task<ODataResponse<DirectoryRole>?> GetEntraDirectoryRoles(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var roles = await GetGraphObjectsAsync<DirectoryRole>(settings, Endpoints.DirectoryRolesEndpoint, executionId, stopToken);

            if (roles == null || roles.Value == null)
            {
                return roles;
            }

            foreach (var val in roles.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = settings.TenantId;
            }

            return roles;
        }

        public async Task<ODataResponse<T>?> GetGraphObjectsAsync<T>(AuthenticationSettings settings, string endpoint, string executionId, CancellationToken stopToken)
            where T : ModelBase
        {
            var objectResponse = await GetODataResponseAsync<T>(endpoint, settings, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = settings.TenantId;
            }

            return objectResponse;
        }

        public async Task<ODataResponse<T>?> GetGraphObjectsAsync<T>(string url, string settingKey, string tenantId, string executionId, CancellationToken stopToken)
            where T : ModelBase
        {
            var objectResponse = await GetODataResponseAsync<T>(url, settingKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return objectResponse;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
            }

            return objectResponse;
        }

        public async Task<PasswordCredential?> AddApplicationPasswordAsync(
            string id,
            UpdatePasswordRequest updatePasswordRequest,
            AuthenticationSettings authSettings,
            CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Application ID cannot be null or empty.", nameof(id));
            }
            if (updatePasswordRequest == null)
            {
                throw new ArgumentNullException(nameof(updatePasswordRequest));
            }

            var url = $"/v1.0/applications/{id}/addPassword";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetOrAddTokenAsync(authSettings, stopToken));
            
            var response = await _httpClient.PostAsJsonAsync(url, updatePasswordRequest, _addPasswordSerializerOptions, stopToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to add application password. Status code: {StatusCode}, Content: {Content}", response.StatusCode, content);
                throw new HttpRequestException($"Failed to add application password. Status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<PasswordCredential>(_addPasswordSerializerOptions, stopToken);
        }

        private async Task<ODataResponse<T>> GetODataResponseAsync<T>(string url, AuthenticationSettings settings, CancellationToken stopToken = default)
            where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetOrAddTokenAsync(settings, stopToken));
            ODataResponse<T> response;
            try
            {
                var httpResponse = await _httpClient.GetAsync(url, stopToken);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync(stopToken);
                    _logger.LogError("Failed to retrieve OData response from {Url}. Status code: {StatusCode}", url, httpResponse.StatusCode);
                    _logger.LogError("Response content: {Content}", content);
                    throw new HttpRequestException($"Request to {url} failed with status code {httpResponse.StatusCode}");
                }
                response = await httpResponse.Content.ReadFromJsonAsync<ODataResponse<T>>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, stopToken)
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

        private async Task<ODataResponse<T>> GetODataResponseAsync<T>(string url, string authSettingsKey, CancellationToken stopToken = default)
            where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetTokenAsync(authSettingsKey, stopToken));

            var response = await _httpClient.GetFromJsonAsync<ODataResponse<T>>(url, stopToken);

            if (response == null)
            {
                throw new InvalidOperationException("Failed to retrieve OData response.");
            }

            return response;
        }
    }
}
