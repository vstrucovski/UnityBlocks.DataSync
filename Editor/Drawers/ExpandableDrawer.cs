using UnityBlocks.DataSync.Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExpandableAttribute))]
public class ExpandableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Default drawing (allow expansion)
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
