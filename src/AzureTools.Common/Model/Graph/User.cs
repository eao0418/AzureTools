// User.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Graph
{
    using System.Collections.Generic;
    using System.Data;

    public class User : ModelBase
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

        public static User CreateFromReader(IDataReader reader)
        {
            var user = new User
            {
                Id = reader["Id"] as string ?? string.Empty,
                UserPrincipalName = reader["UserPrincipalName"] as string ?? string.Empty,
                AccountEnabled = reader.GetBoolean(reader.GetOrdinal("AccountEnabled")),
                PasswordPolicies = reader["PasswordPolicies"] as string ?? string.Empty,
                RiskLevel = reader["RiskLevel"] as string ?? string.Empty,
                DeviceEnrollmentLimit = reader.GetInt32(reader.GetOrdinal("DeviceEnrollmentLimit")),
                ExternalUserState = reader["ExternalUserState"] as string ?? string.Empty,
                OnPremisesSyncEnabled = reader.GetBoolean(reader.GetOrdinal("OnPremisesSyncEnabled")),
                Mail = reader["Mail"] as string ?? string.Empty,
                DisplayName = reader["DisplayName"] as string ?? string.Empty,
                GivenName = reader["GivenName"] as string ?? string.Empty,
                Surname = reader["Surname"] as string ?? string.Empty,
                JobTitle = reader["JobTitle"] as string ?? string.Empty,
                MobilePhone = reader["MobilePhone"] as string ?? string.Empty,
                OfficeLocation = reader["OfficeLocation"] as string ?? string.Empty
            };
            if (reader["SignInActivity"] is not DBNull)
            {
                user.SignInActivity = reader["SignInActivity"] as SignInActivity;
            }
            
            return user;
        }
    }
}