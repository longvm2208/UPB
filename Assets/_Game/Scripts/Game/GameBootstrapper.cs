using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    private void Start()
    {
        LoadSceneManager.Instance.OpenAnimation();
    }
}
