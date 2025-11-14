using System;
using System.Collections;
using UnityEngine;

namespace UnityBlocks.DataSync.Fetchers
{
    [CreateAssetMenu(menuName = "Unity Blocks/DataSync/Fetchers/Google Sheets Fetcher", fileName = "GoogleSheetsFetcher")]
    public class GoogleSheetsFetcherSO : SpreadsheetFetcherSO
    {
        private const string UrlTemplate = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";

        [Tooltip("The Google Sheets document id (the long id in the sheet URL)")]
        public string sheetId;

        [Tooltip("The gid (page id) inside the Google Sheet. Leave empty or 0 for the first sheet.")]
        public string pageId = "0";

        public string GetUrl()
        {
            return string.Format(UrlTemplate, sheetId, pageId);
        }

    public override IEnumerator Fetch(DownloaderSettings settings, Action<string> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(sheetId))
            {
                onError?.Invoke("sheetId is empty in GoogleSheetsFetcherSO");
                yield break;
            }

            var url = GetUrl();
            var downloader = new CsvDataDownloader();
            yield return downloader.Download(url, onSuccess, onError, settings);
        }
    }
}
