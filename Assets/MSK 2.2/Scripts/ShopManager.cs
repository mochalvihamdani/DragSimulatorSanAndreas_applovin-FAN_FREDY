using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
// using EasyMobile;

// using AudienceNetwork;
// using AudienceNetwork.Utility;
using DG.Tweening;


public class ShopManager : MonoBehaviour
{

   
      [Header("KameraSinematik")]
  [SerializeField]  public GameObject[] Kamera;
  [SerializeField] public List<RectTransform>TombolAtas;
  [SerializeField] public List<RectTransform>TombolManual;
[SerializeField] public List<RectTransform>TombolBawah;
[Header("Bounty")]
 [SerializeField]
  public Text BountyKamu;
public int BountyCount;
 [Header("Uang")]
  [SerializeField]
    public Text UangKamu;
    public Text NamaItem;
    public Text StatusItem3DText;
    public int uangCount;
    [Header("Per Tombolan Scene")]
  [SerializeField]
     public Button buyButton;
    public Button playButton;
    public Button RaceButton;
    public Button MabarButton;
    [Header("Index Array")]
  [SerializeField]
    public int currentMotorIndex=0;
     public int currentitem3DScene=0;
     [Header("Per Motoran")]
  [SerializeField]
  public float SumbuX;
    public GameObject[] motorModels;
    public Transform spawnItem3DScene;
    public Transform[] Item3DScene;
    public MotorData[] MotorDataList;
    //  public List<GameObject> motorObjects = new List<GameObject>();
    // public UrutanMotor[] semuaMotor;
    // public UrutanMotor[] urutanMotor;



 void Awake()
    {
     
     
    }
    
    void Start()
    {
       //AudienceNetworkAds.Initialize();
      //MaxSdk.InitializeSdk();
   
        StartCoroutine(CountdownCoroutine());
     
        Time.timeScale = 1;   
        Application.targetFrameRate = 60;
        uangCount = PlayerPrefs.GetInt("Uang", 0);
        BountyCount = PlayerPrefs.GetInt("Bounty", 0);
        foreach(MotorData motor in MotorDataList)
        {
            if (motor.harga == 0)
            motor.isUnlocked = true;
            else
              motor.isUnlocked = PlayerPrefs.GetInt(motor.name, 0) == 0 ?false: true;
        }
        currentMotorIndex = PlayerPrefs.GetInt("SelectedMotor",0);
        currentitem3DScene = PlayerPrefs.GetInt("SelectedItem3D",0);
         

        foreach(GameObject motor in motorModels)
        motor.SetActive(false);
        motorModels[currentMotorIndex].SetActive(true);
         SpawnItem3DScene();
     
    
    }

  
IEnumerator CountdownCoroutine() {
         float ratiox = (float) Screen.height / 1920f ;
 float ratioy = (float) Screen.width / 1080f ;
        Kamera[0].SetActive(true);
        Kamera[1].SetActive(false);
        Kamera[2].SetActive(false);
        Kamera[3].SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Kamera[0].SetActive(false);
        Kamera[1].SetActive(true);
        Kamera[2].SetActive(false);
        Kamera[3].SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Kamera[0].SetActive(false);
        Kamera[1].SetActive(false);
        Kamera[2].SetActive(true);
        Kamera[3].SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Kamera[0].SetActive(false);
        Kamera[1].SetActive(false);
        Kamera[2].SetActive(false);
        Kamera[3].SetActive(true);
        // for (int i=0; i < TombolAtas.Count; i++) 
        //   {  
        //       TombolAtas[i].DOMoveY(ratioy*550, 0.6f).SetEase(Ease.OutBack);
        //   }
         
        //   for (int i=0; i < TombolBawah.Count; i++)
        //   {
        //       TombolBawah[i].DOMoveY(145,0.6f).SetEase(Ease.OutBack);
        //   }
       
        // start the game here
      //   yield return new WaitForSeconds(1.0f);
      //   //  TombolManual[0].DOMoveX(145,0.6f).SetEase(Ease.OutBack);
      //   //  TombolManual[1].DOMoveY(ratioy*260,0.6f).SetEase(Ease.OutBack);
      //   //  TombolManual[2].DOMoveY(ratioy*400,0.6f).SetEase(Ease.OutBack);
      //   //  TombolManual[3].DOMoveY(655,0.6f).SetEase(Ease.OutBack);
      //   //  TombolManual[4].DOMoveX(125,0.6f).SetEase(Ease.OutBack);
      //  yield break;
    }
    public void SpawnItem3DScene()
    {
           
        //  int currentitemIndex = PlayerPrefs.GetInt("SelectedItem3D");
        // Transform prefabItem3D = Item3DScene[currentitemIndex];
        //  PlayerPrefs.GetInt("SelectedItem3D", currentitemIndex);
        // //motorDipilih[currentMotorIndex].SetActive(true);
        // spawnItem3DScene = Instantiate (prefabItem3D); //spawn player posisi di gameobject spawn
        //     bool pawanghujandibeli;
        //      pawanghujandibeli = bool.Parse(PlayerPrefs.GetString("pawangdibeli", "false"));
        //    Debug.Log(pawanghujandibeli);
        //  if(pawanghujandibeli==true)
        //  {
        //       spawnItem3DScene.gameObject.SetActive(true);
        //      //Debug.Log("PAWANG TERBELI");
        //  }
        //  else
        //  {
        //       spawnItem3DScene.gameObject.SetActive(false);
        //       // Debug.Log("PAWANG BLOM");
        //  }
        
    //       if(pawanghujandibeli==true)
    //    {
   
    //    }else{
    //      prefabItem3D.gameObject.SetActive(false);
    //    }
    }
public void IncreaseUang()
{
     if (Input.GetKeyDown(KeyCode.F))
     {
    uangCount += 50; 
    BountyCount +=50;
     PlayerPrefs.SetInt("Uang", PlayerPrefs.GetInt("Uang",0)+uangCount);
    PlayerPrefs.SetInt("Bounty", PlayerPrefs.GetInt("Bounty",0)+BountyCount);
    PlayerPrefs.Save();
    //Debug.Log("Save Dan Tambah Uang");
     }
   
}
    // Update is called once per frame
    void Update()
    {
        

        IncreaseUang();
        UpdateUI();
        UangKamu.text = "Uang: " + PlayerPrefs.GetInt("Uang", 0);
        BountyKamu.text = " " + PlayerPrefs.GetInt("Bounty", 0);
    }

