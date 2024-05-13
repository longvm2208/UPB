public class Singleton<T> where T : Singleton<T>, new()
{
    private T instance;
    
    public T Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    public bool HasInstance => instance != null;
}
