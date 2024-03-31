using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
// using EasyMobile;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649
public class ShopManagerDua : MonoBehaviour
{
    [SerializeField] private string VersioName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;
    [SerializeField] private GameObject StartButton;
private void Awake()
 {
    
     PhotonNetwork.ConnectUsingSettings();
     PhotonNetwork.GameVersion = this.VersioName;
      ///PhotonNetwork.ConnectUsingSettings(VersioName);
 }
    
   

    
    private void Start()
    {
        Time.timeScale = 1;   
        Application.targetFrameRate = 60;
        UsernameMenu.SetActive(true);
      	// in case we started this demo with the wrong scene being active, simply load the menu scene
			if (PhotonNetwork.IsConnected)
			{
				
    Debug.Log("Connected");
				
			}else
            {
  Debug.Log("Gak Konek");
            }	// in case we started this demo with the wrong scene being active, simply load the menu scene
			
    }
private void OnConnectedToMaster()
{
    PhotonNetwork.JoinLobby(TypedLobby.Default);
    Debug.Log("Connected");
}

public void ChangeUserNameInput()
{
    if (UsernameInput.text.Length >=3)
    {
            StartButton.SetActive(true);
    }
    else
    {
         StartButton.SetActive(false);
    }
}

public void SetUsername()
{
    UsernameMenu.SetActive(false);
    PhotonNetwork.NickName = UsernameInput.text;
  
}

public void CreateGame()
{
    PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() {MaxPlayers=5}, null);
       if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				OnJoinedRoom();
			}
}
public void JoinGame()
{
    RoomOptions roomOptions = new RoomOptions();
    roomOptions.MaxPlayers=5;
    PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
     if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				OnJoinedRoom();
			}
}

private void OnJoinedRoom()
{
   
 SceneManager.LoadScene("InGame");
}
    public void KembaliMenu ()
    {
          
         
       
      
          SceneManager.LoadScene("MainMenu");
           
           
            
    }    

    public void KeluarGame ()
    {
       
         Application.Quit();
           
           
            
    }  

    
    

}
}