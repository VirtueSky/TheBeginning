using System.Collections.Generic;

/// <summary>
/// LevelPlay's API Access point
/// </summary>
public interface IronSourceIAgent
{
    //******************* Base API *******************//

    /// <summary>
    /// Allows publishers to set configurations for a waterfall of a given ad type.
    /// </summary>
    /// <param name="waterfallConfiguration">The configuration for the given ad types waterfall. </param>
    /// <param name="adFormat">The AdFormat for which to configure the waterfall.</param>
    void SetWaterfallConfiguration(WaterfallConfiguration waterfallConfiguration, AdFormat adFormat);

    /// <summary>
    /// Notifies the LevelPlay SDK that the application has been paused or unpaused.
    /// </summary>
    /// <param name="pause">Is the application paused</param>
    void onApplicationPause(bool pause);

    /// <summary>
    /// Returns the advertiser ID.
    /// </summary>
    /// <returns>Advertiser ID</returns>
    string getAdvertiserId();

    /// <summary>
    /// Runs the integration validation.
    /// </summary>
    void validateIntegration();

    /// <summary>
    /// Enable or disable tracking of the network state.
    /// By default this is set to false.
    /// </summary>
    /// <param name="track">Should the network state be tracked</param>
    void shouldTrackNetworkState(bool track);

    /// <summary>
    /// Sets a dynamic user ID that can be changed through the session and will be used in server to server rewarded
    /// ad callbacks.
    /// This parameter helps verify AdRewarded transactions and must be set before calling ShowRewardedVideo.
    /// </summary>
    /// <param name="dynamicUserId">The ID to be set</param>
    /// <returns>Was the dynamic user ID set successfully</returns>
    bool setDynamicUserId(string dynamicUserId);

    /// <summary>
    /// Enables or disables adapters debug info.
    /// </summary>
    /// <param name="enabled">Is adapters debug info enabled</param>
    void setAdaptersDebug(bool enabled);

    /// <summary>
    /// Allows setting extra flags, for example "do_not_sell" to allow or disallow selling or sharing personal information.
    /// </summary>
    /// <param name="key">The flag to set</param>
    /// <param name="value">the value for the flag</param>
    void setMetaData(string key, string value);

    /// <summary>
    /// Allows setting extra flags, for example "do_not_sell" to allow or disallow selling or sharing personal information.
    /// </summary>
    /// <param name="key">The flag to set</param>
    /// <param name="values">the values for the flag</param>
    void setMetaData(string key, params string[] values);

    /// <summary>
    /// Returns the conversion value or CV.
    /// </summary>
    /// <returns>Conversion value</returns>
    int? getConversionValue();

    /// <summary>
    /// Toggles manual loading of rewarded videos.
    /// </summary>
    /// <param name="isOn">Are rewarded videos loaded manually.</param>
    void setManualLoadRewardedVideo(bool isOn);

    /// <summary>
    /// Set custom network data.
    /// </summary>
    /// <param name="networkKey">The attribute key</param>
    /// <param name="networkData">The attribute value</param>
    void setNetworkData(string networkKey, string networkData);

    /// <summary>
    /// When setting your PauseGame status to true, all your Unity 3D game activities will be paused (Except the ad callbacks).
    /// The game activity will be resumed automatically when the ad is closed.
    /// You should call the setPauseGame once in your session, before or after initializing the ironSource SDK,
    /// as it affects all ads (Rewarded Video and Interstitial ads) in the session.
    /// </summary>
    /// <param name="pause">Is the game paused</param>
    void SetPauseGame(bool pause);

    //******************* SDK Init *******************//

    /// <summary>
    /// Sets the user's identifier.
    /// If you’re serving the offerwall ad unit or using server-to-server callbacks to reward your users with ironSource
    /// SDK rewarded ad units, you must define a unique identifier for each user using this method.
    /// Set the UserID before the init request, to make sure you avoid any data loses, related to the user.
    /// Use a unique identifier, with up to 64 alphanumeric characters.
    /// </summary>
    /// <param name="userId">User's ID</param>
    void setUserId(string userId);

    /// <summary>
    /// Initialize the LevelPlay SDK.
    /// </summary>
    /// <param name="appKey">Application key</param>
    void init(string appKey);

    /// <summary>
    /// Initialize the LevelPlay SDK for specific ad unit types.
    /// For example IronSourceAdUnits.REWARDED_VIDEO would initialize for rewarded videos.
    /// </summary>
    /// <param name="appKey">Application key</param>
    /// <param name="adUnits">List of ad unit types to initialize</param>
    void init(string appKey, params string[] adUnits);

    //******************* RewardedVideo API *******************//

