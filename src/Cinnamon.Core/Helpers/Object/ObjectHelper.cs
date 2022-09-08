using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinnamon.Core
{
    public static class ObjectHelper
    {
        public static bool HasProperty(this Object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static object? GetProperty(this Object obj, string propertyName)
        {
            return obj.GetType()?.GetProperty(propertyName)?.GetValue(obj);
        }
    }
}
