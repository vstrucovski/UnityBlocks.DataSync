using System.Collections.Generic;
using UnityEngine;

namespace UnityBlocks.DataSync.Data
{
    public interface IScriptableObjectTypedGroup
    {
        List<ScriptableObject> List { get; }

        ScriptableObject CreateSubAsset();
    }
}