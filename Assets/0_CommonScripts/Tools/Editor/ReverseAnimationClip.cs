using UnityEditor;
using UnityEngine;
using System.IO;

public static class ReverseAnimationClip
{
    [MenuItem("Tools/My Tools/Reverse Animation Clip", false)]
    static void Reverse()
    {
        string directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string fileName = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string fileExtension = Path.GetExtension(AssetDatabase.GetAssetPath(Selection.activeObject));
        fileName = fileName.Split('.')[0];
        string copiedFilePath = directoryPath + Path.DirectorySeparatorChar + fileName + "_Reversed" + fileExtension;

        AnimationClip originalClip = GetSelectedClip();

        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Selection.activeObject), copiedFilePath);

        var reversedClip = (AnimationClip)AssetDatabase.LoadAssetAtPath(copiedFilePath, typeof(AnimationClip));

        if (reversedClip == null) return;

        float clipLength = originalClip.length;

        // Get curve bindings
        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(originalClip);

        foreach (EditorCurveBinding curveBinding in curveBindings)
        {
            // Get the animation curve for the current binding
            AnimationCurve curve = AnimationUtility.GetEditorCurve(originalClip, curveBinding);

            // Reverse the keys of the curve
            Keyframe[] keys = curve.keys;
            for (int i = 0; i < keys.Length; i++)
            {
                Keyframe key = keys[i];
                key.time = clipLength - key.time;
                key.inTangent = -key.inTangent;
                key.outTangent = -key.outTangent;
                keys[i] = key;
            }

            // Set the reversed keys to the reversed clip
            AnimationUtility.SetEditorCurve(reversedClip, curveBinding, new AnimationCurve(keys));
        }

        // Copy animation events if any
        AnimationEvent[] events = AnimationUtility.GetAnimationEvents(originalClip);

        if (events.Length > 0)
        {
            for (int i = 0; i < events.Length; i++)
            {
                events[i].time = clipLength - events[i].time;
            }

            AnimationUtility.SetAnimationEvents(reversedClip, events);
        }
    }

    [MenuItem("Tools/My Tools/Reverse Animation Clip", true)]
    static bool ReverseValidation()
    {
        return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(AnimationClip);
    }

    static AnimationClip GetSelectedClip()
    {
        var clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);

        if (clips.Length > 0)
        {
            return clips[0] as AnimationClip;
        }

        return null;
    }
}
