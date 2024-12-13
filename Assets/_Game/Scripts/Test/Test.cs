using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    EventSystem eventSystem;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
    }

    [Button]
    public void CheckEventSystem()
    {
        if (eventSystem == null) Debug.Log("null");
    }

    [Button]
    public void SetEventSystem()
    {
        eventSystem = EventSystem.current;
    }

    [Button]
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(3);
    }
}
