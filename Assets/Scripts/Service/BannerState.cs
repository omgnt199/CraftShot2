
using Assets.Scripts.Common;
using com.adjust.sdk;
using Firebase.Analytics;
# if !UNITY_STANDALONE
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdmobBanner
{
    public class BannerState : MonoBehaviour
    {

#if UNITY_ANDROID
        const string _adUnitId = "ca-app-pub-7398858403018011/6692001748";
#elif UNITY_IPHONE
  const string _adUnitId = "ca-app-pub-7398858403018011/4447576811"; 
#else
        const string _adUnitId = "unused";
#endif

#if !UNITY_STANDALONE
        public static List<Tuple<GameObject, bool, AdPosition, bool>> bannerController = new();
        static BannerView bannerView, bannerViewFullWidth;
        public bool bannerState;
        public AdPosition AdPosition = AdPosition.Bottom;
        public bool isHighPriority;
        public bool isFullWidth;

        public static List<Tuple<GameObject, bool, AdPosition, bool>> highPriorityBannerController = new();

        private void Start()
        {
            LoadBannerFullWidthIfNotLoaded();
        }

        private void OnEnable()
        {
            //if (!RemoteConfigLoader.GetBool("enable_banner_ad")) return;
            //if (ServiceManager.Instance.IsStaging || !IAPProductDefine.IsAdAllowed) return;
            this.WaitNextFrame(() =>
            {
                //AddState(gameObject, bannerState, AdPosition);
                AddState(gameObject, bannerState, AdPosition, isFullWidth, isHighPriority);

            });
        }

        void LoadBannerFullWidthIfNotLoaded()
        {
            if (bannerViewFullWidth == null)
            {
                bannerViewFullWidth = new BannerView(_adUnitId, AdSize.GetLandscapeAnchoredAdaptiveBannerAdSizeWithWidth(-1), AdPosition.Bottom);
                var adRequest = new AdRequest();
                bannerViewFullWidth.OnAdPaid += Analytics_SendEvent_ViewAds;
                bannerViewFullWidth.LoadAd(adRequest);

            }
        }

        public void OnDisable()
        {

            RemoveState(gameObject);
        }

        static void LoadLastState()
        {
            //if (bannerController.Count == 0)
            if (bannerController.Count + highPriorityBannerController.Count == 0)
            {
                HideBanner();
                return;
            }
            //Tuple<GameObject, bool, AdPosition> currentState = bannerController[bannerController.Count - 1];
            Tuple<GameObject, bool, AdPosition, bool> currentState = null;
            if (highPriorityBannerController.Count > 0)
            {
                currentState = highPriorityBannerController[highPriorityBannerController.Count - 1];
            }
            else
            {
                currentState = bannerController[bannerController.Count - 1];
            }
            if (currentState.Item2)
            {
                //ServiceManager.applovinAds.ShowBanner();
                //if (ServiceManager.Instance.IsStaging || !IAPProductDefine.IsAdAllowed) return;
                LoadBanner(currentState.Item4);
                //ServiceManager.applovinAds.SetBannerPosition(currentState.Item3);
                if (!currentState.Item4)
                    bannerView.SetPosition(currentState.Item3);
            }
            else
                //ServiceManager.applovinAds.HideBanner();
                HideBanner();
        }
        public static void AddState(GameObject key, bool state, AdPosition position, bool isFullWidth, bool isHighPriority = false)
        {
            //bannerController.Add(new Tuple<GameObject, bool, AdPosition>(key, state, position)); 
            if (isHighPriority)
            {
                highPriorityBannerController.Add(new Tuple<GameObject, bool, AdPosition, bool>(key, state, position, isFullWidth));
            }
            else
            {
                bannerController.Add(new Tuple<GameObject, bool, AdPosition, bool>(key, state, position, isFullWidth));
            }
            LoadLastState();
        }

        public static void RemoveState(GameObject key)
        {
            //for (int i = 0; i < bannerController.Count; i++)
            //{
            //    if (bannerController[i].Item1 == key)
            //    {
            //        bannerController.RemoveAt(i);
            //        break;
            //    }
            //}

            var index = bannerController.FindIndex(x => x.Item1 == key);
            if (index != -1)
            {
                bannerController.RemoveAt(index);
            }
            else
            {
                index = highPriorityBannerController.FindIndex(x => x.Item1 == key);
                if (index != -1)
                {
                    highPriorityBannerController.RemoveAt(index);
                }
            }

            LoadLastState();
        }

        public static void LoadBanner(bool isFullWidth = false)
        {
            var bannerType = isFullWidth ? bannerViewFullWidth : bannerView;
            if (isFullWidth)
            {
                bannerView.Hide();
                bannerViewFullWidth.Show();
                CurrentState = true;
                return;
            }

            bannerViewFullWidth.Hide();


            if (bannerView != null)
            {
                //bannerView.Destroy();
                //bannerView = null;
                //bannerView.Show();
                SetBannerState(true);
                return;
            }
            // create an instance of a banner view first.  
            bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
            //      bannerView.OnAdPaid -= Analytics_SendEvent_ViewAds;
            bannerView.OnAdPaid += Analytics_SendEvent_ViewAds;
            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            //adRequest.Extras.Add("collapsible", "bottom");
            //adRequest.Extras.Add("collapsible_request_id", Guid.NewGuid().ToString());
            bannerView.LoadAd(adRequest);
            CurrentState = true;
        }

        public static void Analytics_SendEvent_ViewAds(AdValue arg2) => Analytics_SendEvent_ViewAds(arg2, _adUnitId, "banner_collapse");

        public static void Analytics_SendEvent_ViewAds(AdValue arg2, string ad_unit, string ad_format = "banner_collapse")
        {
            try
            {
                var impressionParameters = new[]
                {
                    new Parameter("ad_platform", com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceAdMob),
                    new Parameter("ad_source", "admob"),
                    new Parameter("ad_unit_name", ad_unit),
                    new Parameter("ad_format", ad_format),
                    new Parameter("value", arg2.Value/1000000f),
                    new Parameter("currency", arg2.CurrencyCode), // All AppLovin revenue is sent in USD
                };

                GlobalData.Instance.WaitNextFrame(() =>
                {
                    ServiceManager.TryLog("ad_impression", impressionParameters);
                    AdImpressionAdjust(arg2);
                    //FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
                });
                //GlobalData.Instance.WaitNextFrame(
                //    () => { FirebaseAnalytics.LogEvent("ad_impression", impressionParameters); });
            }

            catch (Exception)
            {
            }
        }


        static void AdImpressionAdjust(AdValue adValue)
        {
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            adjustAdRevenue.setRevenue(adValue.Value / 1000000f, "USD");
            adjustAdRevenue.setAdRevenueNetwork("admob");
            adjustAdRevenue.setAdRevenueUnit(_adUnitId);
            adjustAdRevenue.setAdRevenuePlacement("admob");
            Adjust.trackAdRevenue(adjustAdRevenue);
        }

        public static void HideBanner()
        {
            if (bannerView != null)
            {
                SetBannerState(false);
                //bannerView.Hide();
                //bannerView.Destroy();
            }
            bannerViewFullWidth.Hide();
        }


        //public void ShowBanner()
        //{
        //    if (ServiceManager.Instance.IsStaging || !IAPProductDefine.IsAdAllowed) return;
        //    LoadBanner();
        //    //bannerView.Show();
        //}

        static bool CurrentState = false;
        public static bool SetBannerState(bool state)
        {
            if (CurrentState == state)
            {
                return false;
            }
            CurrentState = state;
            if (state)
            {
                bannerView.Show();
            }
            else
            {
                bannerView.Hide();
            }
            return true;
        }


#else
        public void ShowBanner()
        {
        }

        public static void HideBanner()
        {
        }
#endif




    }
}