using System;
using System.Collections.Generic;
using System.Data;

namespace TED.Dashboard.Repository
{
    public static class Extensions
    {
        #region LINQ extensions for reading from datareader

        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }

        #endregion

        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }


        public static string ToDisplayString(this DateTime date)
        {
            return string.Format("{0} {1}", date.ToShortDateString(), date.ToShortTimeString());
        }

        
    }
}
