using System;
using UnityEngine;

namespace UnityBlocks.DataSync.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class ReorderableListAttribute : PropertyAttribute
    {
        // Marker attribute for Editor drawer
    }
}
