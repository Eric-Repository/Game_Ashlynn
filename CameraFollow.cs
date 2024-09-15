using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //camera speed
    public float CameraMoveSpeed = 120.0f;
    //camera follow object
    public GameObject CameraFollowObj;
    public Transform Target;

    //followed object position
    Vector3 FollowPos;
    //clamp angle
    public float clampAngle = 80.0f;
    //sensitivity
    public float inputSensitivity = 150.0f;
  
    //camera distance to player
    private float camDistanceXToPlayer;
    private float camDistanceYToPlayer;
    private float camDistanceZToPlayer;
    //mouseX and Y
    private float mouseX;
    private float mouseY;
    private float finalInputX;
    private float finalInputZ;
    private float smoothX;
    private float smoothY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        //lock the position of cursor and make it visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        

    }
  
    // Update is called once per frame
    void Update()
    {
        //setup the rotation of the mouse here
  
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX =  mouseX;
        finalInputZ = mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX -= finalInputZ * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        transform.LookAt(Target);

    }

    void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        //set the target object to follow
        Transform target = CameraFollowObj.transform;

        //move towards the game object that is the target
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
