using UnityEngine;

public class HomeBootstrapper : MonoBehaviour
{
    private void Start()
    {
        SceneLoader.Instance.OpenAnimation();
    }
}
