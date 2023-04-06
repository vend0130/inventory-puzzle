using System.Collections.Generic;

namespace Code.Extensions
{
    public static class ArrayExtension
    {
        public static List<T> Clone<T>(this List<T> list) =>
            new List<T>(list);
    }
}