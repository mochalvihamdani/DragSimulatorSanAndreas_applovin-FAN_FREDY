using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* SUBSCRIBING TO MY YOUTUBE CHANNEL: 'VIN CODES' WILL HELP WITH MORE VIDEOS AND CODE SHARING IN THE FUTURE :) THANK YOU */

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    public BosData[] BosDataList;
    public int indexBosSaatini=0;
    public Text BountyKamu;
    public Text[] LevelInfo;
    public int BountyCount;
    // Start is called before the first frame update
    void Start()
    {
    BountyCount = PlayerPrefs.GetInt("Bounty", 0);
     
           foreach(BosData bos in BosDataList)
        {
            Debug.Log(bos.IDbos);
          
            if (bos.bounty <= PlayerPrefs.GetInt("Bounty"))
            {
            lvlButtons[bos.IDbos].interactable=true;
            bos.isUnlocked = true;
                LevelInfo[bos.IDbos].text = bos.bounty.ToString()+", Bounty kamu cukup";
            
            }
            else
            {
              LevelInfo[bos.IDbos].text = bos.bounty.ToString()+", Bounty kamu blm cukup";
            lvlButtons[bos.IDbos].interactable=false;
            bos.isUnlocked = PlayerPrefs.GetInt(bos.namabos, 0) == 0 ?false: true;
            }
        }
        indexBosSaatini = PlayerPrefs.GetInt("SelectedBos",0);
        
       
    }

void Update()
{
  
      BountyKamu.text = "Bounty Kamu :" + PlayerPrefs.GetInt("Bounty", 0);
  
}

}
