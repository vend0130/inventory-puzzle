using System.Collections.Generic;
using UnityEngine;

namespace Code.Extensions
{
    public static class ArrayExtension
    {
        public static List<T> Clone<T>(this List<T> list) =>
            new List<T>(list);

        public static T GetRandomElement<T>(this List<T> list) =>
            list[Random.Range(0, list.Count)];
    }
}