using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityBlocks.DataSync
{
    public class CsvDataDownloader : IDataDownloader
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public IEnumerator Download(string url, Action<string> onSuccess, Action<string> onError, DownloaderSettings settings = null)
        {
            int attempts = 1 + (settings?.retries ?? 0);
            float timeoutSeconds = settings?.timeoutSeconds ?? 0f;
            float retryDelay = settings?.retryDelaySeconds ?? 1f;
            bool enableLogs = settings?.enableLogs ?? false;

            if (enableLogs)
            {
                Debug.Log($"CsvDataDownloader: Starting download for URL: {url} (attempts: {attempts}, timeout: {timeoutSeconds}s)");
            }

            for (int attempt = 0; attempt < attempts; attempt++)
            {
                if (enableLogs)
                {
                    Debug.Log($"CsvDataDownloader: Attempt {attempt + 1}/{attempts} for URL: {url}");
                }
                using (var request = UnityWebRequest.Get(url))
                {
                    if (timeoutSeconds > 0f)
                    {
                        // UnityWebRequest.timeout is an int in seconds
                        request.timeout = Mathf.CeilToInt(timeoutSeconds);
                    }

                    yield return request.SendWebRequest();

                    // Log response code and length for better diagnostics
                    long responseCode = request.responseCode;
                    // Log response code and length for better diagnostics
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        if (enableLogs)
                        {
                            var text = request.downloadHandler?.text ?? string.Empty;
                            var len = text.Length;
                            const int previewMax = 1000;
                            var preview = len > previewMax ? text.Substring(0, previewMax) + "..." : text;
                            Debug.Log($"CsvDataDownloader: Success downloading URL: {url} (HTTP {responseCode}) - {len} chars\nPreview:\n" + preview);
                        }
                        onSuccess(request.downloadHandler.text);
                        yield break;
                    }
                    else
                    {
                        if (enableLogs)
                        {
                            var body = request.downloadHandler?.text;
                            const int previewMax = 1000;
                            if (!string.IsNullOrEmpty(body))
                            {
                                var preview = body.Length > previewMax ? body.Substring(0, previewMax) + "..." : body;
                                Debug.LogWarning($"CsvDataDownloader: Request failed (attempt {attempt + 1}/{attempts}) for URL: {url} - HTTP {responseCode} - Error: {request.error}\nResponse body preview:\n" + preview);
                            }
                            else
                            {
                                Debug.LogWarning($"CsvDataDownloader: Request failed (attempt {attempt + 1}/{attempts}) for URL: {url} - HTTP {responseCode} - Error: {request.error}");
                            }
                        }
                        // If this was the last attempt, report error
                        if (attempt >= attempts - 1)
                        {
                            if (enableLogs)
                                Debug.LogError($"CsvDataDownloader: All attempts failed for URL: {url} (final HTTP {responseCode})");
                            onError(request.error);
                            yield break;
                        }
                        else
                        {
                            // Wait before retrying
                            if (retryDelay > 0f)
                                yield return new WaitForSeconds(retryDelay);
                        }
                    }
                }
            }
        }
    }
}