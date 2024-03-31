using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
namespace Photon.Pun.Demo.PunBasics
{
public class ChatManager : MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable
{
   public Player plMove;
   public PhotonView photonView; 
   public GameObject BubleChat;
   public TextMeshProUGUI UpdatedText;
   public  InputField  ChatInputField;
   public Button EnterChat;
   private bool DisableSend;

private void Awake()
{
    ChatInputField = GameObject.Find("ChatInputField").GetComponent<InputField>();
    EnterChat = GameObject.Find("EnterChat").GetComponent<Button>();
}

public void SendChat()
{
 
           photonView.RPC("SendMessage", Photon.Pun.RpcTarget.AllBuffered, ChatInputField.text);
           BubleChat.SetActive(true);
           DisableSend=true;
}
  private void Update()
   {
       if (photonView.IsMine)
       {
           if(!DisableSend && ChatInputField.isFocused)
           {
               if(ChatInputField.text !="" && ChatInputField.text.Length > 0)
               {
                   EnterChat.onClick.AddListener(SendChat);

                  
               }
           }
       }
   }

 #region IPunObservable implementation
[PunRPC]
public void SendMessage(string message)
{
    UpdatedText.text = message;
    StartCoroutine("Remove");
}
IEnumerator Remove()
{
    yield return new WaitForSeconds(4f);
    BubleChat.SetActive(false);
    DisableSend = false;
}

public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
{
    if(stream.IsWriting)
    {
        stream.SendNext(BubleChat.active);
    }else if(stream.IsReading)
    {
        BubleChat.SetActive((bool)stream.ReceiveNext());
    }

}



}



}
   #endregion