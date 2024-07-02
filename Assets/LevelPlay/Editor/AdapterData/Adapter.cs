using System;
using System.Collections.Generic;

namespace Unity.Services.LevelPlay.Editor.AdapterData
{
    class Adapter
    {
        public enum Status
        {
            NotInstalled = 1,
            UpdateAvailable = 2,
            UpToDate = 3
        }

        internal const string None = "none";
        public Status CurrentStatus { get; private set; }
        public string CurrentVersion { get; private set; }
        public string LatestVersion { get; }
        public string DownloadURL { get; }
        public string AdapterName { get; }
        public bool IsNewAdapter { get; }
        public bool IsRecommended { get; }
        public Dictionary<string, string> SDKVersions { get; }

        public Adapter(
            string adapterName,
            string latestVersion,
            string currentVersion,
            string downloadURL,
            bool isNewAdapter,
            bool isRecommended,
            Dictionary<string, string> sdkVersions)
        {
            AdapterName = adapterName;
            LatestVersion = latestVersion;
            DownloadURL = downloadURL;
            IsNewAdapter = isNewAdapter;
            IsRecommended = isRecommended;
            SDKVersions = sdkVersions;
            CurrentVersion = currentVersion;
            CurrentStatus = GetStatus(currentVersion, latestVersion);
        }

        public void UpdateStatusWithVersion(string newVersion)
        {
            CurrentStatus = GetStatus(newVersion, LatestVersion);
        }

        public void UpdateCurrentVersion(string newVersion)
        {
            CurrentVersion = newVersion;
        }

        static Status GetStatus(string currentVersion, string latestVersion)
        {
            if (currentVersion.Equals(None))
            {
                return Status.NotInstalled;
            }

            // If a new version of the sdk exists,change status to InstalledAndUpdateAvailable
            return IsLatestVersionNewerThanCurrentVersion(currentVersion, latestVersion)
                ? Status.UpdateAvailable : Status.UpToDate;
        }

        public static bool IsLatestVersionNewerThanCurrentVersion(string current, string latest)
        {
            if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(latest) ||
                !Version.TryParse(current, out var currentVersion) || !Version.TryParse(latest, out var latestVersion))
            {
                return false;
            }

            return latestVersion > currentVersion;
        }
    }
}
