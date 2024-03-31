using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorSelector : MonoBehaviour
{
    public int currentMotorIndex=0;
    public GameObject[] motorDipilih;
    void Start()
    {
        currentMotorIndex = PlayerPrefs.GetInt("SelectedMotor", 0);
        foreach(GameObject motor in motorDipilih)
        motor.SetActive(false);
        motorDipilih[currentMotorIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
