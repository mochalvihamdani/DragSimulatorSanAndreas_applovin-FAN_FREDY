using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
public class FollowPlayer : MonoBehaviour
{
    public GameObject tPlayer;
    public Transform tFollowTarget;
    private CinemachineVirtualCamera vcam;
  
    // Use this for initialization
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null)
            {
                tFollowTarget = tPlayer.transform;
                vcam.m_LookAt = tFollowTarget;
                vcam.m_Follow = tFollowTarget;
         
               
            }
        }
    }
}