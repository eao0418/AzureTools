// ODataResponse.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AzureTools.Client.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ODataResponse<T>
        where T : class
    {
        [JsonPropertyName("@odata.context")]
        public string ODataContext { get; set; } = string.Empty;
        [JsonPropertyName("@odata.nextLink")]
        public string ODataNextLink { get; set; } = string.Empty;
        [JsonPropertyName("value")]
        public List<T> Value { get; set; } = new List<T>();
    }
}
