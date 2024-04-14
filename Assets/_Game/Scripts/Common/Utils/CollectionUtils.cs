using System.Collections.Generic;

public class CollectionUtils
{
    /// <summary>
    /// Concatenates multiple lists into a single list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the lists.</typeparam>
    /// <param name="lists">The lists to concatenate.</param>
    /// <returns>A new list containing all the elements from the input lists.</returns>
    public static List<T> ConcatenateLists<T>(params List<T>[] lists)
    {
        int totalCapacity = 0;

        foreach (List<T> list in lists)
        {
            totalCapacity += list.Count;
        }

        List<T> concatenatedList = new List<T>(totalCapacity);

        foreach (List<T> list in lists)
        {
            concatenatedList.AddRange(list);
        }

        return concatenatedList;
    }
}
