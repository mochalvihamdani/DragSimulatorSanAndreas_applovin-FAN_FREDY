
using UnityEngine;
using UnityEngine.UI;
public class AdsManager : MonoBehaviour
{
  //  private AudienceNetwork.InterstitialAd interstitialAdFAN;
    private static int thisGameCoins = 0;
    public static AdsManager instance;
  //  public GameObject activeSpawn;
   // public GameObject[] SpawnRewards;
   public GameObject PopupTutup;
    private void Awake()
    {
        
          //  Advertisements.Instance.Initialize();
        
        // RewardCars.gameObject.SetActive(false);
       // Advertisements.Instance.Initialize();
        instance = this;
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
        Advertisements.Instance.Initialize();
    }
    private void Start()
    {
        Advertisements.Instance.Initialize();
    }
   public void Tutup()
    {
        PopupTutup.gameObject.SetActive(false);
    }
   
    public void RequestAd()
    {
        Debug.Log("Load Iklan");
        Advertisements.Instance.ShowInterstitial(InterstitialClosed);
    }
    public void RewardAds() 
    {
        Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
    }

    
    // public void RewardAdsCars()
    // {
    //     Debug.Log("Panggil Show Reward Cars");
    //     Debug.Log("Panggil Show Reward Cars");
    //     Advertisements.Instance.ShowRewardedVideo((completed, advertiser) => CompleteMethodCars(completed, advertiser));
    //     // Advertisements.Instance.ShowRewardedVideo(CompleteMethodCars);
    // }
    // private void CompleteMethodCars(bool completed, string advertiser)
    // {

    //     Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
    //     GleyMobileAds.ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);

    //     if (completed)
    //     {
    //         Debug.Log("KONTOL");

    //         // Cari objek dengan tag "SpawnRewards"
    //         GameObject[] spawnRewards = GameObject.FindGameObjectsWithTag("SpawnRewards");

    //         // Loop melalui semua elemen dengan tag "SpawnRewards"
    //         foreach (GameObject objekSpawn in spawnRewards)
    //         {
    //             // Periksa apakah objekSpawn tidak null dan aktif
    //             if (objekSpawn != null && objekSpawn.activeSelf)
    //             {
    //                 // Dapatkan komponen spawnObject dari objekSpawn
    //                 spawnObject spawnComponent = objekSpawn.GetComponent<spawnObject>();

    //                 // Periksa apakah spawnComponent tidak null
    //                 if (spawnComponent != null)
    //                 {
    //                     // Lakukan activateSpawnObject hanya pada elemen yang aktif
    //                     spawnComponent.activateSpawnObject();
    //                 }
    //                 else
    //                 {
    //                     Debug.LogError("Objek dengan tag 'SpawnRewards' tidak memiliki komponen spawnObject.");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError("Objek dengan tag 'SpawnRewards' tidak ditemukan atau tidak aktif.");
    //             }
    //         }
    //     }


    //     //foreach (GameObject spawnReward in SpawnRewards)
    //     //{
    //     //    if (spawnReward.activeSelf)
    //     //    {
    //     //        Debug.Log("SPAWN ANJING");
    //     //        spawnObject spawnComponent = spawnReward.GetComponent<spawnObject>();
    //     //        if (spawnComponent != null)
    //     //        {
    //     //        activeSpawn.GetComponent<spawnObject>().activateSpawnObject();
    //     //            //spawnComponent.activateSpawnObject();
    //     //        }
    //     //        else
    //     //        {
    //     //            Debug.LogError("Elemen " + spawnReward.name + " tidak memiliki komponen spawnObject.");
    //     //        }
    //     //    }
    //     //}

    //     //// Jika ditemukan elemen yang aktif, lakukan activateSpawnObject pada elemen tersebut
    //     //if (activeSpawn != null)
    //     //{
    //     //    activeSpawn.GetComponent<spawnObject>().activateSpawnObject();
    //     //    activeSpawn = null;  // Atur kembali ke null setelah digunakan
    //     //}

    //     else
    //         {
    //             Debug.Log("Gak Dapat Reward");
    //         }
        
    // }


    //callback called each time an interstitial is closed
    private void InterstitialClosed(string advertiser)
    {
        if (Advertisements.Instance.debug)
        {
            Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
            GleyMobileAds.ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
        }
    }
    
    //callback called each time a rewarded video is closed
    //if completed = true, rewarded video was seen until the end
    private void CompleteMethod(bool completed, string advertiser)
    {
        if (completed)
        {

            Rewards(thisGameCoins);
           
        }
        else
        {
            Debug.Log("Gak Dapat Reward");
        }
    }
    public void Rewards(int thisGameCoins)
    {
        thisGameCoins += 1000;
        PlayerPrefs.SetInt("Uang", PlayerPrefs.GetInt("Uang", 0) + thisGameCoins);
        PlayerPrefs.Save();
    }
    public void KeluarGame()
    {
        Application.Quit();
    }
}
