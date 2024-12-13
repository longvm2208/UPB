using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
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

        if (GameSettings.Instance.IsInternetTime)
        {
            timeFetcher.FetchTimeFromServer(1, (DateTime startupTime) =>
            {
                startupTime -= TimeSpan.FromSeconds(Time.realtimeSinceStartup);
                GameManager.Ins.SetStartupTime(startupTime);
            });
        }

        // Đợi để lấy time internet
        yield return new WaitForSeconds(1.5f);

        DataManager.Ins.LoadData();
        AudioManager.Ins.Initialize();
        VibrationManager.Initialize();

        yield return new WaitForSeconds(1.5f);

        LoadSceneManager.Ins.LoadScene(SceneId.Home, LoadSceneManager.Mode.Before, (float progress) =>
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
