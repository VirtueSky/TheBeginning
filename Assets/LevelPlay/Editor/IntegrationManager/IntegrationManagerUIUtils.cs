
using Unity.Services.LevelPlay.Editor.AdapterData;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager
{
    static class IntegrationManagerUIUtils
    {
        const string k_LabelInstall = "Install";
        const string k_LabelUpdate = "Update";
        const string k_LabelUpdated = "Updated";
        const string k_TooltipAndroidSdk = "Android SDK version:";
        const string k_TooltipIosSdk = "iOS SDK version:";
        const string k_TooltipLatestVersion = "Latest Version:";
        const string k_TooltipAdapterVersion = "Adapter Version:";
        const string k_Android = "Android";
        const string k_Ios = "iOS";

        internal static string GetButtonText(Adapter.Status status, string newVersion)
        {
            string text;
            switch (status)
            {
                case Adapter.Status.NotInstalled:
                    text = k_LabelInstall;
                    break;
                case Adapter.Status.UpdateAvailable:
                    text = string.IsNullOrWhiteSpace(newVersion) ? k_LabelUpdate : $"{k_LabelUpdate} to {newVersion}";
                    break;
                case Adapter.Status.UpToDate:
                default:
                    text = k_LabelUpdated;
                    break;
            }

            return text;
        }

        internal static string GetToolTipText(Adapter adapter)
        {
            var tooltipText = $"{k_TooltipLatestVersion} \n " +
                              $"{adapter.AdapterName} " +
                              $"{k_TooltipAdapterVersion} " +
                              $"{adapter.LatestVersion}";

            if (adapter.SDKVersions.TryGetValue(k_Android, out var androidVersion))
            {
                tooltipText = $"{tooltipText}\n {k_TooltipAndroidSdk} {androidVersion}";
            }

            if (adapter.SDKVersions.TryGetValue(k_Ios, out var iosVersion))
            {
                tooltipText = $"{tooltipText}\n {k_TooltipIosSdk} {iosVersion}";
            }

            return tooltipText;
        }
    }
}
