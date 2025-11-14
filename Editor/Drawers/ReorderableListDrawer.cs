using UnityBlocks.DataSync.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(ReorderableListAttribute))]
public class ReorderableListDrawer : PropertyDrawer
{
    private ReorderableList CreateList(SerializedProperty property, GUIContent label)
    {
        var list = new ReorderableList(property.serializedObject, property, true, true, true, true);
        list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, label);
        list.elementHeightCallback = (index) =>
        {
            var elem = property.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(elem, true) + 4;
        };
        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var elem = property.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(elem, true)), elem, GUIContent.none, true);
        };
        return list;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!property.isArray)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        var list = CreateList(property, label);
        list.DoList(position);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isArray) return EditorGUI.GetPropertyHeight(property, label, true);
        var list = CreateList(property, label);
        return list.GetHeight();
    }
}
