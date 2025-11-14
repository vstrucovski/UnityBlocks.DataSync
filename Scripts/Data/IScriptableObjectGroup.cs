using System.Collections.Generic;
using UnityEngine;

namespace UnityBlocks.DataSync.Data
{
    public interface IScriptableObjectGroup
    {
        List<ScriptableObject> List { get; }
    }
}