using System;
using UnityEngine;

namespace TheBeginning.AppControl
{
    public struct AppControlAds
    {
        private static AdsManager _adsManager;

        public static void Init(AdsManager adsManager)
        {
            if (_adsManager != null)
            {
                UnityEngine.Object.Destroy(_adsManager);
            }

            AppControlAds._adsManager = adsManager;
        }

        public static void ShowInterstitial(Action completeCallback = null, Action displayCallback = null)
        {
            if (_adsManager == null)
            {
                Debug.LogError("Please Init AppControlAds before use");
                return;
            }

            _adsManager.ShowInterstitial(completeCallback, displayCallback);
        }

        public static void ShowReward(Action completeCallback = null, Action displayCallback = null,
            Action closeCallback = null, Action skipCallback = null)
        {
            if (_adsManager == null)
            {
                Debug.LogError("Please Init AppControlAds before use");
                return;
            }

            _adsManager.ShowRewardAds(completeCallback, displayCallback, closeCallback, skipCallback);
        }

        public static void ShowBanner()
        {
            if (_adsManager == null)
            {
                Debug.LogError("Please Init AppControlAds before use");
                return;
            }

            _adsManager.ShowBanner();
        }

        public static void HideBanner()
        {
            if (_adsManager == null)
            {
                Debug.LogError("Please Init AppControlAds before use");
                return;
            }

            _adsManager.HideBanner();
        }

        public static bool IsRewardReady()
        {
            if (_adsManager == null)
            {
                Debug.LogError("Please Init AppControlAds before use");
                return false;
            }

            return _adsManager.IsRewardReady();
        }
    }
}