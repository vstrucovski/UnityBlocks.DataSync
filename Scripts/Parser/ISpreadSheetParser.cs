namespace UnityBlocks.DataSync.Parser
{
    public interface ISpreadsheetParser
    {
        void ParseAndFill(string csvContent);
    }
}
namespace UnityBlocks.DataSync.Parser
{
    // Deprecated compatibility shim. Use `ISpreadsheetParser` instead (note 'Spreadsheet' spelling).
    [System.Obsolete("ISpreadSheetParser has been renamed to ISpreadsheetParser. Use ISpreadsheetParser instead.")]
    public interface ISpreadSheetParser : ISpreadsheetParser { }
}