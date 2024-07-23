#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Unity.Services.LevelPlay;
using Unity.Services.LevelPlay.Editor;
using Unity.Services.LevelPlay.Editor.AdapterData;
using Unity.Services.LevelPlay.Editor.Analytics;
using Unity.Services.LevelPlay.Editor.IntegrationManager;
using Unity.Services.LevelPlay.Editor.IntegrationManager.UIComponents;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class IronSourceDependenciesManager : EditorWindow
{
    enum PackageType
    {
        None = 0,
        Upm = 1,
        DotUnityPackage = 2
    }

    const string k_IntegrationManagerWindowTitle = "LevelPlay Network Manager";
    const string k_LevelPlayPackageName = "com.unity.services.levelplay";
    const string k_LevelPlayPackageVersion = "8.1.0";
    const string k_AdapterInfoNotAvailable = "Please wait while LevelPlay SDK and adapter information loads.";
    const string k_HeaderUpm = "Unity Package (Ads Mediation UPM Package)";
    const string k_HeaderUnityPackage = "Unity Package (Unity Plugin)";
    const string k_IronSourceXMLFilename = "IronSourceSDKDependencies.xml";
    const string k_IronSourceDownloadDir = "Assets/LevelPlay/Editor/";
    const string k_None = "none";
    const int k_Height = 875;
    internal const int k_Width = 700;

    AdapterSetCreator _adapterSetCreator;
    IntegrationManagerDownloader _integrationManagerDownloader;

    string _currentPackageSDKVersion = k_LevelPlayPackageVersion;
    List<PackageVersionNativeCompatibility> _packageVersions = new List<PackageVersionNativeCompatibility>();
    List<string> _nativeSdkVersions = new List<string>();
    string _sdkInfoData = string.Empty;
    string _sdkLinksData = string.Empty;
    string _latestPackageSDKVersion;
    Texture2D _recommendedIcon;

    UnityPluginComponent _pluginComponent;
    IDrawable _sdkComponent;
    IDrawable _networksComponent;
    IDrawable _messageDisplayComponent;

    // changeable data:
    PackageType _packageType = PackageType.None;

    public static void ShowISDependenciesManager()
    {
        var window = GetWindowWithRect<IronSourceDependenciesManager>(new Rect(0, 0,
            k_Width, k_Height),
            true);
        window.titleContent = new GUIContent(k_IntegrationManagerWindowTitle);
        window.ShowUtility();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    async void OnEnable()
    {
        // Setup Data:
        _adapterSetCreator = new AdapterSetCreator();
        _integrationManagerDownloader = new IntegrationManagerDownloader();

        // Check if package is installed via package manager:
        if (_packageType == PackageType.None)
        {
            LevelPlayPackmanQuerier.instance.CheckIfPackageIsInstalledWithUpm(k_LevelPlayPackageName,
                isInstalledWithUpm =>
                {
                    _packageType = isInstalledWithUpm ? PackageType.Upm : PackageType.DotUnityPackage;
                    _pluginComponent?.UpdateDownloadAction(GetUnityPackageComponentAction(_packageType));
                });
        }

        // Download data:
        _recommendedIcon = await _integrationManagerDownloader.GetRecommendedIcon();
        (_sdkInfoData, _sdkLinksData, _packageVersions) = await _integrationManagerDownloader.DownloadSDKInfoData();
        _latestPackageSDKVersion = SdkRangeFinder.GetLatestPackageSDK(_packageVersions).PackageVersion;
        _nativeSdkVersions = IntegrationManagerDataUtils.GetNativeSDKVersions(_sdkInfoData);

        // Bind UI and data:
        RefreshAdapterData();
        SetUpUIComponents();
        _adapterSetCreator.PropertyChanged += BindUIComponentsToDataCreator;
    }

    void OnGUI()
    {
        if (_packageType == PackageType.None ||
            _adapterSetCreator.ThirdPartyAdapterSet.Count == 0 ||
            _adapterSetCreator.IronSourceAdapterSet.Count == 0)
        {
            GUILayout.Label(k_AdapterInfoNotAvailable);
            return;
        }

        _pluginComponent.Draw();
        _sdkComponent.Draw();
        _networksComponent.Draw();
        _messageDisplayComponent.Draw();
    }

    void OnDestroy()
    {
        AssetDatabase.Refresh();
        _integrationManagerDownloader.CancelDownloads();
    }

    void RefreshAdapterData()
    {
        // Resetting this allows the UI to update when a user downloads a new package
        _currentPackageSDKVersion = k_LevelPlayPackageVersion;
        var currentlyInstalledPackageSDK =
            _packageVersions.FirstOrDefault(info => info.PackageVersion == _currentPackageSDKVersion);

        if (string.IsNullOrEmpty(currentlyInstalledPackageSDK.CompatibleNativeVersionRange))
        {
            LevelPlayLogger.LogWarning(
                $"Ads Mediation version {_currentPackageSDKVersion}: " +
                $"Your currently installed Ads Mediation package version " +
                $"has no compatible ironSource native sdk. You will be unable to manage adapters. " +
                $"In the meantime, install another version of the Ads Mediation Package.");
            return;
        }

        var latestNativeSDKVersionForCurrentlyInstalledPackage =
            SdkRangeFinder.GetLatestNativeSDKInRange(currentlyInstalledPackageSDK, _nativeSdkVersions);

        var currentlyInstalledNativeIronSourceSDKVersion =
            File.Exists(Path.Combine(k_IronSourceDownloadDir, k_IronSourceXMLFilename))
                ? IntegrationManagerDataUtils.GetVersionFromXMLFile(k_IronSourceXMLFilename)
                : k_None;

        try
        {
            _adapterSetCreator.CreateAdapterDataFromNativeSDKVersionData(
                _sdkLinksData,
                _sdkInfoData,
                latestNativeSDKVersionForCurrentlyInstalledPackage,
                currentlyInstalledNativeIronSourceSDKVersion);
        }
        catch (Exception e)
        {
            LevelPlayLogger.LogWarning(
                $"AdapterCreationError: Ads Mediation {_currentPackageSDKVersion}: {e} ");
        }
        finally
        {
            Repaint();
        }
    }

    void BindUIComponentsToDataCreator(object sender, PropertyChangedEventArgs e)
    {
        SetUpUIComponents();
    }

    void SetUpUIComponents()
    {
        _pluginComponent = new UnityPluginComponent(
            GetUnityPackageComponentAction(_packageType),
            _currentPackageSDKVersion,
            _latestPackageSDKVersion);

        _sdkComponent = new LevelPlaySDKComponent(
            _adapterSetCreator.IronSourceAdapterSet,
            onNetworkButtonClick: DownloadAdapterNetworkAction);

        _networksComponent = new NetworksComponent(
            _recommendedIcon,
            _adapterSetCreator.ThirdPartyAdapterSet,
            onNetworkButtonClick: DownloadAdapterNetworkAction);

        _messageDisplayComponent = new MessageDisplayComponent(_adapterSetCreator.MessageData);
    }

    (string headerTitle, Action DownloadAction) GetUnityPackageComponentAction(PackageType unityPackageType)
    {
        string pluginHeaderText;
        Action unityPackageDownloadAction;

        switch (unityPackageType)
        {
            case PackageType.DotUnityPackage:
                pluginHeaderText = k_HeaderUnityPackage;
                unityPackageDownloadAction = () => DownloadUnityPackageAction(_latestPackageSDKVersion);
                break;
            case PackageType.Upm:
                pluginHeaderText = k_HeaderUpm;
                unityPackageDownloadAction = DownloadUpmPackmanPackageAction;
                break;
            case PackageType.None:
            default:
                pluginHeaderText = string.Empty;
                unityPackageDownloadAction = () => {};
                break;
        }

        return (pluginHeaderText, unityPackageDownloadAction);
    }

    void DownloadUnityPackageAction(string latestRemotePackageVersion)
    {
        LevelPlayEditorAnalytics.Instance.SendEventEditorClick(
            LevelPlayEditorAnalytics.LevelPlayComponent.LevelPlayNetworkManager,
            LevelPlayEditorAnalytics.LevelPlayAction.ClickButton_UpdatePackage + "_DotUnityPackage");

        _integrationManagerDownloader.DownloadUnityPackage(
            latestRemotePackageVersion, () => {});
    }

    void DownloadUpmPackmanPackageAction()
    {
        LevelPlayEditorAnalytics.Instance.SendEventEditorClick(
            LevelPlayEditorAnalytics.LevelPlayComponent.LevelPlayNetworkManager,
            LevelPlayEditorAnalytics.LevelPlayAction.ClickButton_UpdatePackage + "_UPM");

        Client.Add(k_LevelPlayPackageName);
        RefreshAdapterData();
    }

    void DownloadAdapterNetworkAction(Adapter adapter)
    {
        LevelPlayEditorAnalytics.Instance.SendInstallAdapterEvent(
            adapter.CurrentStatus == Adapter.Status.NotInstalled ?
            LevelPlayEditorAnalytics.LevelPlayAction.Install : LevelPlayEditorAnalytics.LevelPlayAction.Update,
            adapter.AdapterName, adapter.LatestVersion);

        _integrationManagerDownloader.DownloadAdapterXML(
            adapter.DownloadURL, (newVersion, fileName) =>
            {
                if (adapter.AdapterName == AdapterSetCreator.k_IronSourceAdapterKey)
                {
                    RefreshAdapterData();
                }
                else
                {
                    adapter.UpdateStatusWithVersion(newVersion);
                    adapter.UpdateCurrentVersion(newVersion);
                    _adapterSetCreator.RefreshThirdPartyAdapterCollection();
                }
                Repaint();
                AssetDatabase.ImportAsset(Path.Combine(k_IronSourceDownloadDir, fileName));
            });
    }

    [Obsolete("ProviderInfo has been deprecated without an equivalent API.")]
    public class ProviderInfo
    {
        [Obsolete("currentStatues has been deprecated without an equivalent API.")] public Status currentStatues;
        [Obsolete("providerName has been deprecated without an equivalent API.")] public string providerName;
        [Obsolete("currentUnityVersion has been deprecated without an equivalent API.")] public string currentUnityVersion;
        [Obsolete("latestUnityVersion has been deprecated without an equivalent API.")] public string latestUnityVersion;
        [Obsolete("latestUnityAdsVersion has been deprecated without an equivalent API.")] public string latestUnityAdsVersion;
        [Obsolete("downloadURL has been deprecated without an equivalent API.")] public string downloadURL;
        [Obsolete("displayProviderName has been deprecated without an equivalent API.")] public string displayProviderName;
        [Obsolete("isNewProvider has been deprecated without an equivalent API.")] public bool isNewProvider;
        [Obsolete("fileName has been deprecated without an equivalent API.")] public string fileName;
        [Obsolete("sdkVersionDic has been deprecated without an equivalent API.")] public Dictionary<string, string> sdkVersionDic;

        [Obsolete("ProviderInfo has been deprecated without an equivalent API.")]
        public ProviderInfo()
        {
            isNewProvider = false;
            fileName = string.Empty;
            downloadURL = string.Empty;
            currentUnityVersion = IronSourceDependenciesManagerConstants.NONE;
            sdkVersionDic = new Dictionary<string, string>();
        }

        [Obsolete("Status has been deprecated without an equivalent API.")]
        public enum Status
        {
            INSTALLED = 1,
            NONE = 2,
            UPDATED = 3
        }

        [Obsolete("SetProviderDataProperties has been deprecated without an equivalent API.")]
        public bool SetProviderDataProperties(string provider, Dictionary<string, object> providerData, Dictionary<string, object> providerXML)
        {
            providerName = provider;
            object obj;

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_KEY_NAME, out obj))
            {
                displayProviderName = obj as string;
            }
            else { displayProviderName = providerName; }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_IS_NEW, out obj))
            {
                isNewProvider = bool.Parse(obj as string);
            }

            if (providerXML.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_DOWNLOAD_URL, out obj))
            {
                downloadURL = obj as string;
            }

            if (providerXML.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_FILE_NAME, out obj))
            {
                fileName = obj as string;
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_ANDROID_SDK_VER, out obj))
            {
                sdkVersionDic.Add(IronSourceDependenciesManagerConstants.ANDROID, obj as string);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_IOS_SDK_VER, out obj))
            {
                sdkVersionDic.Add(IronSourceDependenciesManagerConstants.IOS, obj as string);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_UNITY_ADAPTER_VERSION, out obj))
            {
                if ((providerName.ToLower() != IronSourceDependenciesManagerConstants.IRONSOURCE))
                {
                    latestUnityVersion = obj as string;
                }
                else
                {
                    latestUnityVersion = string.Empty;
                }

                downloadURL = downloadURL.Replace(IronSourceDependenciesManagerConstants.UNITY_ADAPTER_MACRO, latestUnityVersion);
            }

            if (providerData.TryGetValue(IronSourceDependenciesManagerConstants.PROVIDER_UNITY_ADAPTER_VERSION, out obj))
            {
                if ((providerName.ToLower() == IronSourceDependenciesManagerConstants.UNITYADS))
                {
                    latestUnityAdsVersion = obj as string;
                }


                downloadURL = downloadURL.Replace(IronSourceDependenciesManagerConstants.UNITY_ADAPTER_MACRO, latestUnityVersion);
            }

            currentUnityVersion = string.Empty;

            if (currentUnityVersion.Equals(IronSourceDependenciesManagerConstants.NONE))
            {
                currentStatues = Status.NONE;
            }
            else
            {
                currentStatues = Status.UPDATED;
            }

            return true;
        }
    }
}
#endif
