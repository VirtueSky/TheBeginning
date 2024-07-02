using System;
using System.Linq;

namespace com.unity3d.mediation
{
    /// <summary>
    /// Manages initialization and basic operations of the LevelPlay SDK.
    /// This class provides methods to initialize the SDK and handles global events for initialization success and failure.
    /// </summary>
    public class LevelPlay
    {
        static event Action<LevelPlayConfiguration> InitSuccessReceived;
        static event Action<LevelPlayInitError> OnInitFailedReceived;

        /// <summary>
        /// Adds or removes event handlers for the SDK initialization success event.
        /// Ensures that the same handler cannot be added multiple times.s
        /// </summary>
        public static event Action<LevelPlayConfiguration> OnInitSuccess
        {
            add
            {
                if (InitSuccessReceived == null || !InitSuccessReceived.GetInvocationList().Contains(value))
                {
                    InitSuccessReceived += value;
                }
            }

            remove
            {
                if (InitSuccessReceived != null && InitSuccessReceived.GetInvocationList().Contains(value))
                {
                    InitSuccessReceived -= value;
                }
            }
        }

        /// <summary>
        /// Adds or removes event handlers for the SDK initialization failure event.
        /// Ensures that the same handler cannot be added multiple times.
        /// </summary>
        public static event Action<LevelPlayInitError> OnInitFailed
        {
            add
            {
                if (OnInitFailedReceived == null || !OnInitFailedReceived.GetInvocationList().Contains(value))
                {
                    OnInitFailedReceived += value;
                }
            }

            remove
            {
                if (OnInitFailedReceived != null && OnInitFailedReceived.GetInvocationList().Contains(value))
                {
                    OnInitFailedReceived -= value;
                }
            }
        }

        /// <summary>
        /// Static constructor to hook up platform-specific initialization callbacks.
        /// </summary>
        static LevelPlay()
        {
#if UNITY_ANDROID
            AndroidLevelPlaySdk.OnInitSuccess += (configuration) =>
            {
                InitSuccessReceived?.Invoke(configuration);
            };
            AndroidLevelPlaySdk.OnInitFailed += (error) =>
            {
                OnInitFailedReceived?.Invoke(error);
            };
#elif UNITY_IOS
            IosLevelPlaySdk.OnInitSuccess += (configuration) =>
            {
                InitSuccessReceived?.Invoke(configuration);
            };
            IosLevelPlaySdk.OnInitFailed += (error) =>
            {
                OnInitFailedReceived?.Invoke(error);
            };
#endif
        }

        /// <summary>
        /// Initializes the LevelPlay SDK with the specified app key and optional user ID and ad format list.
        /// </summary>
        /// <param name="appKey">The application key for the SDK.</param>
        /// <param name="userId">Optional user identifier for use within the SDK.</param>
        /// <param name="adFormats">Optional array of ad formats to initialize.</param>
        public static void Init(string appKey, string userId = null, LevelPlayAdFormat[] adFormats = null)
        {
#if UNITY_ANDROID
            AndroidLevelPlaySdk.Initialize(appKey, userId, adFormats);
#elif UNITY_IOS
            IosLevelPlaySdk.Initialize(appKey, userId, adFormats);
#endif
        }
    }
}
