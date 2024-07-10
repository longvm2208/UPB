using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class CustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get => 0; }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerPrefs.DeleteAll();
    }
}
