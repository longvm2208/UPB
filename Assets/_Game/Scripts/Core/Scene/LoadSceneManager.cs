using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoadSceneManager : SingletonMonoBehaviour<LoadSceneManager>
{
    public enum Mode { Before, With, After }

    [SerializeField] private SceneTransition transition;

    private bool isLoading;
    private SceneId currentScene = SceneId.None;

    public bool IsLoading => isLoading;
    public SceneId CurrentScene => currentScene;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        transition.Initialize();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;

        if (Enum.TryParse(scene.name, out SceneId id))
        {
            currentScene = id;
        }
        else
        {
            Debug.LogError($"There is no id corresponding to this scene: {scene.name}");

            currentScene = SceneId.None;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    public void LoadScene(SceneId sceneId, Mode mode = Mode.After,
        Action<float> onLoading = null, Action onComplete = null)
    {
        isLoading = true;

        switch (mode)
        {
            case Mode.Before:
                StartCoroutine(LoadBefore(sceneId, onLoading, onComplete));
                break;
            case Mode.With:
                StartCoroutine(LoadWith(sceneId, onLoading, onComplete));
                break;
            case Mode.After:
                StartCoroutine(LoadAfter(sceneId, onLoading, onComplete));
                break;
        }
    }

    private IEnumerator LoadBefore(SceneId sceneId, Action<float> onLoading, Action onComplete)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId.ToString());
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            onLoading?.Invoke(operation.progress);

            yield return null;

            if (operation.progress >= 0.9f && !operation.allowSceneActivation)
            {
                yield return transition.Close();

                OnSceneSwitching();
                operation.allowSceneActivation = true;
            }
        }

        onComplete?.Invoke();
    }

    private IEnumerator LoadWith(SceneId sceneId, Action<float> onLoading, Action onComplete)
    {
        transition.Close();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId.ToString());
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            onLoading?.Invoke(operation.progress);

            yield return null;

            if (operation.progress >= 0.9f && !operation.allowSceneActivation)
            {
                yield return new WaitUntil(() => transition.IsCloseComplete);

                OnSceneSwitching();
                operation.allowSceneActivation = true;
            }
        }

        onComplete?.Invoke();
    }

    private IEnumerator LoadAfter(SceneId sceneId, Action<float> onLoading, Action onComplete)
    {
        yield return transition.Close();

        OnSceneSwitching();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId.ToString());

        while (!operation.isDone)
        {
            onLoading?.Invoke(operation.progress);

            yield return null;
        }

        onComplete?.Invoke();
    }

    private void OnSceneSwitching()
    {
        DOTween.KillAll();
        DataManager.Instance.SaveData();
        ScheduleUtils.StopAllCoroutines();
    }

    public void OpenAnimation(Action onComplete = null)
    {
        transition.Open(onComplete);
    }
}
