using UnityBlocks.DataSync.Parser;
using UnityEngine;

namespace UnityBlocks.DataSync.Data
{
    public abstract class BindableScriptableObject : ScriptableObject, ISpreadsheetBindable
    {
        [field: SerializeField] public string id { get; set; }

        public virtual void Parse(SpreadsheetLine spreadsheetData)
        {
            DataBinder.Bind(this, spreadsheetData);
        }
    }
}