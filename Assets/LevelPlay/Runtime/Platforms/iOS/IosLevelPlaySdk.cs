#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.unity3d.mediation
{
    public class IosLevelPlaySdk : MonoBehaviour
    {
        public static event Action<LevelPlayConfiguration> OnInitSuccess;
        public static event Action<LevelPlayInitError> OnInitFailed;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        static IosLevelPlaySdk()
        {
        }

        public static void Initialize(string appKey, string userId, LevelPlayAdFormat[] adFormats)
        {

            new GameObject("IosLevelPlaySdk", typeof(IosLevelPlaySdk)).GetComponent<IosLevelPlaySdk>();
            LPMInitialize(appKey, userId, GetAdFormatArray(adFormats));
        }

        private static string[] GetAdFormatArray(LevelPlayAdFormat[] adFormats)
        {
            if (adFormats == null)
            {
                return null;
            }
            var adFormatsArray = new string[adFormats.Length];
            for (var i = 0; i < adFormats.Length; i++)
            {
                LevelPlayAdFormat adFormat = adFormats[i];
                var adFormatString = adFormat switch
                {
                    LevelPlayAdFormat.BANNER => "banner",
                    LevelPlayAdFormat.INTERSTITIAL => "interstitial",
                    LevelPlayAdFormat.REWARDED => "rewardedvideo",
                    _ => ""
                };
                adFormatsArray[i] = adFormatString;
            }
            return adFormatsArray;
        }

        [DllImport("__Internal")]
        private static extern void LPMInitialize(string appKey, string userId, string[] adFormats);

        public void OnInitializationSuccess(string configuration)
        {
            OnInitSuccess?.Invoke(new LevelPlayConfiguration(configuration));
        }

        public void OnInitializationFailed(string error)
        {
            OnInitFailed?.Invoke(new LevelPlayInitError(error));
        }

    }

}
#endif
