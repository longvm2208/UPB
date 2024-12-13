using System.Collections.Generic;

public static class CollectionExt
{
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static void Swap<T>(this IList<T> list, int i1, int i2)
    {
        if (i1 < 0 || i1 >= list.Count || i2 < 0 || i2 >= list.Count) return;
        (list[i1], list[i2]) = (list[i2], list[i1]);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();

        int i = list.Count;
        int j;

        while (i > 1)
        {
            i--;
            j = rng.Next(i + 1);
            T t = list[j];
            list[j] = list[i];
            list[i] = t;
        }
    }

    public static T Last<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }

    public static void Rotate<T>(this List<T> list, int pos)
    {
        if (pos <= 0 || pos >= list.Count) return;

        int count = list.Count;
        list.Reverse();
        list.Reverse(0, pos);
        list.Reverse(pos, count - pos);
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

    public static List<List<T>> GetAllCombinations<T>(this List<T> list, int k)
    {
        List<List<T>> combinations = new List<List<T>>();
        List<T> currentCombination = new List<T>();

        GenerateAllCombinations(list, k, 0, currentCombination, combinations);
        return combinations;
    }

    static void GenerateAllCombinations<T>(List<T> list, int k, int startIndex, List<T> currentCombination, List<List<T>> combinations)
    {
        if (currentCombination.Count == k)
        {
            combinations.Add(new List<T>(currentCombination));
            return;
        }

        for (int i = startIndex; i < list.Count; i++)
        {
            currentCombination.Add(list[i]);
            GenerateAllCombinations(list, k, i + 1, currentCombination, combinations);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }

    public static IEnumerable<List<T>> GetEachCombination<T>(this List<T> list, int k)
    {
        return GenerateEachCombination(list, k, 0, new List<T>());
    }

    static IEnumerable<List<T>> GenerateEachCombination<T>(List<T> list, int k, int startIndex, List<T> currentCombination)
    {
        if (currentCombination.Count == k)
        {
            yield return new List<T>(currentCombination);
            yield break;
        }

        for (int i = startIndex; i < list.Count; i++)
        {
            currentCombination.Add(list[i]);
            foreach (var combination in GenerateEachCombination(list, k, i + 1, currentCombination))
            {
                yield return combination;
            }
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }
}
