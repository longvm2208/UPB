using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private TimeFetcher timeFetcher;

    [Header("ADDRESSABLE CACHING")]
    [SerializeField]
    private AssetReference reference;

    private void Awake()
    {
        if (reference != null)
        {
            reference.LoadAssetAsync<Sprite>();
        }
    }

    private IEnumerator Start()
    {
        DOVirtual.Float(0f, 0.6f, 3f, value =>
        {
            SetProgress(value);
        });

        timeFetcher.FetchTimeFromServer(1, (DateTime startupTime) =>
        {
            GameManager.Instance.SetStartupTime(startupTime);
        });

        yield return new WaitForSeconds(1.5f);

        DataManager.Instance.LoadData();
        AudioManager.Instance.Initialize();
        VibrationManager.Instance.Initialize();

        yield return new WaitForSeconds(1.5f);

        SceneLoader.Instance.LoadScene(SceneId.Home, SceneLoader.Mode.Before, (float progress) =>
        {
            SetProgress(0.6f + progress / 0.9f * 0.4f);
        }, () =>
        {
            // Load scene complete
        });
    }

    private void SetProgress(float progress)
    {
        fillImage.fillAmount = progress;
    }
}
