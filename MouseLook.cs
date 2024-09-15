using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class controls the mouse input and its affects on the camera
 **/
public class MouseLook : MonoBehaviour
{
    // The sensitivity of the mouse
    public float mouseSensitivity = 100f;
    // The player body
    public Transform playerBody;
    // The rotation of the camera
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the input of the mouse from the x axis
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // Gets the input of the mouse from the y axis
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        // Updates the rotation of the camera
        xRotation -= mouseY;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Makes sure the player cannot look past straight up and straight below
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);


        // Updates the player body
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
