using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class CustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get => 0; }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerPrefs.DeleteAll();

        if (GameSettings.Instance.IsEnableAds)
        {
            Debug.Log("Ads is enabled");
        }
        else
        {
            Debug.LogError("Ads is disabled");
        }

        string path = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        var settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(path);
        if (settings.ActivePlayModeDataBuilderIndex == 2)
        {
            Debug.Log("Addressables - Play Mode Script: Use Existing Build");
        }
        else
        {
            Debug.LogError("Addressables - Play Mode Script: Use Asset Database");
        }
    }
}
