using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
 
public class ShowObjectOnDistance : MonoBehaviour
{
    #region public
    [Header("Objek yang Nonggol")]
    public GameObject[] ObjekToShow;
 
    [Header("Params")]
    public float ShowAtDistance = 10f;
    public float checkEachSec   = 2f;
    public Transform TargetTrans;
 

    #endregion
 
    #region private
    private Transform localTrans;
    private bool checkingActive = true;
    #endregion
 
 
    void Start()
    {

            GameObject pl = GameObject.FindGameObjectWithTag("Player");
            
                TargetTrans = pl.transform;
       
 
        //Caching for performance..
        localTrans = this.transform;
  
        StartCoroutine(WaitForNextFrame());
    }
 void Update()
 {

 }
    IEnumerator WaitForNextFrame()
    {
      
        yield return null;
        while (checkingActive)
        {
            yield return new WaitForSeconds(checkEachSec);
 
            if (TargetTrans && localTrans)
            {
                float dist = Vector3.Distance(localTrans.position, TargetTrans.position);
 
             
    
        for (int i = 0; i < ObjekToShow.Length; i++)
        {
              if (dist < ShowAtDistance){
             ObjekToShow[i].SetActive(true);
            }
            else
            {
                   ObjekToShow[i].SetActive(false);
            }
         
        }
               
                 
            }
        }
    }
 
}