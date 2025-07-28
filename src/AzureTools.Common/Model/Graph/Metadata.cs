// Metadata.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Graph
{
    using System;
    using System.Collections.Generic;

    public record Identity
    {
        public string SignInType { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string IssuerAssignedId { get; init; } = string.Empty;
    }

    public record FederatedIdentityCredential
    {
        public string Id { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> Audiences { get; set; } = [];
    }

    public record SignInActivity
    {
        public DateTimeOffset? LastNonInteractiveSignInDateTime { get; init; }

        public string LastNonInteractiveSignInRequestId { get; init; } = string.Empty;

        public DateTimeOffset? LastSignInDateTime { get; init; }

        public string LastSignInRequestId { get; init; } = string.Empty;

        public DateTimeOffset? LastSuccessfulSignInDateTime { get; init; }

        public string LastSuccessfulSignInRequestId { get; init; } = string.Empty;
    }
}
