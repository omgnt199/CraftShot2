using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemoteConfigLoader : MonoBehaviour
{
    //private Firebase.FirebaseApp app => ServiceManager.firebaseApp;
    //private Firebase.RemoteConfig.FirebaseRemoteConfig remoteConfig;

    //public static bool IsInitialized { get; private set; }


    //public void Init()
    //{
    //    Debug.Log("Firebase remote config initialized");
    //    SetRemoteConfigDefaults();
    //}


    ////public static readonly Dictionary<string, object> DefaultValues = new Dictionary<string, object>
    ////    {
    ////        { "collapsible_ad_delay", 50L },
    ////        { "collapsible_ad_gameplay_enabled", false },
    ////        { "block_place_bonus_count",200L },
    ////        { "block_place_bonus",40L },
    ////        { "maintain_state_url", "https://maintain-cratf-world.eztechglobal.com/jsons/server_mantain.json" },
    ////        { "domain_list", "[\"https://cratf-world.eztechglobal.com/api/v\",\"https://cratf-world-2.eztechglobal.com/api/v\"]" },
    ////        { "maintain_popup_notice_delay", 300L },
    ////        { "force_open_change_log_tab", "" },
    ////        { "change_log", "[{\"tabName\":\"Changelog\" ,\"tabDisplayName\":\"Changelog\" ,\"content\":\"<b><size=52><color=#0052C3>v1.1.10</color><br>New Features:\r\n</color></size></b>\r\n- TNT can destroy doors\r\n- Spawn area is protected\r\n- Fix block lighting<br><b><size=52><color=#0052C3>v1.1.9</color><br>New Features:\r\n</color></size></b>\r\n- Add more blocks<br><b><size=52><color=#0052C3>v1.1.7</color><br>New Features:\r\n</color></size></b><br>1. World Settings\r\n- Customize your world with specific rules. Enable or disable flying, block destruction, and adjust in-game time.\r\n- Shape your world to match your preferred playstyle.\r\n2. Shared World\r\n- Collaborate with others to construct together seamlessly.\r\n- Assign roles (Builders, Visitors).\r\n- Set up collaborative challenges for creative fun.\r\nRemember, these features empower players to shape the game world together. Happy gaming! . \"}]" },
    ////        {"enable_collap_ad",false },
    ////        {"enable_banner_ad",true },
    ////        {"enable_native_ad2",true },
    ////        {"community_showcase_record",0L }

    ////};

    //private void SetRemoteConfigDefaults()
    //{
    //    remoteConfig = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance;

    //    // change minimumFetchInterval and timeout to 30 minutes
    //    var configSettings = new ConfigSettings
    //    {
    //        MinimumFetchInternalInMilliseconds = 1800000,
    //    };
    //    //remoteConfig.SetDefaultsAsync(DefaultValues).ContinueWithOnMainThread(previousTask =>
    //    //{
    //    //    FetchRemoteConfig(InitializeCommonDataAndStartGame);
    //    //});
    //    remoteConfig.SetConfigSettingsAsync(configSettings).ContinueWithOnMainThread(previousTask =>
    //    {
    //        Debug.Log("remoteConfig.SetConfigSettingsAsync");
    //        remoteConfig.SetDefaultsAsync(DefaultValues).ContinueWithOnMainThread(previousTask =>
    //        {
    //            Debug.Log("remoteConfig.SetDefaultsAsync");
    //            FetchRemoteConfig(InitializeCommonDataAndStartGame);
    //        });
    //    });


    //}

    //private void FetchRemoteConfig(System.Action callback)
    //{
    //    remoteConfig.FetchAsync().ContinueWithOnMainThread(fetchTask =>
    //    {
    //        if (fetchTask.IsCanceled)
    //        {
    //            UnityEngine.Debug.LogError("Fetch canceled");
    //        }
    //        else if (fetchTask.IsFaulted)
    //        {
    //            UnityEngine.Debug.LogError("Fetch encountered an error");
    //        }
    //        else if (fetchTask.IsCompleted)
    //        {
    //            UnityEngine.Debug.Log("Fetch completed successfully!");
    //            ActivateRetrievedRemoteConfigValues(callback);
    //        }
    //    });
    //}

    //private void ActivateRetrievedRemoteConfigValues(System.Action onFetchAndActivateSuccessful)
    //{
    //    var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
    //    var info = remoteConfig.Info;
    //    if (info.LastFetchStatus == LastFetchStatus.Success)
    //    {
    //        remoteConfig.ActivateAsync().ContinueWithOnMainThread(
    //           previousTask =>
    //           {
    //               Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
    //               onFetchAndActivateSuccessful();
    //           });
    //    }
    //}

    //private void InitializeCommonDataAndStartGame()
    //{
    //    IsInitialized = true;
    //    var collapsibleAdDelay = remoteConfig.GetValue("collapsible_ad_delay").LongValue;
    //    var collapsibleAdGameplayEnabled = remoteConfig.GetValue("collapsible_ad_gameplay_enabled").BooleanValue;
    //    Debug.Log("collapsibleAdDelay: " + collapsibleAdDelay);
    //    Debug.Log("collapsibleAdGameplayEnabled: " + collapsibleAdGameplayEnabled);
    //}

    ////public static ConfigValue GetValue(string key)
    ////{
    ////    if (!IsInitialized)
    ////    {
    ////        Debug.LogError("RemoteConfigLoader is not initialized yet.");
    ////        return null;
    ////    }
    ////    return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);
    ////}

    //public static string GetString(string key)
    //{
    //    if (!IsInitialized)
    //    {
    //        return DefaultValues[key].ToString();
    //    }
    //    return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
    //}

    //public static long GetLong(string key)
    //{
    //    if (!IsInitialized)
    //    {
    //        return (long)DefaultValues[key];
    //    }
    //    return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
    //}

    //public static bool GetBool(string key)
    //{
    //    if (!IsInitialized)
    //    {
    //        try
    //        {

    //            return (bool)DefaultValues[key];
    //        }
    //        catch (System.Exception e)
    //        {
    //            Debug.LogException(e);
    //            return false;
    //        }
    //    }
    //    return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
    //}

}
