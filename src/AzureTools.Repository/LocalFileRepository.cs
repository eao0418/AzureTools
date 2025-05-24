// LocalFileRepository.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository
{
    /// <summary>
    /// Object repository implementation that writes to a local file.
    /// </summary>
    public class LocalFileRepository : IObjectRepository
    {
        public LocalFileRepository() { }

        /// <inheritdoc/>
        public async Task WriteAsync<T>(IEnumerable<T> items)
        {
            if (items == null || !items.Any())
            {
                return;
            }

            var typeName = typeof(T).Name;
            var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var fileName = $"{typeName}-{timeStamp}.txt";
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            using (var writer = new StreamWriter(filePath))
            {
                foreach (var item in items)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    await writer.WriteLineAsync(item.ToString());
                }
            }
        }
    }
}
