// Endpoints.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    /// <summary>
    /// Defines endpoints for Microsoft Graph API interactions.
    /// </summary>
    public static class Endpoints
    {
        private const string BetaVersion = "beta";
        private const string Version = "v1.0";

        /// <summary>
        /// Base endpoint for Microsoft Graph API users.
        /// </summary>
        public static string UsersEndpointBase = $"/{BetaVersion}/users?$select=";

        /// <summary>
        /// Endpoint for Microsoft Graph API users with specific fields selected.
        /// </summary>
        public static string UsersEndpoint => $"{UsersEndpointBase}id,userPrincipalName,accountEnabled,riskLevel,assignedRoles,authenticationMethods,mail,displayName,givenName,surname,jobTitle,mobilePhone,officeLocation,businessPhones";

        /// <summary>
        /// Base endpoint for Microsoft Graph API service principals.
        /// </summary>
        //public static string ServicePrincipalsEndpointBase => $"/{BetaVersion}/servicePrincipals?$select=";
        public static string ServicePrincipalsEndpointBase => $"/{BetaVersion}/servicePrincipals";

        /// <summary>
        /// Endpoint for Microsoft Graph API service principals with specific fields selected.
        /// </summary>
        //public static string ServicePrincipalsEndpoint => $"{ServicePrincipalsEndpointBase}=id,displayName,appId,applicationType,servicePrincipalType,appRoles,owners,delegatedPermissions,accountEnabled,passwordCredentialsPresent,keyCredentialsPresent,managedIdentities,isOauth2PermissionGrantRestricted,isAssignRoleRestricted,isAppRoleAssignmentRequired,createdDateTime,renewedDateTime&$expand=owners($select=id)";
        public static string ServicePrincipalsEndpoint => $"{ServicePrincipalsEndpointBase}";

        /// <summary>
        /// Base endpoint for Microsoft Graph API groups.
        /// </summary>
        //public static string GroupsEndpointBase => $"/{BetaVersion}/groups?$select=";
        public static string GroupsEndpointBase => $"/{BetaVersion}/groups";

        /// <summary>
        /// Endpoint for Microsoft Graph API groups with specific fields selected.
        /// </summary>
        //public static string GroupsEndpoint => $"{GroupsEndpointBase}id,displayName,securityEnabled,isAssignableToRole,groupTypes,visibility,mailEnabled,owners,members,membershipRule,membershipRuleProcessingState,assignedLicenses,allowExternalSenders,autoSubscribeNewMembers,onPremisesSyncEnabled,createdDateTime,renewedDateTime,resourceBehaviorOptions,preferredDataLocation";
        public static string GroupsEndpoint => $"{GroupsEndpointBase}";

        /// <summary>
        /// Base endpoint for Microsoft Graph API group members.
        /// </summary>
        public static string GroupMembersEndpointBase => $"/{BetaVersion}/groups/{{groupId}}/members";
        //public static string GroupMembersEndpointBase => $"/{BetaVersion}/groups/{{groupId}}/members?$select=";

        /// <summary>
        /// Endpoint for Microsoft Graph API group members with specific fields selected.
        /// </summary>
        public static string GroupMembersEndpoint => $"{GroupMembersEndpointBase}";
        //public static string GroupMembersEndpoint => $"{GroupMembersEndpointBase}id,displayName,mail,userPrincipalName,accountEnabled,createdDateTime,signInActivity,@odata.type";

        /// <summary>
        /// Base endpoint for Microsoft Graph API applications.
        /// </summary>
        public static string ApplicationsEndpointBase => $"/{Version}/applications?$select=";

        /// <summary>
        /// Endpoint for Microsoft Graph API applications with specific fields selected.
        /// </summary>
        public static string ApplicationRegistrationEndpoint => $"{ApplicationsEndpointBase}id,displayName,appId,owners,publicClient,createdDateTime,signInAudience,identifierUris,appRoles,keyCredentials,passwordCredentials,verifiedPublisher,federatedIdentityCredentials";

        /// <summary>
        /// Base endpoint for Microsoft Graph API directory roles.
        /// </summary>
        //public static string DirectoryRolesEndpointBase => $"/{Version}/directoryRoles?$select=";
        public static string DirectoryRolesEndpointBase => $"/{Version}/directoryRoles";

        /// <summary>
        /// Endpoint for Microsoft Graph API directory roles with specific fields selected.
        /// </summary>
        //public static string DirectoryRolesEndpoint => $"{DirectoryRolesEndpointBase}id,displayName,roleTemplateId,description,members,createdDateTime";
        public static string DirectoryRolesEndpoint => $"{DirectoryRolesEndpointBase}";

        /// <summary>
        /// Base endpoint for Microsoft Graph API application owners.
        /// </summary>
        //public static string ApplicationOwnersEndpointBase => $"/{Version}/applications/{{applicationId}}/owners?$select=";
        public static string ApplicationOwnersEndpointBase => $"/{Version}/applications/{{applicationId}}/owners";

        /// <summary>
        /// Endpoint for Microsoft Graph API application owners with specific fields selected.
        /// </summary>
        //public static string ApplicationOwnersEndpoint => $"{ApplicationOwnersEndpointBase}id,displayName,userPrincipalName,mail,createdDateTime";
        public static string ApplicationOwnersEndpoint => $"{ApplicationOwnersEndpointBase}";
    }
}

