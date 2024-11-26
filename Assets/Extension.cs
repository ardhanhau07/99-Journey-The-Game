using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
   private static System.Random rng = new System.Random();

   public static void Shuffle<T>(this IList<T> list)
   {
    int n = list.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);

        Debug.Log($"Remaining items to shuffle: {n + 1}");
        Debug.Log($"Random index chosen: {k}");
        Debug.Log($"Swapping items: {list[k]} (at index {k}) with {list[n]} (at index {n})");

        T value = list[k];
        list[k] = list[n];
        list[n] = value;

        Debug.Log($"List after swap: {string.Join(",", list)}");
    }
   }
}
