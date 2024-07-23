using System.Collections.Generic;
using UnityEngine;

namespace com.unity3d.mediation
{
    /// <summary>
    /// Represents the configuration settings for the LevelPlay mediation platform.
    /// </summary>
    public class LevelPlayConfiguration
    {

        /// <summary>
        /// Indicates whether ad quality control is enabled.
        /// </summary>
        public bool IsAdQualityEnabled { get; }

        internal LevelPlayConfiguration(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            try {
                object jsonObject = JsonUtility.FromJson<object>(json);
                if (jsonObject is Dictionary<string, object> dictionary)
                {
                    if (dictionary.ContainsKey("isAdQualityEnabled"))
                    {
                        IsAdQualityEnabled = (bool)dictionary["isAdQualityEnabled"];
                    }
                }
            } catch (System.Exception e) {
                Debug.LogError("Failed to parse LevelPlayConfiguration: " + e.Message);
            }
        }
    }
}
