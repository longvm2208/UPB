#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Mediation Config")]
public class MediationConfig : ScriptableObject
{
    public MediationType MediationType;

    [Button]
    void ApplyMediation()
    {
        string symbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        string[] symbols = symbolsString.Split(';');

        switch (MediationType)
        {
            case MediationType.AppLovinMax:
                AddSymbol(ref symbols, MediationType.AppLovinMax.GetDescription());
                RemoveSymbol(ref symbols, MediationType.IronSource.GetDescription());
                break;
            case MediationType.IronSource:
                RemoveSymbol(ref symbols, MediationType.AppLovinMax.GetDescription());
                AddSymbol(ref symbols, MediationType.IronSource.GetDescription());
                break;
            default:
                RemoveSymbol(ref symbols, MediationType.AppLovinMax.GetDescription());
                RemoveSymbol(ref symbols, MediationType.IronSource.GetDescription());
                break;
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", symbols));
    }

    void AddSymbol(ref string[] symbols, string symbol)
    {
        if (!System.Array.Exists(symbols, s => s == symbol))
        {
            ArrayUtility.Add(ref symbols, symbol);
        }
    }

    void RemoveSymbol(ref string[] symbols, string symbol)
    {
        ArrayUtility.Remove(ref symbols, symbol);
    }
}
#endif