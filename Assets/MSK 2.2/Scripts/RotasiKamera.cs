using Cinemachine;
 using UnityEngine;
 
 
 public class RotasiKamera : MonoBehaviour {
   public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Update()
    {
            
        if(Input.touchCount > 0) {
        Touch touch = Input.GetTouch(0);
        float turnAngleChange = (touch.deltaPosition.x / Screen.width) * sensitivityX; 
        float pitchAngleChange = (touch.deltaPosition.y / Screen.height) * sensitivityY;

        // Handle any pitch rotation
        if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseY) {
            rotationY = Mathf.Clamp(rotationY+pitchAngleChange, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0f);
        }

        // Handle any turn rotation
        if (axes == RotationAxes.MouseXAndY || axes == RotationAxes.MouseX) {
            transform.Rotate(0f, turnAngleChange , 0f);
        }
    }
    }
    
    void Start()
    {
        //if(!networkView.isMine)
        //enabled = false;

        // Make the rigid body not change rotation
        //if (rigidbody)
        //rigidbody.freezeRotation = true;
    }
}