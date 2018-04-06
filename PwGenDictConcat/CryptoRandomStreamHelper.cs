using KeePassLib.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwGenDictConcat
{
    static class CryptoRandomStreamHelper
    {
        public static int GetRandomInt(this CryptoRandomStream crs, int max)
        {
            var rand = crs.GetRandomUInt64();
            return (int)(rand % (ulong)max);
        }

        public static T GetRandomElement<T>(this CryptoRandomStream crs, IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(GetRandomInt(crs, enumerable.Count()));
        }

        public static IEnumerable<T> GetRandomPermutation<T>(this CryptoRandomStream crs, IEnumerable<T> enumerable)
        {
            var len = enumerable.Count();
            var set = new bool[len];
            var ret = new T[len];

            foreach(var elem in enumerable)
            {
                var index = GetRandomInt(crs, len);
                while (set[index])
                    index = (index + 1) % len;

                ret[index] = elem;
                set[index] = true;
            }

            return ret;
        }
    }
}
