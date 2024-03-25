using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

public static class ShuffleListExtensions
{
    /// <summary>
    /// Shuffle the list in place using the Fisher-Yates method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    /// <summary>
    /// Return a random item from the list.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T[] RandomItems<T>(this IList<T> list, int itemsCount)
    {
        itemsCount = Math.Clamp(itemsCount, 0, list.Count);
        var result = new T[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            result[i] = list.RandomItem();
        }
        return result;
    }

    public static T RandomItemThreadSafe<T>(this IList<T> list, Unity.Mathematics.Random random)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[random.NextInt(0, list.Count)];
    }

    public static T RandomItem<T>(this NativeArray<T> list) where T : struct
    {
        if (list.Length == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty nativeArray");
        return list[UnityEngine.Random.Range(0, list.Length)];
    }

    /// <summary>
    /// Removes a random item from the list, returning that item.
    /// Sampling without replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }
}