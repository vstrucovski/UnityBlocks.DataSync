using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityBlocks.DataSync.Parser
{
    public static class CsvParser
    {
        public static SpreadsheetPage ParseCsv(string csvContent)
        {
            var reader = new StringReader(csvContent);
            var spreadsheet = new SpreadsheetPage();
            var isEmpty = reader.Peek() == -1;
            if (isEmpty) return spreadsheet;

            var keysTxt = reader.ReadLine();
            if (string.IsNullOrEmpty(keysTxt))
            {
                Debug.LogError("Missed csv keys");
                return null;
            }

            var keys = ParseCsvLine(keysTxt);

            while (reader.Peek() != -1)
            {
                var line = new SpreadsheetLine();
                var lineStr = reader.ReadLine();
                var fields = ParseCsvLine(lineStr);
                if (fields.Count > 0)
                {
                    for (var index = 0; index < fields.Count; index++)
                    {
                        var field = fields[index];
                        if (index == 0)
                        {
                            spreadsheet.Add(field, line);
                        }
                        else
                        {
                            line[keys[index]] = field;
                        }
                    }
                }
            }

            return spreadsheet;
        }

        private static List<string> ParseCsvLine(string line)
        {
            var fields = new List<string>();
            if (line == null)
            {
                return fields;
            }

            var inQuotes = false;
            var current = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == '"')
                {
                    // If we're inside quotes and the next char is also a quote, it's an escaped quote ("")
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++; // skip the escaped quote
                    }
                    else
                    {
                        // toggle inQuotes state (entering or leaving quoted section)
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            fields.Add(current.ToString());
            return fields;
        }
    }
}