using System;
using UnityEngine;
using VirtueSky.Events;

namespace TheBeginning.Custom_Scriptable_Event
{
    [CreateAssetMenu(menuName = "Event Custom/Tracking Firebase Has Param Event",
        fileName = "tracking_firebase_has_param_event")]
    public class TrackingFirebaseHasParamEvent : BaseEvent<TrackingFirebaseEventHasParamData>
    {
    }

    [Serializable]
    public class TrackingFirebaseEventHasParamData
    {
        public string eventName;
        public string parameterName;
        public string parameterValue;

        public TrackingFirebaseEventHasParamData(string eventName, string parameterName,
            string parameterValue)
        {
            this.eventName = eventName;
            this.parameterName = parameterName;
            this.parameterValue = parameterValue;
        }
    }
}