using System.IO;
using Unity.Services.LevelPlay.Editor.AdapterData;
using Unity.Services.LevelPlay.Editor.IntegrationManager;
using UnityEditor;

namespace Unity.Services.LevelPlay.Editor
{
    class DependenciesXmlFileMigrator
    {
        const string k_AssetsEditorPath = "Assets/LevelPlay/Editor";
        const string k_IronSourceSdkFileName = "IronSourceSDKDependencies.xml";
        const string k_MsgSdkDependenciesXmlUpdated = "The {0} file was updated to {1}.";
        const string k_IronSourceDependenciesVersion = "8.1.0";
        const string k_IronSourceDependenciesUrl = "https://s3.amazonaws.com/ssa.public/Dependencies-xmls/IronSource/{0}/IronSourceSDKDependencies.xml";

        [InitializeOnLoadMethod]
        static void EnsureSdkDependenciesXmlFileExistsInAssetsFolder()
        {
            if (IsDownloadIronSourceDependenciesXmlRequired(
                    Path.Combine(k_AssetsEditorPath, k_IronSourceSdkFileName),
                    IntegrationManagerDataUtils.GetVersionFromXMLFile(k_IronSourceSdkFileName),
                    k_IronSourceDependenciesVersion))
            {
                DownloadIronSourceDependenciesXml();
            }
        }

        internal static bool IsDownloadIronSourceDependenciesXmlRequired(string ironSourceDependenciesXmlFilePath,
            string currentVersion, string latestVersion)
        {
            return !File.Exists(ironSourceDependenciesXmlFilePath)
                   || Adapter.IsLatestVersionNewerThanCurrentVersion(currentVersion, latestVersion);
        }

        static void DownloadIronSourceDependenciesXml()
        {
#if LEVELPLAY_DEPENDENCIES_INSTALLED
            var integrationManagerDownloader = new IntegrationManagerDownloader();
            integrationManagerDownloader.DownloadAdapterXML(
                string.Format(k_IronSourceDependenciesUrl, k_IronSourceDependenciesVersion),
                (version, file) =>
                {
                    AssetDatabase.Refresh();
                    LevelPlayLogger.Log(string.Format(k_MsgSdkDependenciesXmlUpdated, file, version));
                });
#endif
        }
    }
}
