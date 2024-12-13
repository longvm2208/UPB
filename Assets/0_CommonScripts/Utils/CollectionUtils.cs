using System.Collections.Generic;

public class CollectionUtils
{
    public static List<T> ConcatenateLists<T>(params List<T>[] lists)
    {
        List<T> concatenatedList = new List<T>();

        for (int i = 0; i < lists.Length; i++)
        {
            concatenatedList.AddRange(lists[i]);
        }

        return concatenatedList;
    }
}
