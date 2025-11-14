using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityBlocks.DataSync.Attributes;
using UnityBlocks.DataSync.Data;
using UnityBlocks.DataSync.Fetchers;
using UnityBlocks.DataSync.Parser;
using UnityEngine;

namespace UnityBlocks.DataSync
{
    public class SpreadsheetSyncer : MonoBehaviour
    {
        [SerializeField] protected bool autoStart;
        [SerializeField] private bool isSingleType;

        [SerializeField, HideIf("isSingleType")]
        private List<ScriptableObject> dataToUpdate;

        [SerializeField, ShowIf("isSingleType")]
        private ScriptableObject dataToUpdateGroup;

        [SerializeField] private SpreadsheetFetcherSO fetcher;

        private IDataDownloader _downloader;
        [SerializeField] private DownloaderSettings settings;

        private void Start()
        {
            Init();
            if (autoStart)
            {
                Download();
            }
        }

        private void Init()
        {
            _downloader ??= new CsvDataDownloader(); //TODO rework to SO with diff impl
        }

        [ContextMenu("Download CSV")]
        [Button]
        public void Download()
        {
            // Prefer the serialized fetcher asset if present, otherwise fallback to the (deprecated) runtime fetcher.
            var activeFetcher = fetcher != null ? fetcher : fetcher;
            if (activeFetcher != null)
            {
                Log("Fetching via assigned fetcher");
                StartCoroutine(activeFetcher.Fetch(settings, content =>
                {
                    Log("Success fetched CSV data");
                    ApplyCsv(content);
                    Log("Completed sync");
                }, error => { Debug.LogError("Error fetching CSV: " + error); }));
                return;
            }

            Debug.LogError(
                "No fetcher assigned on SpreadsheetSyncer. Create a SpreadsheetFetcher SO (GoogleSheetsFetcher or UrlFetcher) and assign it to 'fetcherAsset'.");
        }

        public IEnumerator DownloadCoroutine()
        {
            var activeFetcher = fetcher != null ? fetcher : fetcher;
            if (activeFetcher != null)
            {
                Log("Fetching via assigned fetcher (coroutine)");
                yield return StartCoroutine(activeFetcher.Fetch(settings, content =>
                {
                    Log("Success fetched CSV data");
                    ApplyCsv(content);
                }, error => { Debug.LogError("Error fetching CSV: " + error); }));
                yield break;
            }

            Debug.LogError("No fetcher assigned on SpreadsheetSyncer. Cannot perform DownloadCoroutine.");
            yield break;
        }

        /// <summary>
        /// Apply raw CSV/text to the parser and fill content. Public so fetchers or tests can call it directly.
        /// </summary>
        public void ApplyCsv(string content)
        {
            var page = CsvParser.ParseCsv(content);
            Log("Parsed CSV data into page = " + page.Count);
            FillContent(page);
        }

        private void FillContent(SpreadsheetPage page)
        {
            if (isSingleType)
            {
                FillContentSingleType(page);
            }
            else
            {
                FillContentDifferentTypes(page);
            }
        }

        private void FillContentDifferentTypes(SpreadsheetPage page)
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
                //TODO if can't
            }
        }

        private void FillContentSingleType(SpreadsheetPage page)
        {
            if (dataToUpdateGroup is IScriptableObjectTypedGroup typedGroup)
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

        protected void Log(string text)
        {
            // Log only if the downloader settings request logs
            bool settingsLog = settings != null && settings.enableLogs;
            if (settingsLog)
            {
                Debug.Log("DataSync. " + text);
            }
        }
    }
}