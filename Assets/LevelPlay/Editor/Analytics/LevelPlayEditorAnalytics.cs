using System;
using System.Reflection;

namespace Unity.Services.LevelPlay.Editor.Analytics
{
    class LevelPlayEditorAnalytics
    {
        const string k_EventName = "editorgameserviceeditor";

        static LevelPlayEditorAnalytics s_Instance;
        LevelPlayEditorAnalytics() {}

        public static LevelPlayEditorAnalytics Instance
        {
            get
            {
                return s_Instance = s_Instance ?? new LevelPlayEditorAnalytics();
            }
        }

        static IEditorAnalyticsSender _sEditorAnalyticsSender;

        internal static IEditorAnalyticsSender EditorAnalyticsSender
        {
            private get => _sEditorAnalyticsSender = _sEditorAnalyticsSender ?? new EditorAnalyticsSender();
            set => _sEditorAnalyticsSender = value;
        }

        FieldInfo packageVersionField;

        public string packageVersion
        {
            get
            {
                PopulatePackageVersionFieldIfNeeded();
                return (string)packageVersionField?.GetValue(null);
            }
        }

        void PopulatePackageVersionFieldIfNeeded()
        {
            if (packageVersionField == null)
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                foreach (var loadedAssembly in loadedAssemblies)
                {
                    if (loadedAssembly.GetName().Name == "Unity.LevelPlay")
                    {
                        packageVersionField = loadedAssembly.GetType("IronSource")
                            ?.GetField("UNITY_PLUGIN_VERSION", BindingFlags.Static | BindingFlags.Public);

                        break;
                    }
                }
            }
        }

        internal void SendEventEditorClick(string component, string action)
        {
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
            EditorAnalyticsSender.SendEvent(k_EventName,
                new EventBody()
                {
                    component = component,
                    action = action,
                    package = LevelPlayIdentifier.Key,
                    package_ver = packageVersion
                });
#endif
        }

        internal void SendInstallAdapterEvent(string action, string adapterName,
            string versionUpdated)
        {
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
            EditorAnalyticsSender.SendEvent(k_EventName,
                new EventBody()
                {
                    component = LevelPlayComponent.LevelPlayNetworkManager,
                    action = action + "_" + adapterName.Replace("_", "-") + "_" + versionUpdated,
                    package = LevelPlayIdentifier.Key,
                    package_ver = packageVersion
                });
#endif
        }

        internal void SendNewSession(string packageType)
        {
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
            EditorAnalyticsSender.SendEvent(k_EventName,
                new EventBody
                {
                    component = packageType,
                    action = LevelPlayAction.NewSession,
                    package = LevelPlayIdentifier.Key,
                    package_ver = packageVersion
                });
#endif
        }

        internal void SendInstallPackage(string component)
        {
#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && LEVELPLAY_DEPENDENCIES_INSTALLED
            EditorAnalyticsSender.SendEvent(k_EventName,
                new EventBody
                {
                    action = LevelPlayAction.Install,
                    component = component,
                    package = LevelPlayIdentifier.Key,
                    package_ver = packageVersion
                });
#endif
        }

        internal static class LevelPlayComponent
        {
            public const string TopMenuAdsMediation = "TopMenu_AdsMediation";
            public const string TopMenuDeveloperSettings = "TopMenu_DeveloperSettings";
            public const string LevelPlayNetworkManager = "LevelPlay_Network_Manager";

            public const string UpmPackage = "upm";
            public const string UnityPackage = ".unitypackage";
        }

        internal static class LevelPlayAction
        {
            public const string OpenChangelog = "Open_SDKChangelog";
            public const string OpenLevelPlayMediationSettings = "Open_LevelPlayMediationSettings";
            public const string OpenMediatedNetworkSettings = "Open_MediatedNetworkSettings";
            public const string OpenNetworkManager = "Open_NetworkManager";
            public const string OpenDocumentation = "Open_Documentation";
            public const string OpenTroubleShootingGuide = "Open_TroubleShootingGuide";

            public const string ClickButton_UpdatePackage = "ClickButton_UpdatePackage";

            public const string NewSession = "NewSession";
            public const string Install = "Install";
            public const string Update = "Update";
        }
    }

    [Serializable]
    struct EventBody
    {
        public string action;
        public string component;
        public string package;
        public string package_ver;
    }
}
