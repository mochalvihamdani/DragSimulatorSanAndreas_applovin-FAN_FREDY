using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static WayPoints instance;
  public List<Transform> waypoints = new List<Transform>();
  

    // private void Awake()
    // {
    //     foreach (Transform t in transform)
    //     {
    //         waypoints.Add(t);
    //     }
    //     for (int i = 0; i < waypoints.Count; i++)
    //     {
    //         wp.Add(waypoints[i].GetComponent<WayPoint>());
    //         wp[i].urutan = i;
    //     }
    // }
}
