using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");

                if (assets == null || assets.Length < 1)
                {
                    Debug.LogError("Could not find any singleton scriptable object instances in the resources");
                }
                else if (assets.Length > 1)
                {
                    Debug.LogWarning("Multiple instances of the singleton scriptable object found in the resources");
                }

                instance = assets[0];
            }

            return instance;
        }
    }
}
