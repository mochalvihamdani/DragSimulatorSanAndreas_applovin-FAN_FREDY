using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    BikeCamera bc;
    public int urutan;
    public bool jalanPintas;
    public Vector3[] tempat = new Vector3[5];

    private void Start()
    {
        SpawnTempat();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
    public void SpawnTempat()
    {
        tempat = new Vector3[5];
        float lebar = 10;
      
        tempat[0] = transform.TransformPoint(transform.right * -lebar);
        tempat[1] = transform.TransformPoint(transform.right * (-lebar/2));
        tempat[2] = transform.position;
        tempat[3] = transform.TransformPoint(transform.right * (lebar/2));
        tempat[4] = transform.TransformPoint(transform.right * lebar);
    }
    public Vector3 PilihTarget(int index)
    {
        return tempat[index];
    }
}
