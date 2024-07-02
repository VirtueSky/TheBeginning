using System;
using System.Collections.Generic;
using Unity.Services.LevelPlay.Editor.IntegrationManager;

namespace Unity.Services.LevelPlay.Editor.AdapterData
{
    static class AdapterFactory
    {
        static readonly string[] k_RecommendedAdapters = { "UnityAds" };
        internal const string k_AdapterIsNew = "isNewProvider";
        internal const string k_AdapterAndroidSdkVer = "AndroidSDKVersion";
        internal const string k_AdapterIosSdkVer = "iOSSDKVersion";
        internal const string k_AdapterUnityAdapterVersion = "UnityAdapterVersion";
        internal const string k_AdapterDownloadUrl = "DownloadUrl";
        internal const string k_AdapterFileName = "FileName";
        internal const string k_Android = "Android";
        internal const string k_IOS = "iOS";
        const string k_UnityAdapterMacro = "${UnityAdapterVersion}";

        /// <summary>
        /// Creates an <see cref="Adapter"/> instance based on the provided data and XML settings.
        /// The structure for the XML settings should follow the format:
        /// <code>
        /// "adapter": {
        ///   "DownloadUrl": "",
        ///   "FileName": ""
        /// }
        /// </code>
        /// The structure for the adapter data should follow the format:
        /// <code>
        /// "Adapter": {
        ///   "UnityAdapterVersion": "4.3.44.2",
        ///   "AndroidAdapterVersion": "4.3.39",
        ///   "AndroidSDKVersion": "11.10.1",
        ///   "iOSAdapterVersion": "4.3.40.2",
        ///   "iOSSDKVersion": "11.10.1",
        ///   "keyname": "Adapter",
        ///   "isNewProvider": "true"
        /// }
        /// </code>
        /// </summary>
        /// <param name="adapterName">The name of the adapter.</param>
        /// <param name="adapterData">The adapter data structure.</param>
        /// <param name="adapterXML">The XML settings structure.</param>
        /// <returns>An instance of <see cref="Adapter"/> initialized with the provided data and XML settings.</returns>
        public static Adapter CreateAdapter(
            string adapterName,
            Dictionary<string, object> adapterData,
            Dictionary<string, object> adapterXML)
        {
            var latestVersion = GetLatestVersion(adapterData);
            var currentVersion = GetCurrentVersion(GetFileName(adapterXML));
            var downloadURL = GetDownloadURL(adapterXML, latestVersion, currentVersion);
            var isNewAdapter = GetIsNewAdapter(adapterData);
            var isRecommended = GetIsRecommended(adapterName);
            var sdkVersions = GetNativeSDKVersions(adapterData);

            return new Adapter(adapterName,
                latestVersion,
                currentVersion,
                downloadURL,
                isNewAdapter,
                isRecommended,
                sdkVersions);
        }

        static string GetCurrentVersion(string fileName)
        {
            return IntegrationManagerDataUtils.GetVersionFromXMLFile(fileName);
        }

        static string GetDownloadURL(
            IReadOnlyDictionary<string, object> adapterXML,
            string latestVersion,
            string currentVersion)
        {
            var downloadURL = string.Empty;
            if (adapterXML.TryGetValue(k_AdapterDownloadUrl, out var obj))
            {
                downloadURL = obj as string;
            }

            downloadURL = IntegrationManagerDataUtils.ReplaceValue(downloadURL, k_UnityAdapterMacro,
                latestVersion, currentVersion);

            return downloadURL;
        }

        static bool GetIsNewAdapter(IReadOnlyDictionary<string, object> adapterData)
        {
            if (adapterData.TryGetValue(k_AdapterIsNew, out var obj))
            {
                return bool.TryParse(obj as string, out var isNewAdapter) && isNewAdapter;
            }

            return false;
        }

        static bool GetIsRecommended(string adapterName)
        {
            return Array.Exists(k_RecommendedAdapters, name => name == adapterName);
        }

        static string GetLatestVersion(IReadOnlyDictionary<string, object> adapterData)
        {
            if (adapterData.TryGetValue(k_AdapterUnityAdapterVersion, out var obj))
            {
                return obj as string;
            }

            return string.Empty;
        }

        static string GetFileName(IReadOnlyDictionary<string, object> adapterXML)
        {
            if (adapterXML.TryGetValue(k_AdapterFileName, out var obj))
            {
                return obj as string;
            }

            return string.Empty;
        }

        static Dictionary<string, string> GetNativeSDKVersions(IReadOnlyDictionary<string, object> adapterData)
        {
            var nativeSDKVersions = new Dictionary<string, string>();

            if (adapterData.TryGetValue(k_AdapterAndroidSdkVer,
                    out var androidSDKVersionObj))
            {
                var androidSDKVersion = androidSDKVersionObj as string;
                nativeSDKVersions.Add(k_Android, androidSDKVersion);
            }

            if (adapterData.TryGetValue(k_AdapterIosSdkVer,
                    out var iosSDKVersionObj))
            {
                var iosSDKVersion = iosSDKVersionObj as string;
                nativeSDKVersions.Add(k_IOS, iosSDKVersion);
            }

            return nativeSDKVersions;
        }
    }
}
