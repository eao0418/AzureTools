// Plan.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Common.Model.Resources
{
    /// <summary>
    /// Plan for the resource.
    /// </summary>
    public class Plan
    {
        /// <summary>
        /// The plan Id.
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// The publisher Id.
        /// </summary>
        public string? Publisher { get; set; }

        /// <summary>
        /// The offer Id.
        /// </summary>
        public string? Product { get; set; }

        /// <summary>
        /// The promotion code.
        /// </summary>
        public string? PromotionCode { get; set; }

        /// <summary>
        /// The plan's version.
        /// </summary>
        public string? Version { get; set; }
    }
}
