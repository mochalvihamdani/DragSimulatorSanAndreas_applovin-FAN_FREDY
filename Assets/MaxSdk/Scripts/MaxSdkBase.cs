using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AppLovinMax.ThirdParty.MiniJson;
using AppLovinMax.Internal;
using UnityEngine;

#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public abstract class MaxSdkBase
{
    /// <summary>
    /// This enum represents the user's geography used to determine the type of consent flow shown to the user.
    /// </summary>
    public enum ConsentFlowUserGeography
    {
        /// <summary>
        /// User's geography is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The user is in GDPR region.
        /// </summary>
        Gdpr,

        /// <summary>
        /// The user is in a non-GDPR region.
        /// </summary>
        Other
    }

#if UNITY_EDITOR || UNITY_IPHONE || UNITY_IOS
    /// <summary>
    /// App tracking status values. Primarily used in conjunction with iOS14's AppTrackingTransparency.framework.
    /// </summary>
    public enum AppTrackingStatus
    {
        /// <summary>
        /// Device is on &lt; iOS14, AppTrackingTransparency.framework is not available.
        /// </summary>
        Unavailable,

        /// <summary>
        /// The value returned if a user has not yet received an authorization request to authorize access to app-related data that can be used for tracking the user or the device.
        /// </summary>
        NotDetermined,

        /// <summary>
        /// The value returned if authorization to access app-related data that can be used for tracking the user or the device is restricted.
        /// </summary>
        Restricted,

        /// <summary>
        /// The value returned if the user denies authorization to access app-related data that can be used for tracking the user or the device.
        /// </summary>
        Denied,

        /// <summary>
        /// The value returned if the user authorizes access to app-related data that can be used for tracking the user or the device.
        /// </summary>
        Authorized,
    }
#endif

    /// <summary>
    /// An enum describing the adapter's initialization status.
    /// </summary>
    public enum InitializationStatus
    {
        /// <summary>
        /// The adapter is not initialized. Note: networks need to be enabled for an ad unit id to be initialized.
        /// </summary>
        NotInitialized = -4,

        /// <summary>
        /// The 3rd-party SDK does not have an initialization callback with status.
        /// </summary>
        DoesNotApply = -3,

        /// <summary>
        /// The 3rd-party SDK is currently initializing.
        /// </summary>
        Initializing = -2,

        /// <summary>
        /// The 3rd-party SDK explicitly initialized, but without a status.
        /// </summary>
        InitializedUnknown = -1,

        /// <summary>
        /// The 3rd-party SDK initialization failed.
        /// </summary>
        InitializedFailure = 0,

        /// <summary>
        /// The 3rd-party SDK initialization was successful.
        /// </summary>
        InitializedSuccess = 1
    }

    public enum AdViewPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        CenterLeft,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public class AdViewConfiguration
    {
        /// <summary>
        /// The position of the ad.
        /// </summary>
        public AdViewPosition Position { get; private set; }

        /// <summary>
        /// The horizontal (X) position of the banner, relative to the top-left corner of the screen's safe area.
        /// </summary>
        public float XCoordinate { get; private set; }

        /// <summary>
        /// The vertical (Y) position of the banner, relative to the top-left corner of the screen's safe area.
        /// </summary>
        public float YCoordinate { get; private set; }

        /// <summary>
        /// Whether to use adaptive banners. Has no effect on MREC ads.
        /// </summary>
        public bool IsAdaptive { get; set; }

        internal bool UseCoordinates { get; private set; }

        /// <summary>
        /// Creates an AdViewConfiguration with the given AdViewPosition.
        /// </summary>
        /// <param name="position">The position of the ad. Must not be null.</param>
        public AdViewConfiguration(AdViewPosition position)
        {
            Position = position;
            IsAdaptive = true;
            UseCoordinates = false;
        }

        /// <summary>
        /// Creates an AdViewConfiguration with the given x and y coordinates.
        /// </summary>
        /// <param name="x">The horizontal (X) position of the banner, relative to the top-left corner of the screen's safe area.</param>
        /// <param name="y">The vertical (Y) position of the banner, relative to the top-left corner of the screen's safe area.</param>
        public AdViewConfiguration(float x, float y)
        {
            XCoordinate = x;
            YCoordinate = y;
            IsAdaptive = true;
            UseCoordinates = true;
        }
    }

    public class SdkConfiguration
    {
        /// <summary>
        /// Whether or not the SDK has been initialized successfully.
        /// </summary>
        public bool IsSuccessfullyInitialized { get; private set; }

        /// <summary>
        /// Get the country code for this user.
        /// </summary>
        public string CountryCode { get; private set; }

#if UNITY_EDITOR || UNITY_IPHONE || UNITY_IOS
        /// <summary>
        /// App tracking status values. Primarily used in conjunction with iOS14's AppTrackingTransparency.framework.
        /// </summary>
        public AppTrackingStatus AppTrackingStatus { get; private set; }
#endif

        public bool IsTestModeEnabled { get; private set; }

        /// <summary>
        /// Get the user's geography used to determine the type of consent flow shown to the user.
        /// If no such determination could be made, <see cref="MaxSdkBase.ConsentFlowUserGeography.Unknown"/> will be returned.
        /// </summary>
        public ConsentFlowUserGeography ConsentFlowUserGeography { get; private set; }

        [Obsolete("This API has been deprecated and will be removed in a future release.")]
        public ConsentDialogState ConsentDialogState { get; private set; }

#if UNITY_EDITOR || !(UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS)
        public static SdkConfiguration CreateEmpty()
        {
            var sdkConfiguration = new SdkConfiguration();
            sdkConfiguration.IsSuccessfullyInitialized = true;
#pragma warning disable 0618
            sdkConfiguration.ConsentDialogState = ConsentDialogState.Unknown;
#pragma warning restore 0618
#if UNITY_EDITOR
            sdkConfiguration.AppTrackingStatus = AppTrackingStatus.Authorized;
#endif
            sdkConfiguration.CountryCode = TryGetCountryCode();
            sdkConfiguration.IsTestModeEnabled = false;

            return sdkConfiguration;
        }

        private static string TryGetCountryCode()
        {
            try
            {
                return RegionInfo.CurrentRegion.TwoLetterISORegionName;
            }
#pragma warning disable 0168
            catch (Exception ignored)
#pragma warning restore 0168
            {
                // Ignored
            }

            return "US";
        }
#endif

        public static SdkConfiguration Create(IDictionary<string, object> eventProps)
        {
            var sdkConfiguration = new SdkConfiguration();

            sdkConfiguration.IsSuccessfullyInitialized = MaxSdkUtils.GetBoolFromDictionary(eventProps, "isSuccessfullyInitialized");
            sdkConfiguration.CountryCode = MaxSdkUtils.GetStringFromDictionary(eventProps, "countryCode", "");
            sdkConfiguration.IsTestModeEnabled = MaxSdkUtils.GetBoolFromDictionary(eventProps, "isTestModeEnabled");

            var consentFlowUserGeographyStr = MaxSdkUtils.GetStringFromDictionary(eventProps, "consentFlowUserGeography", "");
            if ("1".Equals(consentFlowUserGeographyStr))
            {
                sdkConfiguration.ConsentFlowUserGeography = ConsentFlowUserGeography.Gdpr;
            }
            else if ("2".Equals(consentFlowUserGeographyStr))
            {
                sdkConfiguration.ConsentFlowUserGeography = ConsentFlowUserGeography.Other;
            }
            else
            {
                sdkConfiguration.ConsentFlowUserGeography = ConsentFlowUserGeography.Unknown;
            }

#pragma warning disable 0618
            var consentDialogStateStr = MaxSdkUtils.GetStringFromDictionary(eventProps, "consentDialogState", "");
            if ("1".Equals(consentDialogStateStr))
            {
                sdkConfiguration.ConsentDialogState = ConsentDialogState.Applies;
            }
            else if ("2".Equals(consentDialogStateStr))
            {
                sdkConfiguration.ConsentDialogState = ConsentDialogState.DoesNotApply;
            }
            else
            {
                sdkConfiguration.ConsentDialogState = ConsentDialogState.Unknown;
            }
#pragma warning restore 0618

#if UNITY_IPHONE || UNITY_IOS
            var appTrackingStatusStr = MaxSdkUtils.GetStringFromDictionary(eventProps, "appTrackingStatus", "-1");
            if ("-1".Equals(appTrackingStatusStr))
            {
                sdkConfiguration.AppTrackingStatus = AppTrackingStatus.Unavailable;
            }
            else if ("0".Equals(appTrackingStatusStr))
            {
                sdkConfiguration.AppTrackingStatus = AppTrackingStatus.NotDetermined;
            }
            else if ("1".Equals(appTrackingStatusStr))
            {
                sdkConfiguration.AppTrackingStatus = AppTrackingStatus.Restricted;
            }
            else if ("2".Equals(appTrackingStatusStr))
            {
                sdkConfiguration.AppTrackingStatus = AppTrackingStatus.Denied;
            }
            else // "3" is authorized
            {
                sdkConfiguration.AppTrackingStatus = AppTrackingStatus.Authorized;
            }
#endif

            return sdkConfiguration;
        }
    }

    public struct Reward
    {
        public string Label;
        public int Amount;

        public override string ToString()
        {
            return "Reward: " + Amount + " " + Label;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Label) && Amount > 0;
        }
    }

    /**
     *  This enum contains various error codes that the SDK can return when a MAX ad fails to load or display.
     */
    public enum ErrorCode
    {
        /// <summary>
        /// This error code represents an error that could not be categorized into one of the other defined errors. See the message field in the error object for more details.
        /// </summary>
        Unspecified = -1,

        /// <summary>
        /// This error code indicates that MAX returned no eligible ads from any mediated networks for this app/device.
        /// </summary>
        NoFill = 204,

        /// <summary>
        /// This error code indicates that MAX returned eligible ads from mediated networks, but all ads failed to load. See the adLoadFailureInfo field in the error object for more details.
        /// </summary>
        AdLoadFailed = -5001,

        /// <summary>
        /// This error code represents an error that was encountered when showing an ad.
        /// </summary>
        AdDisplayFailed = -4205,

        /// <summary>
        /// This error code indicates that the ad request failed due to a generic network error. See the message field in the error object for more details.
        /// </summary>
        NetworkError = -1000,

        /// <summary>
        /// This error code indicates that the ad request timed out due to a slow internet connection.
        /// </summary>
        NetworkTimeout = -1001,

        /// <summary>
        /// This error code indicates that the ad request failed because the device is not connected to the internet.
        /// </summary>
        NoNetwork = -1009,

        /// <summary>
        /// This error code indicates that you attempted to show a fullscreen ad while another fullscreen ad is still showing.
        /// </summary>
        FullscreenAdAlreadyShowing = -23,

        /// <summary>
        /// This error code indicates you are attempting to show a fullscreen ad before the one has been loaded.
        /// </summary>
        FullscreenAdNotReady = -24,

#if UNITY_IOS || UNITY_IPHONE
        /// <summary>
        /// This error code indicates you attempted to present a fullscreen ad from an invalid view controller.
        /// </summary>
        FullscreenAdInvalidViewController = -25,
#endif

        /// <summary>
        /// This error code indicates you are attempting to load a fullscreen ad while another fullscreen ad is already loading.
        /// </summary>
        FullscreenAdAlreadyLoading = -26,

        /// <summary>
        /// This error code indicates you are attempting to load a fullscreen ad while another fullscreen ad is still showing.
        /// </summary>
        FullscreenAdLoadWhileShowing = -27,

#if UNITY_ANDROID
        /// <summary>
        /// This error code indicates that the SDK failed to display an ad because the user has the "Don't Keep Activities" developer setting enabled.
        /// </summary>
        DontKeepActivitiesEnabled = -5602,
#endif

        /// <summary>
        /// This error code indicates that the SDK failed to load an ad because the publisher provided an invalid ad unit identifier.
        /// Possible reasons for an invalid ad unit identifier:
        /// 1. Ad unit identifier is malformed or does not exist
        /// 2. Ad unit is disabled
        /// 3. Ad unit is not associated with the current app's package name
        /// 4. Ad unit was created within the last 30-60 minutes
        /// </summary>
        InvalidAdUnitId = -5603
    }

    /**
     * This enum contains possible states of an ad in the waterfall the adapter response info could represent.
     */
    public enum MaxAdLoadState
    {
        /// <summary>
        /// The AppLovin Max SDK did not attempt to load an ad from this network in the waterfall because an ad higher
        /// in the waterfall loaded successfully.
        /// </summary>
        AdLoadNotAttempted,

        /// <summary>
        /// An ad successfully loaded from this network.
        /// </summary>
        AdLoaded,

        /// <summary>
        /// An ad failed to load from this network.
        /// </summary>
        FailedToLoad
    }

    public class AdInfo
    {
        public string AdUnitIdentifier { get; private set; }
        public string AdFormat { get; private set; }
        public string NetworkName { get; private set; }
        public string NetworkPlacement { get; private set; }
        public string Placement { get; private set; }
        public string CreativeIdentifier { get; private set; }
        public double Revenue { get; private set; }
        public string RevenuePrecision { get; private set; }
        public WaterfallInfo WaterfallInfo { get; private set; }
        public long LatencyMillis { get; private set; }
        public string DspName { get; private set; }

        public AdInfo(IDictionary<string, object> adInfoDictionary)
        {
            AdUnitIdentifier = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "adUnitId");
            AdFormat = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "adFormat");
            NetworkName = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "networkName");
            NetworkPlacement = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "networkPlacement");
            CreativeIdentifier = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "creativeId");
            Placement = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "placement");
            Revenue = MaxSdkUtils.GetDoubleFromDictionary(adInfoDictionary, "revenue", -1);
            RevenuePrecision = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "revenuePrecision");
            WaterfallInfo = new WaterfallInfo(MaxSdkUtils.GetDictionaryFromDictionary(adInfoDictionary, "waterfallInfo", new Dictionary<string, object>()));
            LatencyMillis = MaxSdkUtils.GetLongFromDictionary(adInfoDictionary, "latencyMillis");
            DspName = MaxSdkUtils.GetStringFromDictionary(adInfoDictionary, "dspName");
        }

        public override string ToString()
        {
            return "[AdInfo adUnitIdentifier: " + AdUnitIdentifier +
                   ", adFormat: " + AdFormat +
                   ", networkName: " + NetworkName +
                   ", networkPlacement: " + NetworkPlacement +
                   ", creativeIdentifier: " + CreativeIdentifier +
                   ", placement: " + Placement +
                   ", revenue: " + Revenue +
                   ", revenuePrecision: " + RevenuePrecision +
                   ", latency: " + LatencyMillis +
                   ", dspName: " + DspName + "]";
        }
    }

    /// <summary>
    /// Returns information about the ad response in a waterfall.
    /// </summary>
    public class WaterfallInfo
    {
        public String Name { get; private set; }
        public String TestName { get; private set; }
        public List<NetworkResponseInfo> NetworkResponses { get; private set; }
        public long LatencyMillis { get; private set; }

        public WaterfallInfo(IDictionary<string, object> waterfallInfoDict)
        {
            Name = MaxSdkUtils.GetStringFromDictionary(waterfallInfoDict, "name");
            TestName = MaxSdkUtils.GetStringFromDictionary(waterfallInfoDict, "testName");

            var networkResponsesList = MaxSdkUtils.GetListFromDictionary(waterfallInfoDict, "networkResponses", new List<object>());
            NetworkResponses = new List<NetworkResponseInfo>();
            foreach (var networkResponseObject in networkResponsesList)
            {
                var networkResponseDict = networkResponseObject as Dictionary<string, object>;
                if (networkResponseDict == null) continue;

                var networkResponse = new NetworkResponseInfo(networkResponseDict);
                NetworkResponses.Add(networkResponse);
            }

            LatencyMillis = MaxSdkUtils.GetLongFromDictionary(waterfallInfoDict, "latencyMillis");
        }

        public override string ToString()
        {
            return "[MediatedNetworkInfo: name = " + Name +
                   ", testName = " + TestName +
                   ", latency = " + LatencyMillis +
                   ", networkResponse = " + string.Join(", ", NetworkResponses.Select(networkResponseInfo => networkResponseInfo.ToString()).ToArray()) + "]";
        }
    }

    public class NetworkResponseInfo
    {
        public MaxAdLoadState AdLoadState { get; private set; }
        public MediatedNetworkInfo MediatedNetwork { get; private set; }
        public Dictionary<string, object> Credentials { get; private set; }
        public bool IsBidding { get; private set; }
        public long LatencyMillis { get; private set; }
        public ErrorInfo Error { get; private set; }

        public NetworkResponseInfo(IDictionary<string, object> networkResponseInfoDict)
        {
            var mediatedNetworkInfoDict = MaxSdkUtils.GetDictionaryFromDictionary(networkResponseInfoDict, "mediatedNetwork");
            MediatedNetwork = mediatedNetworkInfoDict != null ? new MediatedNetworkInfo(mediatedNetworkInfoDict) : null;

            Credentials = MaxSdkUtils.GetDictionaryFromDictionary(networkResponseInfoDict, "credentials", new Dictionary<string, object>());
            IsBidding = MaxSdkUtils.GetBoolFromDictionary(networkResponseInfoDict, "isBidding");
            LatencyMillis = MaxSdkUtils.GetLongFromDictionary(networkResponseInfoDict, "latencyMillis");
            AdLoadState = (MaxAdLoadState) MaxSdkUtils.GetIntFromDictionary(networkResponseInfoDict, "adLoadState");

            var errorInfoDict = MaxSdkUtils.GetDictionaryFromDictionary(networkResponseInfoDict, "error");
            Error = errorInfoDict != null ? new ErrorInfo(errorInfoDict) : null;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder("[NetworkResponseInfo: adLoadState = ").Append(AdLoadState);
            stringBuilder.Append(", mediatedNetwork = ").Append(MediatedNetwork);
            stringBuilder.Append(", credentials = ").Append(string.Join(", ", Credentials.Select(keyValuePair => keyValuePair.ToString()).ToArray()));

            switch (AdLoadState)
            {
                case MaxAdLoadState.FailedToLoad:
                    stringBuilder.Append(", error = ").Append(Error);
                    break;
                case MaxAdLoadState.AdLoaded:
                    stringBuilder.Append(", latency = ").Append(LatencyMillis);
                    break;
            }

            return stringBuilder.Append("]").ToString();
        }
    }

    public class MediatedNetworkInfo
    {
        public string Name { get; private set; }
        public string AdapterClassName { get; private set; }
        public string AdapterVersion { get; private set; }
        public string SdkVersion { get; private set; }
        public InitializationStatus InitializationStatus { get; private set; }

        public MediatedNetworkInfo(IDictionary<string, object> mediatedNetworkDictionary)
        {
            // NOTE: Unity Editor creates empty string
            Name = MaxSdkUtils.GetStringFromDictionary(mediatedNetworkDictionary, "name", "");
            AdapterClassName = MaxSdkUtils.GetStringFromDictionary(mediatedNetworkDictionary, "adapterClassName", "");
            AdapterVersion = MaxSdkUtils.GetStringFromDictionary(mediatedNetworkDictionary, "adapterVersion", "");
            SdkVersion = MaxSdkUtils.GetStringFromDictionary(mediatedNetworkDictionary, "sdkVersion", "");
            var initializationStatusInt = MaxSdkUtils.GetIntFromDictionary(mediatedNetworkDictionary, "initializationStatus", (int) InitializationStatus.NotInitialized);
            InitializationStatus = InitializationStatusFromCode(initializationStatusInt);
        }

        public override string ToString()
        {
            return "[MediatedNetworkInfo name: " + Name +
                   ", adapterClassName: " + AdapterClassName +
                   ", adapterVersion: " + AdapterVersion +
                   ", sdkVersion: " + SdkVersion +
                   ", initializationStatus: " + InitializationStatus + "]";
        }

        private static InitializationStatus InitializationStatusFromCode(int code)
        {
            if (Enum.IsDefined(typeof(InitializationStatus), code))
            {
                return (InitializationStatus) code;
            }
            else
            {
                return InitializationStatus.NotInitialized;
            }
        }
    }

    public class ErrorInfo
    {
        public ErrorCode Code { get; private set; }
        public string Message { get; private set; }
        public int MediatedNetworkErrorCode { get; private set; }
        public string MediatedNetworkErrorMessage { get; private set; }
        public string AdLoadFailureInfo { get; private set; }
        public WaterfallInfo WaterfallInfo { get; private set; }
        public long LatencyMillis { get; private set; }

        public ErrorInfo(IDictionary<string, object> errorInfoDictionary)
        {
            Code = (ErrorCode) MaxSdkUtils.GetIntFromDictionary(errorInfoDictionary, "errorCode", -1);
            Message = MaxSdkUtils.GetStringFromDictionary(errorInfoDictionary, "errorMessage", "");
            MediatedNetworkErrorCode = MaxSdkUtils.GetIntFromDictionary(errorInfoDictionary, "mediatedNetworkErrorCode", (int) ErrorCode.Unspecified);
            MediatedNetworkErrorMessage = MaxSdkUtils.GetStringFromDictionary(errorInfoDictionary, "mediatedNetworkErrorMessage", "");
            AdLoadFailureInfo = MaxSdkUtils.GetStringFromDictionary(errorInfoDictionary, "adLoadFailureInfo", "");
            WaterfallInfo = new WaterfallInfo(MaxSdkUtils.GetDictionaryFromDictionary(errorInfoDictionary, "waterfallInfo", new Dictionary<string, object>()));
            LatencyMillis = MaxSdkUtils.GetLongFromDictionary(errorInfoDictionary, "latencyMillis");
        }

        public override string ToString()
        {
            var stringbuilder = new StringBuilder("[ErrorInfo code: ").Append(Code);
            stringbuilder.Append(", message: ").Append(Message);

            if (Code == ErrorCode.AdDisplayFailed)
            {
                stringbuilder.Append(", mediatedNetworkCode: ").Append(MediatedNetworkErrorCode);
                stringbuilder.Append(", mediatedNetworkMessage: ").Append(MediatedNetworkErrorMessage);
            }

            stringbuilder.Append(", latency: ").Append(LatencyMillis);
            return stringbuilder.Append(", adLoadFailureInfo: ").Append(AdLoadFailureInfo).Append("]").ToString();
        }
    }

    /// <summary>
    /// Inset values for the safe area on the screen used to render banner ads.
    /// </summary>
    public class SafeAreaInsets
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="SafeAreaInsets"/>.
        /// </summary>
        /// <param name="insets">An integer array with insets values in the order of left, top, right, and bottom</param>
        internal SafeAreaInsets(int[] insets)
        {
            Left = insets[0];
            Top = insets[1];
            Right = insets[2];
            Bottom = insets[3];
        }

        public override string ToString()
        {
            return "[SafeAreaInsets: Left: " + Left +
                   ", Top: " + Top +
                   ", Right: " + Right +
                   ", Bottom: " + Bottom + "]";
        }
    }

    /// <summary>
    /// Determines whether ad events raised by the AppLovin's Unity plugin should be invoked on the Unity main thread.
    /// </summary>
    public static bool? InvokeEventsOnUnityMainThread { get; set; }

    /// <summary>
    /// The CMP service, which provides direct APIs for interfacing with the Google-certified CMP installed, if any.
    /// </summary>
    public static MaxCmpService CmpService
    {
        get { return MaxCmpService.Instance; }
    }

    internal static bool DisableAllLogs
    {
        get; private set;
    }

    protected static void ValidateAdUnitIdentifier(string adUnitIdentifier, string debugPurpose)
    {
        if (string.IsNullOrEmpty(adUnitIdentifier))
        {
            MaxSdkLogger.UserError("No MAX Ads Ad Unit ID specified for: " + debugPurpose);
        }
    }

    // Allocate the MaxEventExecutor singleton which handles pushing callbacks from the background to the main thread.
    protected static void InitializeEventExecutor()
    {
        MaxEventExecutor.InitializeIfNeeded();
    }

    /// <summary>
    /// Generates serialized Unity meta data to be passed to the SDK.
    /// </summary>
    /// <returns>Serialized Unity meta data.</returns>
    protected static string GenerateMetaData()
    {
        var metaData = new Dictionary<string, string>(2);
        metaData.Add("UnityVersion", Application.unityVersion);

        return Json.Serialize(metaData);
    }

    /// <summary>
    /// Parses the prop string provided to a <see cref="Rect"/>.
    /// </summary>
    /// <param name="rectPropString">A prop string representing a Rect</param>
    /// <returns>A <see cref="Rect"/> the prop string represents.</returns>
    protected static Rect GetRectFromString(string rectPropString)
    {
        var rectDict = Json.Deserialize(rectPropString) as Dictionary<string, object>;
        var originX = MaxSdkUtils.GetFloatFromDictionary(rectDict, "origin_x", 0);
        var originY = MaxSdkUtils.GetFloatFromDictionary(rectDict, "origin_y", 0);
        var width = MaxSdkUtils.GetFloatFromDictionary(rectDict, "width", 0);
        var height = MaxSdkUtils.GetFloatFromDictionary(rectDict, "height", 0);

        return new Rect(originX, originY, width, height);
    }

    protected static void HandleExtraParameter(string key, string value)
    {
        bool disableAllLogs;
        if ("disable_all_logs".Equals(key) && bool.TryParse(value, out disableAllLogs))
        {
            DisableAllLogs = disableAllLogs;
        }
    }

    /// <summary>
    /// Handles forwarding callbacks from native to C#.
    /// </summary>
    /// <param name="propsStr">A prop string with the event data</param>
    protected static void HandleBackgroundCallback(string propsStr)
    {
        try
        {
            MaxSdkCallbacks.ForwardEvent(propsStr);
        }
        catch (Exception exception)
        {
            var eventProps = Json.Deserialize(propsStr) as Dictionary<string, object>;
            if (eventProps == null) return;

            var eventName = MaxSdkUtils.GetStringFromDictionary(eventProps, "name", "");
            MaxSdkLogger.UserError("Unable to notify ad delegate due to an error in the publisher callback '" + eventName + "' due to exception: " + exception.Message);
            MaxSdkLogger.LogException(exception);
        }
    }

    protected static string SerializeLocalExtraParameterValue(object value)
    {
        if (!(value.GetType().IsPrimitive || value is string || value is IList || value is IDictionary))
        {
            MaxSdkLogger.UserError("Local extra parameters must be an IList, IDictionary, string, or a primitive type");
            return "";
        }

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            {"value", value}
        };

        return Json.Serialize(data);
    }

    #region Obsolete

    [Obsolete("This API has been deprecated and will be removed in a future release. Please use AdViewPosition instead.")]
    public enum BannerPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        CenterLeft,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    [Obsolete("This API has been deprecated and will be removed in a future release.")]
    public enum ConsentDialogState
    {
        Unknown,
        Applies,
        DoesNotApply
    }

    #endregion
}

