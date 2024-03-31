using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using EasyMobile;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
public class BikeCameraOnline : MonoBehaviourPunCallbacks
{
  
     public static BikeCameraOnline Instance;
    //public WayPoints wp;
    public Transform spawnPlayer;
      [HideInInspector] public bool StartMotorMajuBOT;
     private int currentCamera =0;
    public Transform spawnBOT;
    //public GameObject motorPrefabMusuh;
    // public GameObject[] Item3DScene;
     //public Transform spawnItem3DScene;
    public Transform[] motorModels;
    public List<GameObject> motorObjects = new List<GameObject>();
    public UrutanMotor[] semuaMotor;
    public UrutanMotor[] urutanMotor;

    public UnityEngine.UI.Text urutanText;
    private GameObject motorDipake;
    public Transform target;
    public Transform BikerMan;

    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float height = 1.0f;
    public float Angle = 20;


    public List<Transform> cameraSwitchView;
    public BikeUIClass BikeUI;

    public LayerMask lineOfSightMask = 0;

   



    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    [HideInInspector]
    public int Switch;

    private int gearst = 0;
    private float thisAngle = -150;
    private float restTime = 0.0f;


    private Rigidbody myRigidbody;



    private Photon.Pun.Demo.PunBasics.BikeControlOnline bikeScript;



    [System.Serializable]
    public class BikeUIClass
    {

        public Image tachometerNeedle;
        public Image barShiftGUI;

        public Text speedText;
        public Text GearText;

    }


    


    ////////////////////////////////////////////// TouchMode (Control) ////////////////////////////////////////////////////////////////////


    private int PLValue = 0;

     void Awake()
    {
   

        
    }


    public void PoliceLightSwitch()
    {

        if (!target.gameObject.GetComponent<PoliceLights>()) return;

        PLValue++;

        if (PLValue > 1) PLValue = 0;

        if (PLValue == 1)
            target.gameObject.GetComponent<PoliceLights>().activeLight = true;

        if (PLValue == 0)
            target.gameObject.GetComponent<PoliceLights>().activeLight = false;


    }
    public void MainMenu ()
    {
     Invoke("MainMenu", 5.0f);
          SceneManager.LoadScene("MainMenu");
    }    
 public void updateUrutan()
    {
       
            foreach (UrutanMotor motor in semuaMotor)
            {
                urutanMotor[motor.GetUrutanMotor(semuaMotor) - 1] = motor;
            }
 
            urutanText.text = "";
            for (int i = 0; i < urutanMotor.Length; i++)
            {
                urutanText.text += (i + 1) + " - " + urutanMotor[i].name + "\n";
            }
        
    }

    public void CameraSwitch()
    {
        Switch++;
        if (Switch > cameraSwitchView.Count) { Switch = 0; }
    }
 public void KameraGanti()
    {
        
        bikeScript.cameraGanti[currentCamera].SetActive(false);
           currentCamera++;
        if (currentCamera==bikeScript.cameraGanti.Length)
        currentCamera=0;
        bikeScript.cameraGanti[currentCamera].SetActive(true);
    }

    public void BikeAccelForward(float amount)
    {
       bikeScript.accelFwd = amount;
    }

    public void BikeAccelBack(float amount)
    {
        bikeScript.accelBack = amount;
    }

    public void BikeSteer(float amount)
    {
        
        bikeScript.steerAmount = amount;
            
    }

    public void BikeHandBrake(bool HBrakeing)
    {
        bikeScript.brake = HBrakeing;
    }