    /// <summary>
    /// Load a Rewarded video.
    /// </summary>
    void loadRewardedVideo();

    /// <summary>
    /// Show a rewarded video
    /// </summary>
    void showRewardedVideo();

    /// <summary>
    /// Show a rewarded video with a specific placement.
    /// </summary>
    /// <param name="placementName">Name of the placement</param>
    void showRewardedVideo(string placementName);

    /// <summary>
    /// Returns the availability of a rewarded video.
    /// </summary>
    /// <returns>Is a rewarded video available</returns>
    bool isRewardedVideoAvailable();

    /// <summary>
    /// Determines if a rewarded video placement is capped.
    /// </summary>
    /// <param name="placementName">Name of the placement</param>
    /// <returns>Is the placement capped</returns>
    bool isRewardedVideoPlacementCapped(string placementName);

    /// <summary>
    /// Retrieve a placement by name.
    /// </summary>
    /// <param name="placementName">Placement's name</param>
    /// <returns>An IronSourcePlacement containing the placement infos</returns>
    IronSourcePlacement getPlacementInfo(string name);

    /// <summary>
    /// Set custom parameters for server-to-server completion callbacks.
    /// </summary>
    /// <param name="parameters">Key-Value list of parameters</param>
    void setRewardedVideoServerParams(Dictionary<string, string> parameters);

    /// <summary>
    /// Clears custom parameters for server-to-server completion callbacks.
    /// </summary>
    void clearRewardedVideoServerParams();

    //******************* Interstitial API *******************//

    /// <summary>
    /// Load an interstitial video
    /// </summary>
    void loadInterstitial();

    /// <summary>
    /// Show an interstitial video
    /// </summary>
    void showInterstitial();

    /// <summary>
    /// Show an interstitial video for a specific placement.
    /// </summary>
    /// <param name="placementName">Name of the placement</param>
    void showInterstitial(string placementName);

    /// <summary>
    /// Determines if an interstitial video is ready.
    /// </summary>
    /// <returns>Is an interstitial video ready</returns>
    bool isInterstitialReady();

    /// <summary>
    /// Determines if an interstitial placement is capped.
    /// </summary>
    /// <param name="placementName">Name of the placement</param>
    /// <returns>Is the placement capped</returns>
    bool isInterstitialPlacementCapped(string placementName);

    //******************* Banner API *******************//

    /// <summary>
    /// Load a banner.
    /// </summary>
    /// <param name="size">Standard size of the banner</param>
    /// <param name="position">Anchored position of the banner</param>
    void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position);

    /// <summary>
    /// Load and show a banner for a specific placement.
    /// </summary>
    /// <param name="size">Standard size of the banner</param>
    /// <param name="position">Anchored position of the banner</param>
    /// <param name="placementName">Name of the placement</param>
    void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName);

    /// <summary>
    /// Destroy a banner.
    /// </summary>
    void destroyBanner();

    /// <summary>
    /// Show a banner.
    /// </summary>
    void displayBanner();

    /// <summary>
    /// Hide a banner.
    /// </summary>
    void hideBanner();

    /// <summary>
    /// Determines if a banner placement is capped.
    /// </summary>
    /// <param name="placementName">Name of the placement</param>
    /// <returns>Is the banner placement capped</returns>
    bool isBannerPlacementCapped(string placementName);

    /// <summary>
    ///  Get maximal Adaptive Height.
    /// </summary>
    /// <param name="width"> The device width </param>
    float getMaximalAdaptiveHeight(float width);

    /// <summary>
    ///  Get Device Screen Width.
    /// </summary>
    float getDeviceScreenWidth();

    //******************* Segment API *******************//

    /// <summary>
    /// Set the segment a user belongs to.
    /// </summary>
    /// <param name="segment">Segment information for the current user</param>
    void setSegment(IronSourceSegment segment);

    //******************* Consent API *******************//

    /// <summary>
    /// Set user's GDPR consent
    /// </summary>
    /// <param name="consent">Is the user consenting</param>
    void setConsent(bool consent);

    //******************* ConsentView API *******************//

    /// <summary>
    /// Loads the consent view.
    /// </summary>
    /// <param name="consentViewType">View type for the consent popup</param>
    void loadConsentViewWithType(string consentViewType);

    /// <summary>
    /// Show the consent view
    /// </summary>
    /// <param name="consentViewType">View type for the consent popup</param>
    void showConsentViewWithType(string consentViewType);

    //******************* ILRD API *******************//

    /// <summary>
    /// Set ILRD information
    /// </summary>
    /// <param name="dataSource">Source of the ILRD info</param>
    /// <param name="impressionData">Key-value pair list containing the ILRD data</param>
    void setAdRevenueData(string dataSource, Dictionary<string, string> impressionData);


    //******************* TestSuite API *******************//

    void launchTestSuite();
}

