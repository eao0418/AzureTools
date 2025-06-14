// LocalFileRepository.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository
{
    using AzureTools.Repository.Model;
    using System.Text.Json;

    /// <summary>
    /// Object repository implementation that writes to a local file.
    /// </summary>
    public class LocalFileRepository : IObjectRepository
    {
        public LocalFileRepository() { }

        /// <inheritdoc/>
        public async Task WriteAsync<T>(IEnumerable<T> items)
            where T: DtoBase
        {
            if (items == null || !items.Any())
            {
                return;
            }

            var executionId = items.First().ExecutionId;

            var typeName = typeof(T).Name;
            var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var fileName = $"{executionId}-{typeName}-{timeStamp}.json";
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            var json = JsonSerializer.Serialize(
                items,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
