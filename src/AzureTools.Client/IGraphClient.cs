// IGraphClient.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Client.Model;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGraphClient
    {
        Task<ODataResponse<ApplicationOwner>?> GetApplicationRegistrationOwnersAsync(string authSettingsKey, string tenantId, string appId, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<ApplicationOwner>?> GetApplicationRegistrationOwnersAsync(string url, string authSettingsKey, string tenantId, string appId, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<ApplicationRegistration>?> GetApplicationRegistrationsAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<DirectoryRole>?> GetEntraDirectoryRoles(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<T>?> GetGraphObjectsAsync<T>(AuthenticationSettings settings, string endpoint, string executionId, CancellationToken stopToken) where T : ModelBase;
        Task<ODataResponse<T>?> GetGraphObjectsAsync<T>(string url, string settingKey, string tenantId, string executionId, CancellationToken stopToken) where T : ModelBase;
        Task<ODataResponse<GroupMember>?> GetGroupMembershipAsync(string authSettingsKey, string tenantId, string groupId, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<GroupMember>?> GetGroupMembershipAsync(string url, string authSettingsKey, string tenantId, string groupId, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<Group>?> GetGroupsAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<ServicePrincipal>?> GetServicePrincipalsAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
        Task<ODataResponse<User>?> GetUsersAsync(AuthenticationSettings settings, string? executionId = null, CancellationToken stopToken = default);
    }
}