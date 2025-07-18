
using UnityEngine;
using UnityEngine.UI;
using System;
public class AdsManager : MonoBehaviour
{
  //  private AudienceNetwork.InterstitialAd interstitialAdFAN;
    private static int thisGameCoins = 5000;
    public static AdsManager instance;
    private int clickCount = 0;
  //  public GameObject activeSpawn;
   // public GameObject[] SpawnRewards;
//    public GameObject PopupTutup;
    private void Awake()
    {
        
          //  Advertisements.Instance.Initialize();
        
        // RewardCars.gameObject.SetActive(false);
       // Advertisements.Instance.Initialize();
        instance = this;
        Gley.MobileAds.API.Initialize();
        // Inisialisasi AdMob
        //MobileAds.Initialize(initStatus => { });

        //// Inisialisasi AppLovin
        //MaxSdk.SetSdkKey(appLovinSdkKey);
        //MaxSdk.InitializeSdk();

        //// Inisialisasi FAN
        //AudienceNetworkAds.Initialize();

        //// Memuat iklan AdMob
        //LoadAdMobInterstitial();
    }
    private void OnEnable()
    {
        // Advertisements.Instance.Initialize();
    }
    private void Start()
    {
        // Advertisements.Instance.Initialize();
    }
//    public void Tutup()
//     {
//         PopupTutup.gameObject.SetActive(false);
//     }
   
     public void RequestAd()
    {
        Debug.Log("Load Iklan");
        Gley.MobileAds.API.ShowInterstitial(InterstitialClosed);
    }

    public void Klick7xmunculiklan()
    {
        clickCount++;

        if (clickCount >= 7)
        {
            Debug.Log("Load Iklan");
            Gley.MobileAds.API.ShowInterstitial(InterstitialClosed);
            clickCount = 0; // Reset clickCount after the ad is displayed
        }
    }

public void Klick10xmunculiklan()
    {
        clickCount++;

        if (clickCount >= 10)
        {
            Debug.Log("Load Iklan");
            Gley.MobileAds.API.ShowInterstitial(InterstitialClosed);
            clickCount = 0; // Reset clickCount after the ad is displayed
        }
    }
    private void InterstitialClosed()
    {
        Debug.Log("Interstitial closed -> Resume Game");
    }

    public void RewardAds()
    {
        // if (claimCount >= claimLimit)
        // {
        //     if (claimTimer <= 0)
        //     {
        //         claimCount = 0;
        //         claimTimer = 0;
        //     }
        //     else
        //     {
        //         Debug.Log("Anda telah mencapai batas klaim. Harap tunggu " + Mathf.Ceil(claimTimer) + " detik untuk klaim berikutnya.");
        //         return;
        //     }
        // }

        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
    }

    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            // claimCount++;

            Rewards(thisGameCoins);
            Debug.Log("Rewarded: " + thisGameCoins + " coins");

        //     if (claimCount >= claimLimit)
        //     {
        //         claimTimer = claimCooldown;
        //         claimCooldownText.text = "Anda telah mencapai batas klaim. Harap tunggu " + claimCooldown + " detik untuk klaim berikutnya.";
        //         Debug.Log("Anda telah mencapai batas klaim. Harap tunggu " + claimCooldown + " detik untuk klaim berikutnya.");
        //     }
        // }
        // else
        // {
        //     Debug.Log("No Reward Received");
         }
    }

    public void Rewards(int coinsToAdd)
    {
        PlayerPrefs.SetInt("Uang", PlayerPrefs.GetInt("Uang", 0) + coinsToAdd);
        PlayerPrefs.Save();
    }
    public void KeluarGame()
    {
        Application.Quit();
    }
}
