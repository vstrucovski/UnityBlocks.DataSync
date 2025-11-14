using System.Reflection;
using UnityBlocks.DataSync.Attributes;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    private bool ShouldShow(SerializedProperty property)
    {
        var attr = (ShowIfAttribute)attribute;
        var target = property.serializedObject.targetObject;
        var t = target.GetType();
        var field = t.GetField(attr.ConditionField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        bool result = true;
        if (field != null)
        {
            var val = field.GetValue(target);
            if (val is bool b) result = b;
        }
        else
        {
            var propInfo = t.GetProperty(attr.ConditionField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propInfo != null)
            {
                var pv = propInfo.GetValue(target);
                if (pv is bool pb) result = pb;
            }
        }

        return attr.Invert ? !result : result;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShouldShow(property))
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ShouldShow(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
    }
}
