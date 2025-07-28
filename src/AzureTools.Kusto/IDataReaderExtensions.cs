// IDataReaderExtensions.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class IDataReaderExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IDataReader reader, Func<IDataReader, T> mapper) where T: class, new()
        {
            ArgumentNullException.ThrowIfNull(reader);
            var result = new List<T>();

            while (reader.Read())
            {
                if (reader.FieldCount == 0)
                {
                    continue; // Skip empty rows
                }
                result.Add(mapper.Invoke(reader));
            }

            return result;
        }
    }
}
