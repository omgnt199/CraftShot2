
using System;
using System.Collections.Generic;
using UnityEngine;
using static MaxSdkBase;

public class BannerHolder : MonoBehaviour
{
    public static List<Tuple<GameObject, bool, BannerPosition>> bannerController = new();

    public static void AddState(GameObject key, bool state, BannerPosition position)
    {
        //bannerController.Add(new Tuple<GameObject, bool, BannerPosition>(key, state, position));
        //LoadLastState();
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
        //LoadLastState();
    }

    static void LoadLastState()
    {
        if (bannerController.Count == 0)
        {
            ServiceManager.applovinAds.HideBanner();
            return;
        }
        Tuple<GameObject, bool, BannerPosition> currentState = bannerController[bannerController.Count - 1];
        if (currentState.Item2)
        {
            ServiceManager.applovinAds.ShowBanner();
            ServiceManager.applovinAds.SetBannerPosition(currentState.Item3);
        }
        else
            ServiceManager.applovinAds.HideBanner();
    }

    //public bool bannerState;
    //public BannerPosition bannerPosition;

    //private void OnEnable()
    //{
    //    AddState(gameObject, bannerState, bannerPosition);
    //}
    //public void OnDisable()
    //{
    //    RemoveState(gameObject);
    //}

}