using UnityEngine;
using System.Collections;

public class BikeSwitch : MonoBehaviour
{

    public Transform[] Bikes;
    public Transform MyCamera;


    public void CurrentBikeActive(int current)
    {

        int amount = 0;

        foreach (Transform Bike in Bikes)
        {
            if (current == amount)
            {
                MyCamera.GetComponent<Photon.Pun.Demo.PunBasics.BikeCameraOnline>().target = Bike;

                MyCamera.GetComponent<Photon.Pun.Demo.PunBasics.BikeCameraOnline>().Switch = 0;
                MyCamera.GetComponent<Photon.Pun.Demo.PunBasics.BikeCameraOnline>().cameraSwitchView = Bike.GetComponent<BikeControl>().bikeSetting.cameraSwitchView;
                MyCamera.GetComponent<Photon.Pun.Demo.PunBasics.BikeCameraOnline>().BikerMan = Bike.GetComponent<BikeControl>().bikeSetting.bikerMan;

                Bike.GetComponent<Photon.Pun.Demo.PunBasics.BikeControlOnline>().activeControl = true;
            }
            else
            {
                Bike.GetComponent<Photon.Pun.Demo.PunBasics.BikeControlOnline>().activeControl = false;
            }

            amount++;
        }
    }




}
