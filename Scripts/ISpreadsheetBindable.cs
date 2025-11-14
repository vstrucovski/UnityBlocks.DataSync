using UnityBlocks.DataSync.Parser;

namespace UnityBlocks.DataSync
{
    /// <summary>
    /// Contract for objects that can be bound/updated from a spreadsheet row.
    /// Provides a stable identifier and a Parse method to ingest a <see cref="SpreadsheetLine"/>.
    /// </summary>
    public interface ISpreadsheetBindable
    {
        /// <summary>Unique identifier used to match spreadsheet rows to objects.</summary>
        string id { get; set; }

        /// <summary>Parse the provided spreadsheet data into this object.</summary>
        void Parse(SpreadsheetLine spreadsheetData);
    }
}
