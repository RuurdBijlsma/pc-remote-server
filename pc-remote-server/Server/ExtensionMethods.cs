using System.Collections.Generic;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration

namespace pc_remote_server.Server
{
    public static class ExtensionMethods
    {
        public static void Deconstruct<T>(this IEnumerable<T> enumerable, out T elem1, out T elem2)
        {
            elem1 = enumerable.ElementAtOrDefault(0);
            elem2 = enumerable.ElementAtOrDefault(1);
        }
    }
}