using UnityEngine;

public class HomeBootstrapper : MonoBehaviour
{
    private void Start()
    {
        LoadSceneManager.Ins.OpenAnimation();
    }
}
