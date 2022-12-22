using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM
{
    internal static class Conversions
    {
        internal static string ConvertToSqlFormat(this object obj)
        {
            if(obj is DateTime dt)
                return $"{dt.Month}.{dt.Day}.{dt.Year} {dt.Hour}:{dt.Minute}:{dt.Second}";
            return obj.ToString();
        }
    }
}
