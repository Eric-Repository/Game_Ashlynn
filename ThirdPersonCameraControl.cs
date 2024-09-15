using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    // The speed at which the character rotate
    float rotationSpeed = 1.5f;
    // References the Target and Player object
    public Transform Target, Player;
    // the value of Mouse X and Y asis
    float mouseX, mouseY;

    // References the Obstruction object
    public Transform Obstruction;
    float zoomSpeed = 2f;
    
    void Start()
    {
        Obstruction = Target;
        //remove the cursor
        Cursor.visible = false;
        //lock the cursor to center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraControl();
        ViewObstructed();
    }
    

    void CameraControl()
    {
        //user input to get mouseX and mouseY
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        //prevent camera from fliping aournd
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        //make camera focus on the right area
        transform.LookAt(Target);
        //press left shift key to only rotate the camera
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
        else
        {
            //rotate camera with character
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            Player.rotation = Quaternion.Euler(0, mouseX, 0);
        }
    }
    

    void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f))
        {
            //if not the player hitting the object
            if (hit.collider.gameObject.tag != "Player")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                
                if(Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
            }
            else
            {
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, Target.position) < 4.5f)
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
            }
        }
    }
}