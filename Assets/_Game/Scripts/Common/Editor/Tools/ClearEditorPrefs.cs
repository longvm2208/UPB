using UnityEditor;

namespace EditorTools
{
    public class ClearEditorPrefs
    {
        [MenuItem("Project/Clear All EditorPrefs")]
        public static void Clear()
        {
            if (EditorUtility.DisplayDialog("Clear All EditorPrefs",
                "Are you sure you want to clear all EditorPrefs? " +
                "This action cannot be undone.", "Yes", "No"))
            {
                EditorPrefs.DeleteAll();
            }
        }
    }
}