  public void unlockMotor()
  {
 
       MotorData m = MotorDataList[currentMotorIndex];
       PlayerPrefs.SetInt (m.name, 1);
       PlayerPrefs.SetInt("SelectedMotor", currentMotorIndex);
       m.isUnlocked=true;
       PlayerPrefs.SetInt("Uang", PlayerPrefs.GetInt("Uang",0) -m.harga);
       PlayerPrefs.Save();
           
            
    }
    public void ChangeNext()
    {
        motorModels[currentMotorIndex].SetActive(false);
        currentMotorIndex++;
        if (currentMotorIndex==motorModels.Length)
        currentMotorIndex=0;
        motorModels[currentMotorIndex].SetActive(true);
         motorModels[currentMotorIndex].transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), .25f);
        MotorData m = MotorDataList[currentMotorIndex];
          if(!m.isUnlocked)
          return;
         PlayerPrefs.SetInt("SelectedMotor",currentMotorIndex);
            //   if (Advertisements.IsInterstitialAdReady())
            // {
            //     Advertisements.ShowInterstitialAd();
            // }
    }

     public void ChangePrevious()
    {
        motorModels[currentMotorIndex].SetActive(false);
        currentMotorIndex--;
        if (currentMotorIndex== -1)
        currentMotorIndex= motorModels.Length -1;
        motorModels[currentMotorIndex].SetActive(true);
         motorModels[currentMotorIndex].transform.DOPunchScale (new Vector3 (0.2f, 0.2f, 0.2f), .25f);
          MotorData m = MotorDataList[currentMotorIndex];
          if(!m.isUnlocked)
          return;
         PlayerPrefs.SetInt("SelectedMotor",currentMotorIndex);
       
        }

    private void UpdateUI()
    {
        
        MotorData m = MotorDataList[currentMotorIndex];
         NamaItem.text=m.name;
        if (m.isUnlocked==true)
        {
             StatusItem3DText.color=Color.blue;
            StatusItem3DText.text="Sudah di Beli";
            playButton.gameObject.SetActive(true);
            RaceButton.gameObject.SetActive(true);
            MabarButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
             
        }
        else
        {
            StatusItem3DText.color=Color.red;
            StatusItem3DText.text="Uang kamu belum cukup";
            RaceButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            MabarButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<Text>().text="Buy -"+ m.harga;
            if (m.harga >= PlayerPrefs.GetInt("Uang"))
            {
                buyButton.interactable = false;
                //Debug.Log("Uang Kamu Gak Cukup");
                    //Debug.Log(PlayerPrefs.GetInt("Uang", 0));
            }
            else
            {
               
             StatusItem3DText.color=Color.yellow;
             StatusItem3DText.text="Bisa di Beli !";
                buyButton.interactable = true;
                   //Debug.Log("Uang Kamu  Cukup");
            }
        }
    }
     

    public void FreeRide ()
    {
          
         
       
         PlayerPrefs.SetInt("SelectedMotor",currentMotorIndex);
          SceneManager.LoadScene("Simple");
           
           
            
    }    

    public void KeluarGame ()
    {
       
         Application.Quit();
           
           
            
    }  

    public void RaceMode ()
    {
      
         PlayerPrefs.SetInt("SelectedMotor",currentMotorIndex);
          SceneManager.LoadScene("Simple");
           
    }    
    

}
