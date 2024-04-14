using Sirenix.OdinInspector;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void Register()
    {
        EventDispatcher.RegisterListener(EventId.Example, TestMethod);
    }

    [Button]
    public void Unregister()
    {
        EventDispatcher.UnregisterListener(EventId.Example, TestMethod);
    }

    private void TestMethod(object args)
    {
        Debug.Log((int)args);
    }
}
