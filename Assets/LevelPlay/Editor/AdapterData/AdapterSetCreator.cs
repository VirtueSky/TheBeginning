using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Services.LevelPlay.Editor.IntegrationManager;

namespace Unity.Services.LevelPlay.Editor.AdapterData
{
    class AdapterSetCreator : INotifyPropertyChanged
    {
        const string k_UpdateMsg = "UpdateMessage";
        const string k_LatestMsg = "LatestMessage";
        const string k_AdapterUnityAdapterVersion = "UnityAdapterVersion";
        internal const string k_IronSourceAdapterKey = "ironSource";
        internal const string k_LevelPlaySdkXmlKey = "levelplaysdk";

        ICollection<Adapter> m_thirdPartyAdapterSet;
        ICollection<Adapter> m_ironSourceAdapterSet;
        string m_messageData;

        internal ICollection<Adapter> ThirdPartyAdapterSet
        {
            get => m_thirdPartyAdapterSet;
            private set
            {
                m_thirdPartyAdapterSet = value;
                OnPropertyChanged();
            }
        }

        internal ICollection<Adapter> IronSourceAdapterSet
        {
            get => m_ironSourceAdapterSet;
            private set
            {
                m_ironSourceAdapterSet = value;
                OnPropertyChanged();
            }
        }

        internal string MessageData
        {
            get => m_messageData;
            private set
            {
                m_messageData = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AdapterSetCreator()
        {
            ThirdPartyAdapterSet = new SortedSet<Adapter>(new AdapterSetComparor());
            IronSourceAdapterSet = new List<Adapter>();
        }

        public void RefreshThirdPartyAdapterCollection()
        {
            ThirdPartyAdapterSet = new List<Adapter>(ThirdPartyAdapterSet);
        }

        public void CreateAdapterDataFromNativeSDKVersionData(
            string linksJson,
            string sdkInfoJson,
            string latestAvailableNativeVersion,
            string currentlyInstalledNativeIronSourceSDKVersion)
        {
            var sdkInfoParsed =
                IntegrationManagerDataUtils.DeserializeJsonToDictionary(sdkInfoJson, out var sdkInfoDictionary);
            var xmlLinksParsed =
                IntegrationManagerDataUtils.DeserializeJsonToDictionary(linksJson, out var linksDictionary);

            if (!sdkInfoParsed || !xmlLinksParsed)
            {
                throw new Exception("Unable to deserialize IronSource adapter data.");
            }

            var adapterInfoData =
                GetAdapterInfoForVersion(sdkInfoDictionary, currentlyInstalledNativeIronSourceSDKVersion) ??
                GetAdapterInfoForVersion(sdkInfoDictionary, latestAvailableNativeVersion);

            if (adapterInfoData == null)
            {
                throw new Exception(
                    "Unable to retrieve adapter data for currently installed or latest IronSource SDK versions.");
            }

            IronSourceAdapterSet = CreateFirstPartyAdapterSet(linksDictionary, adapterInfoData,
                latestAvailableNativeVersion);
            ThirdPartyAdapterSet = CreateThirdPartyAdapterSets(linksDictionary, adapterInfoData);
            MessageData = latestAvailableNativeVersion != currentlyInstalledNativeIronSourceSDKVersion
                ? GetMessageFromSDKInfo(sdkInfoDictionary, k_UpdateMsg)
                : GetMessageFromSDKInfo(sdkInfoDictionary, k_LatestMsg);
        }

        internal ICollection<Adapter> CreateFirstPartyAdapterSet(
            [NotNull] IReadOnlyDictionary<string, object> linksDictionary,
            [NotNull] IReadOnlyDictionary<string, object> adapterInfoData,
            [NotNull] string latestNativeVersion)
        {
            var primaryAdapterSet = new List<Adapter>();

            if (adapterInfoData.TryGetValue(k_IronSourceAdapterKey, out var ironSourceAdapterValue) &&
                linksDictionary.TryGetValue(k_LevelPlaySdkXmlKey, out var ironSourceLinksValue) &&
                ironSourceAdapterValue is Dictionary<string, object> ironSourceVersions &&
                ironSourceLinksValue is Dictionary<string, object> ironSourceXMLObject)
            {
                // This must be set to tell adapter info the latest native version in the current package range
                // otherwise, it will pick the latest version from the xml file which is not necessarily in the package range
                ironSourceVersions[k_AdapterUnityAdapterVersion] = latestNativeVersion;

                var info = AdapterFactory.CreateAdapter(
                    k_IronSourceAdapterKey,
                    ironSourceVersions,
                    ironSourceXMLObject);
                primaryAdapterSet.Add(info);
            }
            else
            {
                throw new Exception(
                    $"ironSource Ads key '{k_LevelPlaySdkXmlKey}' is unavailable.");
            }

            return primaryAdapterSet;
        }

        internal ICollection<Adapter> CreateThirdPartyAdapterSets(
            [NotNull] IReadOnlyDictionary<string, object> linksDictionary,
            [NotNull] IReadOnlyDictionary<string, object> adapterInfoData)
        {
            var thirdPartyAdaptersSet = new SortedSet<Adapter>(new AdapterSetComparor());

            foreach (var adapter in adapterInfoData)
            {
                var sdkLinksItemKey = adapter.Key.ToLower(new System.Globalization.CultureInfo("en"));
                if (adapter.Key != k_IronSourceAdapterKey &&
                    adapter.Value is Dictionary<string, object> adapterVersionInfo &&
                    linksDictionary.ContainsKey(sdkLinksItemKey) &&
                    linksDictionary[sdkLinksItemKey] is Dictionary<string, object> adapterXMLObject)
                {
                    var info = AdapterFactory.CreateAdapter(
                        adapter.Key,
                        adapterVersionInfo,
                        adapterXMLObject);
                    thirdPartyAdaptersSet.Add(info);
                }
            }

            return thirdPartyAdaptersSet;
        }

        [CanBeNull]
        static Dictionary<string, object> GetAdapterInfoForVersion(
            [NotNull] IReadOnlyDictionary<string, object> sdkInfoDictionary,
            string ironSourceSDKVersionOnFile)
        {
            if (!sdkInfoDictionary.TryGetValue(ironSourceSDKVersionOnFile, out var adapterInfo) || adapterInfo == null)
            {
                return null;
            }

            return adapterInfo as Dictionary<string, object>;
        }

        static string GetMessageFromSDKInfo(IReadOnlyDictionary<string, object> sdkInfoDictionary,
            string messageType)
        {
            return sdkInfoDictionary.TryGetValue(messageType, out var latestMsgJson)
                ? latestMsgJson.ToString()
                : string.Empty;
        }
    }
}
