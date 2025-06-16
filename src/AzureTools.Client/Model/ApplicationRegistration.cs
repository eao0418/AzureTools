// ApplicationRegistration.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using AzureTools.Client.Model.Application;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    public class ApplicationRegistration : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string AppId { get; set; } = string.Empty;
        public List<string> Owners { get; set; } = new ();
        public Dictionary<string, object> PublicClient { get; set; } = new ();
        public string SignInAudience { get; set; } = string.Empty;
        public List<string> IdentifierUris { get; set; } = new ();
        public List<string> RequiredResourceAccess { get; set; } = new ();
        public List<string> AppRoles { get; set; } = new ();
        public List<JsonDocument> KeyCredentials { get; set; } = new ();
        public List<PasswordCredential> PasswordCredentials { get; set; } = new ();
        public List<FederatedIdentityCredential> FederatedIdentityCredentials { get; set; } = new List<FederatedIdentityCredential>();
        public Dictionary<string,object> VerifiedPublisher { get; set; } = new();
        public DateTime CreatedDateTime { get; set; }
    }
}
