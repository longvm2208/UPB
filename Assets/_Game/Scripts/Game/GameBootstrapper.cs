using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    private void Start()
    {
        LoadSceneManager.Ins.OpenAnimation();
    }
}
