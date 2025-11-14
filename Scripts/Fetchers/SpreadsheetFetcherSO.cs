using System;
using System.Collections;
using UnityEngine;

namespace UnityBlocks.DataSync.Fetchers
{
    public abstract class SpreadsheetFetcherSO : ScriptableObject
    {
        /// <summary>
        /// Fetch CSV/text. Implementations should use the provided
        /// <paramref name="settings"/> when performing network requests.
        /// The caller (usually a MonoBehaviour) will StartCoroutine on the returned IEnumerator.
        /// </summary>
        public abstract IEnumerator Fetch(DownloaderSettings settings,
            Action<string> onSuccess, Action<string> onError);
    }
}