/// <summary>
/// An extension class for <see cref="MaxSdkBase.BannerPosition"/> and <see cref="MaxSdkBase.AdViewPosition"/> enums.
/// </summary>
internal static class AdPositionExtenstion
{
    public static string ToSnakeCaseString(this MaxSdkBase.AdViewPosition position)
    {
        if (position == MaxSdkBase.AdViewPosition.TopLeft)
        {
            return "top_left";
        }
        else if (position == MaxSdkBase.AdViewPosition.TopCenter)
        {
            return "top_center";
        }
        else if (position == MaxSdkBase.AdViewPosition.TopRight)
        {
            return "top_right";
        }
        else if (position == MaxSdkBase.AdViewPosition.Centered)
        {
            return "centered";
        }
        else if (position == MaxSdkBase.AdViewPosition.CenterLeft)
        {
            return "center_left";
        }
        else if (position == MaxSdkBase.AdViewPosition.CenterRight)
        {
            return "center_right";
        }
        else if (position == MaxSdkBase.AdViewPosition.BottomLeft)
        {
            return "bottom_left";
        }
        else if (position == MaxSdkBase.AdViewPosition.BottomCenter)
        {
            return "bottom_center";
        }
        else // position == MaxSdkBase.AdViewPosition.BottomRight
        {
            return "bottom_right";
        }
    }
}
