using System;
using System.Collections;
using UnityEngine;

namespace UnityBlocks.DataSync.Fetchers
{
    [CreateAssetMenu(menuName = "Unity Blocks/DataSync/Fetchers/URL Fetcher", fileName = "UrlFetcher")]
    public class UrlFetcherSO : SpreadsheetFetcherSO
    {
        [Tooltip("Static URL to fetch. If left empty, sheetId/pageId will be ignored.")]
        public string url;

    public override IEnumerator Fetch(DownloaderSettings settings, Action<string> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrEmpty(url))
            {
                onError?.Invoke("url is empty in UrlFetcherSO");
                yield break;
            }

            var downloader = new CsvDataDownloader();
            yield return downloader.Download(url, onSuccess, onError, settings);
        }
    }
}
