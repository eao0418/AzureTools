// User.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System.Collections.Generic;

    public class User: ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string UserPrincipalName { get; set; } = string.Empty;
        public bool AccountEnabled { get; set; }
        public SignInActivity? SignInActivity { get; set; } 
        public string PasswordPolicies { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public Dictionary<string, object>? CustomSecurityAttributes { get; set; }
        public List<string>? AssignedRoles { get; set; }
        public List<string>? AuthenticationMethods { get; set; }
        public int DeviceEnrollmentLimit { get; set; }
        public string ExternalUserState { get; set; } = string.Empty;
        public bool OnPremisesSyncEnabled { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string MobilePhone { get; set; } = string.Empty;
        public string OfficeLocation { get; set; } = string.Empty;
        public List<string>? BusinessPhones { get; set; }
        public List<Identity>? Identities { get; set; }
    }
}