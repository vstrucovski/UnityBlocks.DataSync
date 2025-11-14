using System;

namespace UnityBlocks.DataSync.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : Attribute
    {
        public readonly string Label;
        public ButtonAttribute(string label = null)
        {
            Label = label;
        }
    }
}
