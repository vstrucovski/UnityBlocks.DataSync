using System;
using System.Linq;
using System.Reflection;
using UnityBlocks.DataSync;
using UnityBlocks.DataSync.Attributes;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpreadsheetSyncer), true)]
[CanEditMultipleObjects]
public class SpreadsheetSyncerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector first
        DrawDefaultInspector();

        var targetType = target.GetType();

        // Only operate on types in our library namespace
    const string AllowedNamespacePrefix = "UnityBlocks.DataSync";
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
