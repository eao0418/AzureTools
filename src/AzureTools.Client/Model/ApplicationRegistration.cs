// ApplicationRegistration.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;
    using System.Collections.Generic;

    internal class ApplicationRegistration : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public List<string> Owners { get; set; } = new List<string>();
        public bool PublicClient { get; set; }
        public string SignInAudience { get; set; } = string.Empty;
        public List<string> IdentifierUris { get; set; } = new List<string>();
        public List<string> RequiredResourceAccess { get; set; } = new List<string>();
        public List<string> AppRoles { get; set; } = new List<string>();
        public List<string> KeyCredentials { get; set; } = new List<string>();
        public List<string> PasswordCredentials { get; set; } = new List<string>();
        public List<FederatedIdentityCredential> FederatedIdentityCredentials { get; set; } = new List<FederatedIdentityCredential>();
        public string VerifiedPublisher { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; }
    }
}