/// <summary>
/// A Data Source descriptor
/// </summary>
public static class dataSource
{
    /// <summary>
    /// Defines the MoPub data source
    /// </summary>
    public static string MOPUB { get { return "MoPub"; } }
}

/// <summary>
/// Contains the available Ad Unit identifiers
/// </summary>
public static class IronSourceAdUnits
{
    /// <summary>
    /// Rewarded Video Ad Unit
    /// </summary>
    public static string REWARDED_VIDEO { get { return "rewardedvideo"; } }

    /// <summary>
    /// Interstitial Video Ad Unit
    /// </summary>
    public static string INTERSTITIAL { get { return "interstitial"; } }

    /// <summary>
    /// Offerwall Ad Unit
    /// </summary>
    public static string OFFERWALL { get { return "offerwall"; } }

    /// <summary>
    /// Banner Ad Unit
    /// </summary>
    public static string BANNER { get { return "banner"; } }
}

/// <summary>
/// Defines a banner's size informations
/// </summary>
public class IronSourceBannerSize
{
    private int width;
    private int height;
    private string description;
    private bool isAdaptive;
    private ISContainerParams isContainerParams = new ISContainerParams();
    private bool respectAndroidCutouts;

    /// <summary>
    /// Standard banner size
    /// </summary>
    public static IronSourceBannerSize BANNER = new IronSourceBannerSize("BANNER");

    /// <summary>
    /// Standard large size
    /// </summary>
    public static IronSourceBannerSize LARGE = new IronSourceBannerSize("LARGE");

    /// <summary>
    /// Standard rectangle size
    /// </summary>
    public static IronSourceBannerSize RECTANGLE = new IronSourceBannerSize("RECTANGLE");

    /// <summary>
    /// Standard rectangle size
    /// </summary>
    public static IronSourceBannerSize SMART = new IronSourceBannerSize("SMART");

    private IronSourceBannerSize()
    {
    }

    /// <summary>
    /// Constructor for a custom banner size
    /// </summary>
    /// <param name="width">Width of the banner</param>
    /// <param name="height">Height of the banner</param>
    public IronSourceBannerSize(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.description = "CUSTOM";
    }

    /// <summary>
    /// Constructor for a custom banner size
    /// </summary>
    /// <param name="description">Description of the banner</param>
    public IronSourceBannerSize(string description)
    {
        this.description = description;
        this.width = 0;
        this.height = 0;
    }

    /// <summary>
    /// Set the banner to adaptive
    /// </summary>
    /// <param name="adaptive">Is the banner adaptive</param>
    public void SetAdaptive(bool adaptive)
    {
        this.isAdaptive = adaptive;
    }

    /// <summary>
    /// Determines if the banner is adaptive
    /// </summary>
    /// <returns>Is the banner adaptive</returns>
    public bool IsAdaptiveEnabled()
    {
        return this.isAdaptive;
    }

    /// <summary>
    ///  Set the Container for adaptive banner.
    /// </summary>
    /// <param name="ISContainerParams">The Container params Width and Height.</param>
    public void setBannerContainerParams(ISContainerParams parameters)
    {
        this.isContainerParams = parameters;
    }

    public ISContainerParams getBannerContainerParams()
    {
        return this.isContainerParams;
    }

    /// <summary>
    ///  Set Respect for Android Cutouts. https://developer.android.com/develop/ui/views/layout/display-cutout
    /// </summary>
    /// <param name="SetRespectAndroidCutouts">Set if to respect the Android Cutouts or not.</param>
    public void SetRespectAndroidCutouts(bool respectAndroidCutouts)
    {
        this.respectAndroidCutouts = respectAndroidCutouts;
    }

    public bool IsRespectAndroidCutoutsEnabled()
    {
        return this.respectAndroidCutouts;
    }

    /// <summary>
    /// Description for the banner
    /// </summary>
    public string Description { get { return description; } }

    /// <summary>
    /// Width of the banner
    /// </summary>
    public int Width { get { return width; } }

    /// <summary>
    /// Height of the banner
    /// </summary>
    public int Height { get { return height; } }
}

/// <summary>
/// Banner position anchor options
/// </summary>
public enum IronSourceBannerPosition
{
    TOP = 1,
    BOTTOM = 2
};

public class ISContainerParams
{
    public float Width { get; set; }
    public float Height { get; set; }

    public ISContainerParams()
    {
        Width = -1;
        Height = -1;
    }
}
