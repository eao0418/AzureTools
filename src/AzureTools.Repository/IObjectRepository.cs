// IObjectRepository.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for an object repository.
    /// </summary>
    public interface IObjectRepository
    {
        /// <summary>
        /// Writes a collection of items to a repository.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="items">The collection of items.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WriteAsync<T>(IEnumerable<T> items);
    }
}