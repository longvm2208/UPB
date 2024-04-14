using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneSwitcher
{
    private const string ScenesFolderPath = "Assets/_Game/Scenes/";

    [MenuItem("Scenes/Load #1")]
    public static void OpenLoad()
    {
        OpenScene("Load");
    }

    [MenuItem("Scenes/Load #1", true)]
    public static bool OpenLoadValidate()
    {
        return OpenSceneValidate("Load");
    }

    [MenuItem("Scenes/Home #2")]
    public static void OpenHome()
    {
        OpenScene("Home");
    }

    [MenuItem("Scenes/Home #2", true)]
    public static bool OpenHomeValidate()
    {
        return OpenSceneValidate("Home");
    }

    [MenuItem("Scenes/Game #3")]
    public static void OpenGame()
    {
        OpenScene("Game");
    }

    [MenuItem("Scenes/Game #3", true)]
    public static bool OpenGameValidate()
    {
        return OpenSceneValidate("Game");
    }

    [MenuItem("Scenes/Test #T")]
    public static void OpenTest()
    {
        OpenScene("Test");
    }

    [MenuItem("Scenes/Test #T", true)]
    public static bool OpenTestValidate()
    {
        return OpenSceneValidate("Test");
    }

    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(ScenesFolderPath + sceneName + ".unity");
        }
    }

    private static bool OpenSceneValidate(string sceneName)
    {
        return System.IO.File.Exists(ScenesFolderPath + sceneName + ".unity");
    }
}