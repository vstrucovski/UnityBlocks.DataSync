using System;
using System.Linq;
using System.Reflection;
using UnityBlocks.DataSync.Attributes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class ButtonMethodDrawer : Editor
{
    // Restrict to your library namespace to avoid impacting unrelated inspectors
    private const string AllowedNamespacePrefix = "UnityBlocks.DataSync";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Only operate on types in our library namespace
        var targetType = target.GetType();
        if (string.IsNullOrEmpty(targetType.Namespace) || !targetType.Namespace.StartsWith(AllowedNamespacePrefix))
            return;

        var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetParameters().Length == 0);

        foreach (var m in methods)
        {
            var attr = m.GetCustomAttribute<ButtonAttribute>(true);
            if (attr == null) continue;

            var label = string.IsNullOrEmpty(attr.Label) ? ObjectNames.NicifyVariableName(m.Name) : attr.Label;
            if (GUILayout.Button(label))
            {
                // Support multi-object editing
                foreach (var obj in targets)
                {
                    try
                    {
                        var method = obj.GetType().GetMethod(m.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        method?.Invoke(obj, null);
                        if (obj is UnityEngine.Object uobj)
                        {
                            EditorUtility.SetDirty(uobj);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
        }
    }
}
