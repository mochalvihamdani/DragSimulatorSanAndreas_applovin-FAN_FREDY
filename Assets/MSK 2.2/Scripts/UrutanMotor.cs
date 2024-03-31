using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrutanMotor : MonoBehaviour
{
    public static UrutanMotor instance;
    public int currentWaypoint;
    public int currentLap;
    public Transform wpSebelumnya;
    private void Start()
    {
       
     Initialize();
    }
    public void Initialize()
    {
      wpSebelumnya= GetComponent<BikeControlAI>().waypoints[0];
        currentWaypoint = 0;
        currentLap = 0;
      Debug.Log(wpSebelumnya);
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Nabrak Waypoint");
        
    //     if (other.CompareTag("WayPoint"))
    //         {
    //         int wpTerakhir = BikeControlAI.Instance.waypoints.Count -1;
    //         WayPoint wp = other.GetComponent<WayPoint>();
    //      Debug.Log("CheckPoint");
    //         if (wp.urutan == BikeControlAI.Instance.lastWaypointIndex && currentWaypoint == wpTerakhir - 1)
    //         {
    //             //CompleteLap
    //             currentLap++;
    //             currentWaypoint = 0;
    //             Debug.Log("Lap Complete");
              
    //             if (currentLap >= BikeCamera.Instance.lapTujuan)
    //             {
    //                 currentWaypoint = wpTerakhir;
    //                 Debug.Log("Menang");
    //                 //MainManager.Instance.Menang(this);
    //             }
    //         }
    //         if (!wp.jalanPintas)
    //         {
    //             if (currentWaypoint == wp.urutan - 1)
    //             {
    //                 currentWaypoint = wp.urutan;
    //                 wpSebelumnya = other.transform;
    //             }
    //             //if (currentWaypoint ==)
    //         }
    //         else {
    //             currentWaypoint = wp.urutan;
    //             wpSebelumnya = other.transform;
    //         }
    //     }
     
    // }
    public float GetDistance()
    {

        if (wpSebelumnya)
        {
            //Debug.Log(wpSebelumnya);
            return (transform.position - wpSebelumnya.position).magnitude + currentWaypoint * 100 + currentLap * 1000;
        }
        else
        {
            return 0;
        }
    }
    public int GetUrutanMotor(UrutanMotor[] semuaMotor)
    {
         
        float distance = GetDistance();
        int position = 1;
        foreach (UrutanMotor motor in semuaMotor)
        {
            if (motor.GetDistance() > distance)
            {
                position++;
            }
           
        }
        return position;
    }
}
