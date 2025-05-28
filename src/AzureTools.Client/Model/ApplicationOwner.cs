// ApplicationOwner.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;

    public class ApplicationOwner : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string UserPrincipalName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public DateTime CreatedDateTime { get; set; }
    }
}
