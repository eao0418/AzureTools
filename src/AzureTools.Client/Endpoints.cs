// Endpoints.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client
{
    public static class Endpoints
    {
        private const string BetaVersion = "beta";
        private const string Version = "v1.0";
        public static string UsersEndpoint => $"/{BetaVersion}/users?top=150&select=id,userPrincipalName,accountEnabled,signInActivity,passwordPolicies,riskLevel,customSecurityAttributes,assignedRoles,authenticationMethods,deviceEnrollmentLimit,externalUserState,onPremisesSyncEnabled,mail,displayName,givenName,surname,jobTitle,mobilePhone,officeLocation,businessPhones,identities$expand=manager($select=id)";
        public static string ServicePrincipalsEndpoint => $"/{BetaVersion}/servicePrincipals?top=150&select=id,displayName,appId,applicationType,servicePrincipalType,appRoles,owners,delegatedPermissions,accountEnabled,passwordCredentialsPresent,keyCredentialsPresent,managedIdentities,isOauth2PermissionGrantRestricted,isAssignRoleRestricted,isAppRoleAssignmentRequired,createdDateTime,renewedDateTime&$expand=owners($select=id)";
        public static string GroupsEndpoint => $"/{Version}/groups?top=150&$select=id,displayName,securityEnabled,isAssignableToRole,groupTypes,visibility,mailEnabled,owners,members,membershipRule,membershipRuleProcessingState,assignedLicenses,allowExternalSenders,autoSubscribeNewMembers,onPremisesSyncEnabled,createdDateTime,renewedDateTime,resourceBehaviorOptions,preferredDataLocation";
        public static string GroupMembersEndpoint => $"/{Version}/groups/{{groupId}}/members?$select=id,displayName,mail,userPrincipalName,accountEnabled,createdDateTime,signInActivity,@odata.type";
        public static string ApplicationRegistrationEndpoint => $"/{Version}/applications?top=150&$select=id,displayName,appId,owners,publicClient,createdDateTime,signInAudience,identifierUris,requiredResourceAccess,appRoles,keyCredentials,passwordCredentials,verifiedPublisher,federatedIdentityCredentials";
        public static string DirectoryRolesEndpoint => $"/{Version}/directoryRoles?$select=id,displayName,roleTemplateId,description,members,createdDateTime";
        public static string ApplicationOwnersEndpoint => $"/{Version}/applications/{{applicationId}}/owners?$select=id,displayName,userPrincipalName,mail,createdDateTime";
    }
}

