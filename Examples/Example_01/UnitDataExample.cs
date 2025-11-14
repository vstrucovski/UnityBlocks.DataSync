using UnityBlocks.DataSync.Parser;
using UnityEngine;

namespace UnityBlocks.DataSync.Examples.Example_01
{
    [CreateAssetMenu(menuName = "Unity Blocks/DataSync/Example/Unit Data (1)")]
    public class UnitDataExample : ScriptableObject, ISpreadsheetBindable
    {
        [field: SerializeField] public string id { get; set; } //first column
        public int health;                                     //second column
        public float attack;                                   //third column

        public void Parse(SpreadsheetLine spreadsheetData)
        {
            DataBinder.Bind(this, spreadsheetData);
        }
    }
}