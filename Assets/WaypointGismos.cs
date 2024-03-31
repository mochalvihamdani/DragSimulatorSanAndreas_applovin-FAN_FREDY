using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGismos : MonoBehaviour
{
    // Start is called before the first frame update
     void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
