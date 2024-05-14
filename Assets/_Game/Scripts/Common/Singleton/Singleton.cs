public class Singleton<T> where T : Singleton<T>, new()
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public static bool HasInstance => instance != null;
}
