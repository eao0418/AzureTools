// ARMResponse.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Resources
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ARMResponse<T>
        where T : class
    {
        [JsonPropertyName("nextLink")]
        public string? NextLink { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public List<T> Value { get; set; } = new List<T>();
    }
}