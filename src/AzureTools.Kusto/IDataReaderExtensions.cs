namespace AzureTools.Kusto
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class IDataReaderExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this IDataReader reader) where T: class, new()
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            var properties = typeof(T).GetProperties();
            var result = new List<T>();

            while (reader.Read())
            {
                var row = new T();
                foreach (var prop in properties)
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        prop.SetValue(row, reader[prop.Name]);
                    }
                }

                result.Add(row);
            }

            return result;
        }
    }
}
