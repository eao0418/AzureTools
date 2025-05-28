// GraphClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Authentication.Cache;
    using System;
    using System.Threading.Tasks;
    using System.Threading;
    using AzureTools.Repository;
    using AzureTools.Client.Model;
    using System.Net.Http;
    using System.Net.Http.Json;

    public class GraphClient
    {
        private readonly ITokenCache _tokenCache;
        private readonly IObjectRepository _objectRepository;
        private readonly HttpClient _httpClient;

        public GraphClient(
            ITokenCache graphServiceClientFactory,
            IObjectRepository objectRepository,
            HttpClient httpClient)
        {
            _tokenCache = graphServiceClientFactory ?? throw new ArgumentNullException(nameof(graphServiceClientFactory));
            _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task GetUsers(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }
            
            await GetAndEnumerateObjects<User>(settings, Endpoints.UsersEndpoint, executionId, stopToken);
        }

        public async Task GetGroups(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            await GetAndEnumerateObjects<Group>(settings, Endpoints.GroupsEndpoint, executionId, stopToken);
        }

        public async Task GetServicePrincipals(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            await GetAndEnumerateObjects<ServicePrincipal>(settings, Endpoints.ServicePrincipalsEndpoint, executionId, stopToken);
        }

        public async Task GetApplicationRegistrations(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            await GetAndEnumerateObjects<ApplicationRegistration>(settings, Endpoints.ApplicationRegistrationEndpoint, executionId, stopToken);
        }

        public async Task GetGroupMembershipAsync(string authSettingsKey, string tenantId, string groupId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var groupMembersEndpoint = Endpoints.GroupMembersEndpoint.Replace("{groupId}", groupId);

            var objectResponse = await GetODataResponseAsync<GroupMember>(groupMembersEndpoint, authSettingsKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
                val.GroupId = groupId;
            }

            await _objectRepository.WriteAsync<GroupMember>(objectResponse.Value);

            var nextLink = objectResponse.ODataNextLink;

            while (string.IsNullOrWhiteSpace(nextLink) is false)
            {
                var nextResponse = await GetODataResponseAsync<GroupMember>(nextLink, authSettingsKey, stopToken);

                if (nextResponse == null || nextResponse.Value == null)
                {
                    break;
                }

                foreach (var nextVal in nextResponse.Value)
                {
                    nextVal.ExecutionId = executionId;
                    nextVal.TenantId = tenantId;
                    nextVal.GroupId = groupId;
                }

                await _objectRepository.WriteAsync<GroupMember>(nextResponse.Value);
                nextLink = nextResponse.ODataNextLink;
            }
        }

        public async Task GetApplicationRegistrationOwnersAsync(string authSettingsKey, string tenantId, string appId, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            var appMembersEndpoint = Endpoints.ApplicationOwnersEndpoint.Replace("{applicationId}", appId);

            var objectResponse = await GetODataResponseAsync<ApplicationOwner>(appMembersEndpoint, authSettingsKey, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = tenantId;
                val.ApplicationId = appId;
            }

            await _objectRepository.WriteAsync<ApplicationOwner>(objectResponse.Value);

            var nextLink = objectResponse.ODataNextLink;

            while (string.IsNullOrWhiteSpace(nextLink) is false)
            {
                var nextResponse = await GetODataResponseAsync<ApplicationOwner>(nextLink, authSettingsKey, stopToken);

                if (nextResponse == null || nextResponse.Value == null)
                {
                    break;
                }

                foreach (var nextVal in nextResponse.Value)
                {
                    nextVal.ExecutionId = executionId;
                    nextVal.TenantId = tenantId;
                    nextVal.ApplicationId = appId;
                }

                await _objectRepository.WriteAsync<ApplicationOwner>(nextResponse.Value);
                nextLink = nextResponse.ODataNextLink;
            }
        }

        public async Task GetEntraDirectoryRoles(AuthenticationSettings settings, string? executionId = default, CancellationToken stopToken = default)
        {
            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            await GetAndEnumerateObjects<DirectoryRole>(settings, Endpoints.DirectoryRolesEndpoint, executionId, stopToken);
        }

        public async Task GetAndEnumerateObjects<T>(AuthenticationSettings settings, string endpoint, string executionId, CancellationToken stopToken)
            where T: ModelBase
        {
            var objectResponse = await GetODataResponseAsync<T>(Endpoints.UsersEndpoint, settings, stopToken);

            if (objectResponse == null || objectResponse.Value == null)
            {
                return;
            }

            foreach (var val in objectResponse.Value)
            {
                val.ExecutionId = executionId;
                val.TenantId = settings.TenantId;
            }

            await _objectRepository.WriteAsync<T>(objectResponse.Value);

            var nextLink = objectResponse.ODataNextLink;
            var settingsKey = settings.GetAuthKey();

            while (string.IsNullOrWhiteSpace(nextLink) is false)
            {
                var nextResponse = await GetODataResponseAsync<T>(nextLink, settingsKey, stopToken);

                if (nextResponse == null || nextResponse.Value == null)
                {
                    break;
                }

                foreach (var nextVal in nextResponse.Value)
                {
                    nextVal.ExecutionId = executionId;
                    nextVal.TenantId = settings.TenantId;
                }

                await _objectRepository.WriteAsync<T>(nextResponse.Value);
                nextLink = nextResponse.ODataNextLink;
            }
        }

        public async Task<ODataResponse<T>> GetODataResponseAsync<T>(string url, AuthenticationSettings settings, CancellationToken stopToken = default)
            where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _tokenCache.GetOrAddTokenAsync(settings, stopToken));
            
            var response = await _httpClient.GetFromJsonAsync<ODataResponse<T>>(url, stopToken);
            
            if (response == null)
            {
                throw new InvalidOperationException("Failed to retrieve OData response.");
            }
            
            return response;
        }

        public async Task<ODataResponse<T>> GetODataResponseAsync<T>(string url, string authSettingsKey, CancellationToken stopToken = default)
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
