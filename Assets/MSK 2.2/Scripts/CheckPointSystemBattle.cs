using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class CheckPointSystemBattle : MonoBehaviour
{
public static CheckPointSystemBattle Instance;
 [Header("Posisi Juara")]
  [SerializeField]
public bool juarapertama = false;
public bool juarakedua = false;

 public float []DistanceArrays;

    [Header ("Motor = Player Cars01")]
    public Transform Car01;
    public Transform Car02;
   
   public float  Car01Dist;
   public float Car02Dist;
 
 public GameObject[] motorModels;
 


   public float First;
    //float Fourth;
    public float Second;
    public float Third;

    [Header("UI")]
    public TextMeshProUGUI PlayerTextFinish;
    public TextMeshProUGUI Car01Text;
    public TextMeshPro Car02Text;
   
    //public TextMeshPro Car04Text;

    public GameObject NextCheckPoint;

    public int JuaraPosisi;
    void Start()
    {   
     
        NextCheckPoint.SetActive(false);
    // FinishLine.Instance.FinishState=false;
         JuaraPosisi = PlayerPrefs.GetInt("JuaraPosisi", 0);
    }




    
    void Update()
    {

        GameObject[] MotorAI = GameObject.FindGameObjectsWithTag("BOT");
         motorModels = MotorAI;
        

        GameObject  Playerini = GameObject.FindGameObjectWithTag("Player");
        Car01 = Playerini.transform;
        Car02 = MotorAI[0].transform;
       // Car03 = MotorAI[1].transform;

        Car02Text = MotorAI[0].transform.Find("PosisiText").GetComponent<TextMeshPro>();
      //  Car03Text = MotorAI[1].transform.Find("PosisiText").GetComponent<TextMeshPro>();
        DistanceArrays[0] = Vector3.Distance(transform.position, Car01.position);
        DistanceArrays[1] = Vector3.Distance(transform.position, Car02.position);
       // DistanceArrays[2] = Vector3.Distance(transform.position, Car03.position);
        
       

        Array.Sort(DistanceArrays);

        First = DistanceArrays[0];
        Second = DistanceArrays[1];
        //Third = DistanceArrays[2];
        //Fourth = DistanceArrays[3];

         Car01Dist = Vector3.Distance(transform.position, Car01.position);
         Car02Dist = Vector3.Distance(transform.position, Car02.position);
         //Car03Dist = Vector3.Distance(transform.position, Car03.position);
        //float Car04Dist = Vector3.Distance(transform.position, Car04.position);

        #region Car01UI 
        if (Car01Dist == First) {
            Car01Text.text = "1/2";
            juarapertama=true;
            juarakedua=false;
            
            PlayerPrefs.SetInt("JuaraPosisi",1);
           PlayerPrefs.Save();
                      PlayerTextFinish.text="Kamu Juara 1 ! Selamat";
                     
        
   
        }
        if (Car01Dist == Second)  

        {
            juarapertama=false;
            juarakedua=true;
          
            Car01Text.text = "2/2";
             PlayerPrefs.SetInt("JuaraPosisi",2);
             PlayerPrefs.Save();
                      PlayerTextFinish.text="Yaaa Kamu Tidak Juara ! ";
               
   
              
        }
        // if (Car01Dist == Third)
        // {
        //     juarapertama=false;
        //     juarakedua=false;
        

        //     Car01Text.text = "3/3";
        //     PlayerPrefs.SetInt("JuaraPosisi",3);
        //     PlayerPrefs.Save();
                 
        //               PlayerTextFinish.text="Kamu Ketiga Gapapa tar juga jago ! Selamat kamu dapat 150 Coin";
              


        // }








        
        // if (Car01Dist == Fourth)
        // {
        //     Car01Text.text = "4/4";
        // }
        #endregion

        #region Car02UI
        if (Car02Dist == First)
        {
            Car02Text.text = "1st";
        }
        if (Car02Dist == Second)
        {
            Car02Text.text = "2nd";
        }
        // if (Car02Dist == Third)
        // {
        //     Car02Text.text = "3rd";
        // }
        // if (Car02Dist == Fourth)
        // {
        //     Car02Text.text = "4th";
        // }
        #endregion

        // #region Car03UI
        // if (Car03Dist == First)
        // {
        //     Car03Text.text = "1st";
        // }
        // if (Car03Dist == Second)
        // {
        //     Car03Text.text = "2nd";
        // }
        // if (Car03Dist == Third)
        // {
        //     Car03Text.text = "3rd";
        // }
        // if (Car03Dist == Fourth)
        // {
        //     Car03Text.text = "4th";
        // }
        // #endregion

        // #region Car04UI
        // if (Car04Dist == First)
        // {
        //     Car04Text.text = "1st";
        // }
        // if (Car04Dist == Second)
        // {
        //     Car04Text.text = "2nd";
        // }
        // if (Car04Dist == Third)
        // {
        //     Car04Text.text = "3rd";
        // }
        // // if (Car04Dist == Fourth)
        // // {
        // //     Car04Text.text = "4th";
        // // }
        // #endregion

    }
  
  
private void OnTriggerEnter(Collider other)
    {
      
       
     if (other.CompareTag("Player")) 
        {
       
           // Debug.Log("Player!");
            NextCheckPoint.SetActive(true);
            gameObject.SetActive(false);
        }
    else if (other.CompareTag("BOT"))
         {        
                 
             //Debug.Log("BOT!");
            NextCheckPoint.SetActive(true);
            gameObject.SetActive(false);
         }
   
      

}
}
   

    

