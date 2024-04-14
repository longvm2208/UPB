using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InspectorUtils
{
    [MenuItem("Project/Inspector Utils/Lock Inspector &l")]
    public static void LockInspector()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Project/Inspector Utils/Enable Constrain Proportions Scale &e")]
    public static void EnableConstrainProportionsScale()
    {
        foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors)
        {
            if (activeEditor.target is not Transform) continue;

            var transform = (Transform)activeEditor.target;
            var property = transform.GetType().GetProperty("constrainProportionsScale", BindingFlags.NonPublic | BindingFlags.Instance);

            if (property == null) continue;

            var value = (bool)property.GetValue(transform, null);
            property.SetValue(transform, !value, null);
        }
    }

    [MenuItem("Project/Inspector Utils/Toggle Debug Mode - Mouse Over Window &d")]
    public static void ToggleDebugMode()
    {
        EditorWindow targetInspector = EditorWindow.mouseOverWindow;

        if (targetInspector != null && targetInspector.GetType().Name == "InspectorWindow")
        {
            Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
            FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            InspectorMode mode = (InspectorMode)field.GetValue(targetInspector);
            mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal);

            MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(targetInspector, new object[] { mode });

            targetInspector.Repaint();
        }
    }

    [MenuItem("Project/Inspector Utils/Lock Inspector &l", true)]
    [MenuItem("Project/Inspector Utils/Enable Constrain Proportions Scale &e", true)]
    [MenuItem("Project/Inspector Utils/Toggle Debug Mode - Mouse Over Window &d", true)]
    public static bool Validation()
    {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}