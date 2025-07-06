// Provider.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Client.Model.Resources
{
    using System.Collections.Generic;

    public sealed class Provider: ModelBase
    {
        /// <summary>
        /// The Id of the Resource Provider.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The name of the Resource Provider.
        /// </summary>
        public string? Namespace { get; set; }

        /// <summary>
        /// The list of authorizations granted to this provider.
        /// </summary>
        public List<Dictionary<string, string>>? Authorizations { get; set; }

        /// <summary>
        /// The resource types supported by this provider.
        /// </summary>
        public List<ProviderResourceType>? ResourceTypes { get; set; }

        /// <summary>
        /// The registration state of the resource provider.
        /// </summary>
        public string? RegistrationState { get; set; }

        /// <summary>
        /// The registration policy of the resource provider.
        /// </summary>
        public string? RegistrationPolicy { get; set; }
    }
}
