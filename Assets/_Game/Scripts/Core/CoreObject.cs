using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreObject : MonoBehaviour
{
    private void Awake()
    {
        DestroyIfNeeded();
        DontDestroyOnLoad(gameObject);
    }

    private void DestroyIfNeeded()
    {
        if (Application.isEditor)
        {
            CoreObject[] objects = FindObjectsOfType<CoreObject>();

            if (objects.Length > 1)
            {
                Debug.Log("Destroy duplicated core object");
                Destroy(gameObject);
            }
        }
        else
        {
            Scene scene = SceneManager.GetActiveScene();

            if (!Enum.TryParse(scene.name, out SceneId sceneId) || sceneId != SceneId.Load)
            {
                Debug.LogError("The core object should not exist in other scenes except the loading scene");
                Destroy(gameObject);
            }
        }
    }
}
