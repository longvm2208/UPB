using UnityEditor;

public class ScriptCreator
{
    private const string PATH = "Assets/_Game/Scripts/UI/Base";

    private const string PANEL_TEMPLATE = "/Editor/Templates/Panel.cs.txt";
    private const string POPUP_TEMPLATE = "/Editor/Templates/Popup.cs.txt";

    [MenuItem("Assets/Create/Panel Popup/C# Script/Panel", false, 0)]
    public static void CreatePanelScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH + PANEL_TEMPLATE, "Panel.cs");
    }

    [MenuItem("Assets/Create/Panel Popup/C# Script/Panel", true)]
    public static bool CreatePanelValidate()
    {
        return System.IO.File.Exists(PATH + PANEL_TEMPLATE);
    }

    [MenuItem("Assets/Create/Panel Popup/C# Script/Popup", false, 0)]
    public static void CreatePopupScript()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH + POPUP_TEMPLATE, "Popup.cs");
    }

    [MenuItem("Assets/Create/Panel Popup/C# Script/Popup", true)]
    public static bool CreatePopupValidate()
    {
        return System.IO.File.Exists(PATH + POPUP_TEMPLATE);
    }
}
