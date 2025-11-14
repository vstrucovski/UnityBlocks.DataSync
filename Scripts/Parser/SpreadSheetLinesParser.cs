using System.Collections.Generic;
using UnityBlocks.DataSync.Data;
using UnityEngine;

namespace UnityBlocks.DataSync.Parser
{
    public class SpreadsheetLinesParser : MonoBehaviour, ISpreadsheetParser
    {
        [SerializeField] private List<ScriptableObject> dataToUpdate;
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
            var flattenList = new List<ScriptableObject>();
            foreach (var item in dataToUpdate)
            {
                if (item is IScriptableObjectGroup group)
                {
                    flattenList.AddRange(group.List);
                }
                else
                {
                    flattenList.Add(item);
                }
            }

            Log($"Found {flattenList.Count} objects to update");
            foreach (var item in flattenList)
            {
                if (item is ISpreadsheetBindable csvRemote)
                {
                    var key = csvRemote.id;
                    if (!page.ContainsKey(key))
                    {
                        Log("No remote data for key => " + key);
                        continue;
                    }

                    var line = page[key];
                    if (line != null)
                    {
                        csvRemote.Parse(line);
                    }
                }
            }

            foreach (var pageKey in page.Keys)
            {
                // TODO: Handle any unmatched data
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