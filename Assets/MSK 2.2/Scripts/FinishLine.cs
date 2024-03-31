using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using EasyMobile;

public class FinishLine : MonoBehaviour
{
    public static FinishLine Instance;
    public GameObject PopupFinish;
    
public bool FinishState=false;


     void Awake()
    {
    

        
    }
    
    void Start()
    {
    PopupFinish = GameObject.Find("PopupFinish");
    PopupFinish.transform.GetChild(0).gameObject.SetActive(false);
     int JuaraPosisi = PlayerPrefs.GetInt("JuaraPosisi", 0);       
   
  //  MaxSdk.InitializeSdk();
   
    }

     private void Update()
    {
          
    }

  

   
     
    void OnTriggerEnter(Collider col) {
      if(col.gameObject.name=="FinishLine")
      {
        
       Time.timeScale = 0;
       Debug.Log("FINISH");
       
      PopupFinish.SetActive(true);
      PopupFinish.transform.GetChild(0).gameObject.SetActive(true);
         
        int JuaraPosisi = PlayerPrefs.GetInt("JuaraPosisi");
    Debug.Log(JuaraPosisi);
      
           if(JuaraPosisi == 1){
               
           int uangCount = PlayerPrefs.GetInt ("Uang");
           int BountyCount = PlayerPrefs.GetInt ("Bounty");
            PlayerPrefs.SetInt("Uang", uangCount + 225);
            PlayerPrefs.SetInt("Bounty", BountyCount + 0);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("Uang", 0));
   
          }
          if (JuaraPosisi==2) {
             
               
           int uangCount = PlayerPrefs.GetInt ("Uang");
           int BountyCount = PlayerPrefs.GetInt ("Bounty");
            PlayerPrefs.SetInt("Uang", uangCount + 50);
            PlayerPrefs.SetInt("Bounty", BountyCount + 0);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("Uang", 0));
   
          }
            if (JuaraPosisi==3) {
             
               
           int uangCount = PlayerPrefs.GetInt ("Uang");
           int BountyCount = PlayerPrefs.GetInt ("Bounty");
            PlayerPrefs.SetInt("Uang", uangCount + 0);
            PlayerPrefs.SetInt("Bounty", BountyCount + 0);
            PlayerPrefs.Save();
            //Debug.Log(PlayerPrefs.GetInt("Uang", 0));
   
          }
     
      }
  }
}