    public void BikeShift(bool Shifting)
    {
         
        bikeScript.shift = Shifting;
        
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


     public void NaikGigi()
 {
      bikeScript.ShiftUp();
 }
 public void TurunGigi()
 {
      bikeScript.ShiftDown();
 }

    public void RestBike()
    {

        if (restTime == 0)
        {
           
            myRigidbody.AddForce(Vector3.up * 500000);
            myRigidbody.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
            restTime = 2.0f;
        }

    }




    public void ShowBikeUI()
    {



        gearst = bikeScript.currentGear;
        BikeUI.speedText.text = ((int)bikeScript.speed).ToString();




        if (bikeScript.bikeSetting.automaticGear)
        {

            if (gearst > 0 && bikeScript.speed > 1)
            {
                BikeUI.GearText.color = Color.green;
                BikeUI.GearText.text = gearst.ToString();
            }
            else if (bikeScript.speed > 1)
            {
                BikeUI.GearText.color = Color.red;
                BikeUI.GearText.text = "R";
            }
            else
            {
                BikeUI.GearText.color = Color.white;
                BikeUI.GearText.text = "N";
            }

        }
        else
        {

            if (bikeScript.NeutralGear)
            {
                BikeUI.GearText.color = Color.white;
                BikeUI.GearText.text = "N";
            }
            else
            {
                if (bikeScript.currentGear != 0)
                {
                    BikeUI.GearText.color = Color.green;
                    BikeUI.GearText.text = gearst.ToString();
                }
                else
                {

                    BikeUI.GearText.color = Color.red;
                    BikeUI.GearText.text = "R";
                }
            }

        }





        thisAngle = (bikeScript.motorRPM / 20) - 175;
        thisAngle = Mathf.Clamp(thisAngle, -180, 90);

        BikeUI.tachometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -thisAngle);
        BikeUI.barShiftGUI.rectTransform.localScale = new Vector3(bikeScript.powerShift / 100.0f, 1, 1);

    }


       
    

public void SpawnItem3D()
{
        //  int currentitemIndex = PlayerPrefs.GetInt("SelectedItem3D");
        // GameObject prefabItem3D = Item3DScene[currentitemIndex];
        //  PlayerPrefs.GetInt("SelectedItem3D", currentitemIndex);
        //    GameObject go = Instantiate(prefabItem3D, spawnItem3DScene );  //spawn player posisi di gameobject spawn
        //     bool pawanghujandibeli;
        //      pawanghujandibeli = bool.Parse(PlayerPrefs.GetString("pawangdibeli", "false"));
        //    Debug.Log(pawanghujandibeli);
        //  if(pawanghujandibeli==true)
        //  {
        //       go.SetActive(true);
        //       GameObject.Find("Hujan").SetActive(false);
        //      //Debug.Log("PAWANG TERBELI");
        //  }
        //  else
        //  {
        //     GameObject.Find("Hujan").SetActive(true);
        //      go.SetActive(false);
        //       // Debug.Log("PAWANG BLOM");
        //  }
}
public void SpawnPlayers()
{
    
    //     int currentMotorIndex = PlayerPrefs.GetInt("SelectedMotor");
    //     Transform prefabMotor = motorModels[currentMotorIndex];
      
    //     //motorDipilih[currentMotorIndex].SetActive(true);
       // target=  GameObject.FindGameObjectWithTag("Player").transform;
    //    target = Instantiate (prefabMotor,spawnPlayer); //spawn player posisi di gameobject spawn
    //      //GameObject spawnPlayer = Instantiate (prefabMotor,spawnPlayer.transform.position,spawnPlayer.transform.rotation);
         
    //     prefabMotor.gameObject.SetActive(true);
}
    void Start()
    {  
     
           
            
    //  MaxSdk.InitializeSdk();
     
      
  
     
   
          
        bikeScript = (Photon.Pun.Demo.PunBasics.BikeControlOnline)target.GetComponent<Photon.Pun.Demo.PunBasics.BikeControlOnline>();

        myRigidbody = target.GetComponent<Rigidbody>();

        cameraSwitchView = bikeScript.bikeSetting.cameraSwitchView;

        BikerMan = bikeScript.bikeSetting.bikerMan;
            
            
    }




    void Update()
    {
       
        Transform PlayerSpawn = GameObject.FindWithTag("Player").transform;//soawn nyari tag
        target= PlayerSpawn;
   
     if(target==null){
         Debug.Log("Load BLom ada");
     }

      
        
        bikeScript = (Photon.Pun.Demo.PunBasics.BikeControlOnline)target.GetComponent<Photon.Pun.Demo.PunBasics.BikeControlOnline>();

        myRigidbody = target.GetComponent<Rigidbody>();


        if (Input.GetKeyDown(KeyCode.G))
        {
            RestBike();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            PoliceLightSwitch();
        }


        if (restTime!=0.0f)
        restTime=Mathf.MoveTowards(restTime ,0.0f,Time.deltaTime);




        ShowBikeUI();

        GetComponent<Camera>().fieldOfView = Mathf.Clamp(bikeScript.speed / 10.0f + 60.0f, 60, 90.0f);



        if (Input.GetKeyDown(KeyCode.C))
        {
            bikeScript.cameraGanti[currentCamera].SetActive(false);
           currentCamera++;
        if (currentCamera==bikeScript.cameraGanti.Length)
        currentCamera=0;
        bikeScript.cameraGanti[currentCamera].SetActive(true);
        }


        if (!bikeScript.crash)
        {
            if (Switch == 0)
            {
                // Damp angle from current y-angle towards target y-angle

                float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
               target.eulerAngles.x + Angle, ref xVelocity, smooth);

                float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                target.eulerAngles.y, ref yVelocity, smooth);

                // Look at the target
                transform.eulerAngles = new Vector3(Angle, yAngle, 0.0f);

                var direction = transform.rotation * -Vector3.forward;
                var targetDistance = AdjustLineOfSight(target.position + new Vector3(0, height, 0), direction);


                transform.position = target.position + new Vector3(0, height, 0) + direction * targetDistance;


            }
            else
            {

                transform.position = cameraSwitchView[Switch - 1].position;
                transform.rotation = Quaternion.Lerp(transform.rotation, cameraSwitchView[Switch - 1].rotation, Time.deltaTime * 5.0f);

            }
        }
        else
        {
            Vector3 look = BikerMan.position - transform.position;
            transform.rotation = Quaternion.LookRotation(look);
        }

   
    


    float AdjustLineOfSight(Vector3 target, Vector3 direction)
    {


        RaycastHit hit;

        if (Physics.Raycast(target, direction, out hit, distance, lineOfSightMask.value))
            return hit.distance;
        else
            return distance;

    }
    }
}
}
