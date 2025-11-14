using System;
using UnityEngine;

namespace UnityBlocks.DataSync.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string ConditionField;
        public readonly bool Invert;

        public ShowIfAttribute(string conditionField, bool invert = false)
        {
            ConditionField = conditionField;
            Invert = invert;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class HideIfAttribute : ShowIfAttribute
    {
        public HideIfAttribute(string conditionField) : base(conditionField, true) { }
    }
}
