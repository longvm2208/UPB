using System.Collections.Generic;
using System.Linq;

public static class CollectionExtensions
{
    /// <summary>
    /// Determines whether a collection is null or has no elements without having to enumerate the entire collection to get a count.<br/>
    /// Uses LINQ's Any() method to determine if the collection is empty, so there is some GC overhead.
    /// </summary>
    /// <param name="list">List to evaluate</param>
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || !list.Any();
    }

    /// <summary>
    /// Swaps two elements in the list at the specified indices.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="indexA">The index of the first element.</param>
    /// <param name="indexB">The index of the second element.</param>
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }

    /// <summary>
    /// Shuffle an array or a list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ts"></param>
    public static void Shuffle<T>(this IList<T> ts)
    {
        System.Random random = new System.Random();

        int i = ts.Count;
        int j;

        while (i > 1)
        {
            i--;
            j = random.Next(i + 1);
            T t = ts[j];
            ts[j] = ts[i];
            ts[i] = t;
        }
    }

    public static T Last<T>(this IList<T> ts)
    {
        return ts[ts.Count - 1];
    }

    public static void Rotate<T>(this List<T> list, int position)
    {
        int count = list.Count;
        position = position % count;

        if (position == 0) return;

        if (position < 0) position += count;

        list.Reverse();
        list.Reverse(0, position);
        list.Reverse(position, count - position);
    }

    /// <summary>
    /// Generates all combinations of size <paramref name="k"/> from the elements of the <paramref name="list"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The input list from which combinations are generated.</param>
    /// <param name="k">The size of combinations to be generated.</param>
    /// <returns>A list containing all combinations of size <paramref name="k"/> from the input list.</returns>
    public static List<List<T>> GetCombinations<T>(this List<T> list, int k)
    {
        List<List<T>> result = new List<List<T>>();
        List<T> currentCombination = new List<T>();

        GenerateCombinations(list, k, 0, currentCombination, result);
        return result;
    }

    /// <summary>
    /// Helper method to generate combinations recursively.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The input list from which combinations are generated.</param>
    /// <param name="k">The size of combinations to be generated.</param>
    /// <param name="startIndex">The index to start generating combinations from in the <paramref name="list"/>.</param>
    /// <param name="currentCombination">The current combination being constructed.</param>
    /// <param name="result">The list to store all generated combinations.</param>
    private static void GenerateCombinations<T>(List<T> list, int k, int startIndex, List<T> currentCombination, List<List<T>> result)
    {
        if (currentCombination.Count == k)
        {
            result.Add(new List<T>(currentCombination));
            return;
        }

        for (int i = startIndex; i < list.Count; i++)
        {
            currentCombination.Add(list[i]);
            GenerateCombinations(list, k, i + 1, currentCombination, result);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }

    public static int SelectWeightedRandomIndex(this IList<int> probabilities)
    {
        int totalWeight = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            totalWeight += probabilities[i];
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < probabilities.Count; i++)
        {
            cumulativeWeight += probabilities[i];

            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return UnityEngine.Random.Range(0, probabilities.Count);
    }
}
