using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rivière.BusinessLogic;
using Microsoft.Xna.Framework;

namespace Rivière.Utils
{
    static class ExtensionMethods
    {
        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static ReadOnlyDictionary<TKey, TValue> ReadOnly<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);

        public static Color ToXNAColor(this CardColor color) 
            => (color == CardColor.Red) ? Color.Red : Color.Black;
    }
}
