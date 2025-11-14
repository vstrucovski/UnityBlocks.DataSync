using UnityBlocks.DataSync.Data;
using UnityBlocks.DataSync.Parser;
using UnityEngine;

namespace UnityBlocks.DataSync.Examples.Example_03
{
    [CreateAssetMenu(menuName = "Unity Blocks/DataSync/Example/TestItem_03")]
    public class TestItem : BindableScriptableObject
    {
        public int value;

        public override void Parse(SpreadsheetLine spreadsheetData)
        {
            DataBinder.Bind(this, spreadsheetData);
        }
    }
}