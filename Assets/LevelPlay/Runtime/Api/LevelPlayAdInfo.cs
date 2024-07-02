using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

namespace com.unity3d.mediation
{
    /// <summary>
    /// Contains detailed information about a LevelPlay advertisement, including its dimensions, placement, and performance metrics.
    /// </summary>
    public class LevelPlayAdInfo
    {
        // Constants for JSON keys
        const string AdUnitIdKey = "adUnitId";
        const string AdSizeKey = "adSize";
        const string AdFormatKey = "adFormat";
        const string PlacementNameKey = "placementName";
        const string AuctionIdKey = "auctionId";
        const string CountryKey = "country";
        const string AbKey = "ab";
        const string SegmentNameKey = "segmentName";
        const string AdNetworkKey = "adNetwork";
        const string InstanceNameKey = "instanceName";
        const string InstanceIdKey = "instanceId";
        const string RevenueKey = "revenue";
        const string PrecisionKey = "precision";
        const string EncryptedCpmKey = "encryptedCPM";

        const string AdSizeDescriptionKey = "description";
        const string AdSizeWidthKey = "width";
        const string AdSizeHeightKey = "height";

        // Properties
        public readonly string adUnitId;
        [CanBeNull] public readonly LevelPlayAdSize adSize;
        public readonly string adFormat;
        public readonly string placementName;
        public readonly string auctionId;
        public readonly string country;
        public readonly string ab;
        public readonly string segmentName;
        public readonly string adNetwork;
        public readonly string instanceName;
        public readonly string instanceId;
        public readonly double? revenue;
        public readonly string precision;
        public readonly string encryptedCPM;

        internal LevelPlayAdInfo(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            object obj;
            double parsedDouble;
            try
            {
                CultureInfo invCulture = CultureInfo.InvariantCulture;
                Dictionary<string, object> jsonDic =
                    IronSourceJSON.Json.Deserialize(json) as Dictionary<string, object>;
                if (jsonDic.TryGetValue(AdUnitIdKey, out obj) && obj != null)
                {
                    adUnitId = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdSizeKey, out obj) && obj != null)
                {
                    adSize = GetAdSize(obj.ToString());
                }

                if (jsonDic.TryGetValue(AdFormatKey, out obj) && obj != null)
                {
                    adFormat = obj.ToString();
                }

                if (jsonDic.TryGetValue(PlacementNameKey, out obj) && obj != null)
                {
                    placementName = obj.ToString();
                }

                if (jsonDic.TryGetValue(AuctionIdKey, out obj) && obj != null)
                {
                    auctionId = obj.ToString();
                }

                if (jsonDic.TryGetValue(CountryKey, out obj) && obj != null)
                {
                    country = obj.ToString();
                }

                if (jsonDic.TryGetValue(AbKey, out obj) && obj != null)
                {
                    ab = obj.ToString();
                }

                if (jsonDic.TryGetValue(SegmentNameKey, out obj) && obj != null)
                {
                    segmentName = obj.ToString();
                }

                if (jsonDic.TryGetValue(AdNetworkKey, out obj) && obj != null)
                {
                    adNetwork = obj.ToString();
                }

                if (jsonDic.TryGetValue(InstanceNameKey, out obj) && obj != null)
                {
                    instanceName = obj.ToString();
                }

                if (jsonDic.TryGetValue(InstanceIdKey, out obj) && obj != null)
                {
                    instanceId = obj.ToString();
                }

                if (jsonDic.TryGetValue(RevenueKey, out obj) && obj != null && double.TryParse(
                        string.Format(invCulture, "{0}", obj), NumberStyles.Any, invCulture, out parsedDouble))
                {
                    revenue = parsedDouble;
                }

                if (jsonDic.TryGetValue(PrecisionKey, out obj) && obj != null)
                {
                    precision = obj.ToString();
                }

                if (jsonDic.TryGetValue(EncryptedCpmKey, out obj) && obj != null)
                {
                    encryptedCPM = obj.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("error parsing LevelPlayAdInfo" + ex.ToString());
            }
        }

        /// <summary>
        /// Retrieves the ad size from a JSON string describing the ad size.
        /// </summary>
        /// <param name="adSizeJson">The JSON string describing the ad size.</param>
        /// <returns>An instance of <see cref="LevelPlayAdSize"/> or null if parsing fails.</returns>
        static LevelPlayAdSize GetAdSize(string adSizeJson)
        {
            string description = "";
            int width = 0;
            int height = 0;
            if (!string.IsNullOrEmpty(adSizeJson))
            {
                try
                {
                    object obj;
                    Dictionary<string, object> jsonDic =
                        IronSourceJSON.Json.Deserialize(adSizeJson) as Dictionary<string, object>;
                    if (jsonDic.TryGetValue(AdSizeDescriptionKey, out obj) && obj != null)
                    {
                        description = obj.ToString();
                    }

                    if (jsonDic.TryGetValue(AdSizeWidthKey, out obj) && obj != null)
                    {
                        width = Int32.Parse(obj.ToString());
                    }

                    if (jsonDic.TryGetValue(AdSizeHeightKey, out obj) && obj != null)
                    {
                        height = Int32.Parse(obj.ToString());
                    }

                    return new LevelPlayAdSize(description, width, height);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }

            Debug.Log("LevelPlayAdInfo GetAdSize json: return null");
            return null;
        }

        public override string ToString()
        {
            return $"adUnitId: {adUnitId}, adSize: {adSize.ToString()}, adFormat: {adFormat}, placementName: {placementName}, auctionId: {auctionId}, country: {country}, ab: {ab}, segmentName: {segmentName}, adNetwork: {adNetwork}, instanceName: {instanceName}, instanceId: {instanceId}, revenue: {revenue}, precision: {precision}, encryptedCPM: {encryptedCPM}";
        }
    }
}
