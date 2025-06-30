// DirectoryRole.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model
{
    using System;
    using System.Collections.Generic;

    public class DirectoryRole : ModelBase
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string RoleTemplateId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Members { get; set; } = [];
        public DateTime CreatedDateTime { get; set; }
    }
}
