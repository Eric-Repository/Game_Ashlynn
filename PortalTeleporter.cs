using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using Cinemachine;

public class PortalTeleporter : MonoBehaviour
{
    public CharacterController player;
    public Camera thirdPersonCamera;
    public Transform receiver;
    public GameObject otherTPC;
    public Transform child0;
    public Transform child1;
    public Transform child2;
    public Transform child3;
    public CinemachineFreeLook TPC_Camera_CMFL;
    public Transform underWaterWorld;

    private bool playerInPortal = false;
    private bool canTeleport = true;

    public FishController fishController;

    private void checkUnderWater()
    {
        if (Vector3.Distance(player.transform.position, underWaterWorld.position) < 100)
        {
            fishController.isInWater = true;
        }
        else
            fishController.isInWater = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInPortal)
        {

            // The player can only enter the portal from one side
            Vector3 portalToPlayer = player.transform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // The camera can only enter hte portal from one side
            Vector3 portalToCamera = thirdPersonCamera.transform.position - transform.position;
            float dotProductCamera = Vector3.Dot(transform.up, portalToCamera);

            if (dotProduct < 0f && canTeleport)
            {
                canTeleport = false;
                float rotationalDifference = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationalDifference += (180);

                player.transform.Rotate(Vector3.up, rotationalDifference);
                //child0.Rotate(Vector3.up, rotationalDifference);
                //child1.Rotate(Vector3.up, rotationalDifference);
                //child2.Rotate(Vector3.up, rotationalDifference);
                //child3.Rotate(Vector3.up, rotationalDifference);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationalDifference, 0f) * portalToPlayer;
                player.enabled = false;
                player.transform.position = receiver.position + positionOffset;
                checkUnderWater();
                player.enabled = true;
                playerInPortal = false;

                TPC_Camera_CMFL.enabled = false;
                otherTPC.SetActive(false);

                Vector3 positionOffsetCamera = Quaternion.Euler(0f, rotationalDifference, 0f) * portalToCamera;

                otherTPC.transform.Rotate(Vector3.up, rotationalDifference);
                otherTPC.transform.position = receiver.position + positionOffsetCamera;

                thirdPersonCamera.transform.Rotate(Vector3.up, rotationalDifference);
                thirdPersonCamera.transform.position = receiver.position + positionOffsetCamera;

                Invoke("TurnOnCMFL", 0.05f);
                otherTPC.SetActive(true);
            }
        }
    }

    void TurnOnCMFL()
    {
        TPC_Camera_CMFL.enabled = true;
        canTeleport = true;
    }

    /**
     *  This method is called whenever an object enters the portal
     *  @param  Collider  other  The object that enters the portal
    **/
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInPortal = true;
        }
    }

     /**
     *  This method is called whenever an object exits the portal
     *  @param  Collider  other  The object that enters the portal
    **/
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInPortal = false;
        }
    }
}
