using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.OpenAnimation();
    }
}
