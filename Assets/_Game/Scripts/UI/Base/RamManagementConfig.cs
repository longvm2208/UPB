#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/UI Ram Management Config")]
public class RamManagementConfig : ScriptableObject
{
    public RamManagementType ManagementType;

    [Button]
    void ApplyManagementType()
    {
        string symbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        string[] symbols = symbolsString.Split(';');

        switch (ManagementType)
        {
            case RamManagementType.Addressable:
                AddSymbol(ref symbols, RamManagementType.Addressable.GetDescription());
                RemoveSymbol(ref symbols, RamManagementType.Resource.GetDescription());
                break;
            case RamManagementType.Resource:
                RemoveSymbol(ref symbols, RamManagementType.Addressable.GetDescription());
                AddSymbol(ref symbols, RamManagementType.Resource.GetDescription());
                break;
            default:
                RemoveSymbol(ref symbols, RamManagementType.Addressable.GetDescription());
                RemoveSymbol(ref symbols, RamManagementType.Resource.GetDescription());
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
