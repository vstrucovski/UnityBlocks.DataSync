using UnityEngine;

namespace UnityBlocks.DataSync
{
    [CreateAssetMenu(menuName = "Unity Blocks/DataSync/Downloader Settings", fileName = "DownloaderSettings")]
    public class DownloaderSettings : ScriptableObject
    {
        [Tooltip("Number of retry attempts after the initial request fails. Set to 0 to disable retries.")]
        public int retries = 0;

        [Tooltip("Timeout in seconds for each request. Set to 0 to use UnityWebRequest default (no timeout).")]
        public float timeoutSeconds = 10f;

        [Tooltip("Delay in seconds between retries.")]
        public float retryDelaySeconds = 1f;
        
        [Tooltip("Enable logging for download stages (start, attempts, success, failures). This is independent of the component's Debug flag.")]
        public bool enableLogs = false;
    }
}
