using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

/**
 * Renders the camera view onto a material
 **/
public class PortalCamera : MonoBehaviour
{
    // Declaration and initialization of variables
    public Transform playerPos;
    public Transform playerCamera;
    public Transform exitPortal;
    public Transform enterPortal;
    public Transform renderPortal;
    public Camera portalCamera;

    // Update is called once per frame
    void Update()
    {
        // Uses the position of the player to position the second camera
        Vector3 playerOffsetFromPortal = playerPos.position - enterPortal.position;
        transform.position = exitPortal.position + playerOffsetFromPortal;

        // Uses the rotation of the player to rotate the second camera
        transform.rotation = playerCamera.rotation;
        SetNearClipPlane();
    }

    /**
     * Sets the near renderplane of the camera
    **/
    void SetNearClipPlane()
    {
        // Calculates the distance between the player camera nd the render portal
        float distance = Vector3.Distance(playerPos.position, renderPortal.position);
        // If the player is very close to the portal set the near render plane to the lowest possible distance
        // Other wise the near renderplane is equal to the distance between the portal and the player
        if (distance > 0.5 && distance < 100)
            portalCamera.nearClipPlane = distance;
        else
            portalCamera.nearClipPlane = 0.01f;
    }
}
