using System.Linq;
using UnityBlocks.DataSync.Data;
using UnityEngine;

namespace UnityBlocks.DataSync.Parser
{
    public class SpreadsheetPageParser : MonoBehaviour, ISpreadsheetParser
    {
        [SerializeField] private ScriptableObject dataToUpdate;
        [SerializeField] private bool debug;

        public void ParseAndFill(string csvContent)
        {
            Log("Parsing CSV content...");
            var page = CsvParser.ParseCsv(csvContent);
            Log("Parsed CSV data into page count = " + page.Count);
            FillContent(page);
        }

        private void FillContent(SpreadsheetPage page)
        {
            if (dataToUpdate is IScriptableObjectTypedGroup typedGroup)
            {
                var typedList = typedGroup.List.Cast<ISpreadsheetBindable>();
                var existingAssets = typedList.ToDictionary(item => item.id);
                foreach (var pageKey in page.Keys)
                {
                    var line = page[pageKey];
                    if (existingAssets.TryGetValue(pageKey, out var existingAsset))
                    {
                        Log($"Updating asset with key: {pageKey}");
                        existingAsset.Parse(line);
                    }
                    else
                    {
                        Log($"Missing asset for key: {pageKey}, creating new one.");
                        var newItem = typedGroup.CreateSubAsset();
                        if (newItem is ISpreadsheetBindable bindableData)
                        {
                            bindableData.id = pageKey;
                            bindableData.Parse(line);
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Wrong SO type");
            }
        }

        private void Log(string text)
        {
            if (debug)
            {
                Debug.Log("SpreadsheetParser: " + text);
            }
        }
    }
}