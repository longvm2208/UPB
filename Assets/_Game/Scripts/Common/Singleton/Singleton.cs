public class Singleton<T> where T : Singleton<T>, new()
{
    static T instance;
    public static T Instance => instance ??= new T();

    public static bool HasInstance => instance != null;
}
