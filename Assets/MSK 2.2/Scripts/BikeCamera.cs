using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using EasyMobile;

public class BikeCamera : MonoBehaviour
{
     public static BikeCamera Instance;
    //public WayPoints wp;
    public Transform spawnPlayer;
     [Header("Spawn AI")]
     public int spawnCount; // How many objects should be spawned
    private int objectIndex; // Random objectsToSpawn index
    private int spawnIndex; //
      [Header("Text Hitung")]
    [SerializeField]  public Text TextUICountDown;
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



    private BikeControl bikeScript;
  private BikeControlAI bikeScriptAI;


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
        
         Invoke("MainMenu", 0.2f);
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
 public void NaikGigi()
 {
      bikeScript.ShiftUp();
 }
 public void TurunGigi()
 {
      bikeScript.ShiftDown();
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

 void Awake()
    {
     

        
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

 public void SpawnMusuh()
    {

// for (int i = 0; i < spawnCount; i++)
//      {
//          // For each iteration generate a random index; You could make an int array containing if an object already got spawned and change the index.
//          objectIndex = Random.Range(0, objectsToSpawn.Length);
//          spawnIndex = Random.Range(0, spawnPoints.Length);
//          // Instantiate object
//          GameObject go = Instantiate(objectsToSpawn[objectIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
//          // Add Object to spawnedObjects List
//          spawnedObjects.Add(go);
//      }
           
             semuaMotor = new UrutanMotor[motorObjects.Count];
        urutanMotor = new UrutanMotor[motorObjects.Count];
       
            for (int i = 0; i < 1; i++)
            {
                   int roll = Random.Range(0, motorObjects.Count);
                // GameObject motorPrefabMusuh = motorObjects[roll];
               
                GameObject go = Instantiate(motorObjects[roll], spawnBOT );
               // Debug.Log(motorPrefabMusuh);
                  Debug.Log(roll);
              go.SetActive(true);
                      //motorObjects[i].SetActive(true);
                 //motorObjects[i].gameObject.SetActive(true);
             // semuaMotor[i].gameObject.SetActive(true);
                 semuaMotor[i] = motorObjects[i].GetComponent<UrutanMotor>();
        }
        
            // GameObject go = Instantiate(motorPrefabMusuh, transform.position, transform.rotation);
            // //go.name = name;
          
       
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
    
        int currentMotorIndex = PlayerPrefs.GetInt("SelectedMotor");
        Transform prefabMotor = motorModels[currentMotorIndex];
       
        //motorDipilih[currentMotorIndex].SetActive(true);
        target = Instantiate (prefabMotor,spawnPlayer); //spawn player posisi di gameobject spawn
         //GameObject spawnPlayer = Instantiate (prefabMotor,spawnPlayer.transform.position,spawnPlayer.transform.rotation);
         
        prefabMotor.gameObject.SetActive(true);
}
    void Start()
    {  
        // MaxSdk.InitializeSdk();
     
        StartCoroutine(HitungNomerCoroutine());
     

    IEnumerator HitungNomerCoroutine() {
        Button tombolGas = GameObject.Find("Accel").GetComponent<Button>();
        Button tombolNaikGigi = GameObject.Find("NaikGigi").GetComponent<Button>();
        Button tombolTurunGigi= GameObject.Find("TurunGigi").GetComponent<Button>();
        tombolGas.gameObject.SetActive(false);
        tombolNaikGigi.gameObject.SetActive(false);
        tombolTurunGigi.gameObject.SetActive(false);
        StartMotorMajuBOT=false;
        TextUICountDown.text = "3";

        yield return new WaitForSeconds(1.0f);
        TextUICountDown.text = "2";
        StartMotorMajuBOT=false;
        tombolGas.gameObject.SetActive(false);
        tombolNaikGigi.gameObject.SetActive(false);
        tombolTurunGigi.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        TextUICountDown.text = "1";
        StartMotorMajuBOT=false;
        tombolGas.gameObject.SetActive(false);
        tombolNaikGigi.gameObject.SetActive(false);
        tombolTurunGigi.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        TextUICountDown.text = "Go!";
        
        // start the game here
        yield return new WaitForSeconds(1.0f);
        StartMotorMajuBOT=true;
        tombolGas.gameObject.SetActive(true);
        tombolNaikGigi.gameObject.SetActive(true);
        tombolTurunGigi.gameObject.SetActive(true);
        TextUICountDown.text = "";
       
    }

    
    
  


       SpawnPlayers();
       SpawnMusuh();
       SpawnItem3D();
       //updateUrutan();
        //   if (!RuntimeManager.IsInitialized())
        //     RuntimeManager.Init();
        //  Advertisements.ShowInterstitial();
      
      
  
     
   
          
        bikeScript = (BikeControl)target.GetComponent<BikeControl>();//panggil script

        myRigidbody = target.GetComponent<Rigidbody>();

        cameraSwitchView = bikeScript.bikeSetting.cameraSwitchView;

        BikerMan = bikeScript.bikeSetting.bikerMan;

    }


    
        

    void Update()
    {
        

        if (!target) return;

     
        
        bikeScript = (BikeControl)target.GetComponent<BikeControl>();

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
