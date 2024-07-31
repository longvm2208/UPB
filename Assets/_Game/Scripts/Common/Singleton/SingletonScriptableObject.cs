using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");

                if (assets == null || assets.Length < 1)
                {
                    Debug.LogError("Could not find any instances");
                    return null;
                }
                else if (assets.Length > 1)
                {
                    Debug.LogWarning("Multiple instances were found");
                }

                instance = assets[0];
            }

            return instance;
        }
    }

    public static bool HasInstance => instance != null;
}
