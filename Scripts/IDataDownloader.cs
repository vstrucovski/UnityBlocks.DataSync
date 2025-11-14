using System;
using System.Collections;

namespace UnityBlocks.DataSync
{
    public interface IDataDownloader
    {
        IEnumerator Download(string url, Action<string> onSuccess, Action<string> onError, DownloaderSettings settings = null);
    }
}