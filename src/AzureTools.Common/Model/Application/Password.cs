// Password.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Application
{
    using System;

    public class UpdatePasswordRequest
    {
        public PasswordCredential? PasswordCredential { get; set; }
    }

    public class PasswordCredential
    {
        public string CustomKeyIdentifier { get; set; } = string.Empty;
        public DateTime? EndDateTime { get; set; }
        public string KeyId { get; set; } = string.Empty;
        public DateTime? StartDateTime { get; set; }
        public string SecretText { get; set; } = string.Empty;
        public string Hint { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;
    }
}
