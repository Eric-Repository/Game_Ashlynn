using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalKey : MonoBehaviour
{
    // The location of the portals
    public Transform mainWorldPortalPos;
    public Transform sideWorldPortalPos;

    // The transform of the portal collider
    public Transform enterPortal;
    public Transform exitPortal;

    // The actual enter portal
    public GameObject enterPortalObject;
    public GameObject sideWorld;
    public GameObject exitPortalObject;

    // Otherworld
    public GameObject otherWorld1;
    public GameObject otherWorld2;
    public GameObject otherWorld3;

    //public GameObject otherWorld3;

    // State machine
    public bool isThePortalOn = false;

    // Animator
    public Animator anim;

    public Item key01;
    public Item key02;
    public Item key03;

    // portal manager
    public PortalManager portalManager;

    

    public void interaction()
    {
        if (this.tag == "PortalKey" && this.name == "Key")
        {
            enterPortal.position = mainWorldPortalPos.position;
            enterPortal.rotation = mainWorldPortalPos.rotation;
            exitPortal.position = sideWorldPortalPos.position;
            exitPortal.rotation = sideWorldPortalPos.rotation;
            if (isThePortalOn)
            {
                enterPortalObject.SetActive(false);
                sideWorld.SetActive(false);
                exitPortalObject.SetActive(false);
                isThePortalOn = false;
                anim.SetInteger("onOrOff", 0);
            }
            else
            {
                enterPortalObject.SetActive(true);
                sideWorld.SetActive(true);
                exitPortalObject.SetActive(true);
                isThePortalOn = true;
                anim.SetInteger("onOrOff", 1);
                // Disables other world
                // This is done for efficiency so that only the main world and the current side world are rendered simultaneously
                otherWorld1.SetActive(false);
                otherWorld2.SetActive(false);
                otherWorld3.SetActive(false);
            }
            portalManager.disable();
        }
        //when player interact with dino key and also has key01 in his inventory then he will be able to open the portal
        else if (this.tag == "PortalKey" && this.name == "DinoKey" && Inventory.instance.HasItem(key01))
        {
            enterPortal.position = mainWorldPortalPos.position;
            enterPortal.rotation = mainWorldPortalPos.rotation;
            exitPortal.position = sideWorldPortalPos.position;
            exitPortal.rotation = sideWorldPortalPos.rotation;
            if (isThePortalOn)
            {
                enterPortalObject.SetActive(false);
                sideWorld.SetActive(false);
                exitPortalObject.SetActive(false);
                isThePortalOn = false;
                anim.SetInteger("onOrOff", 0);
            }
            else
            {
                enterPortalObject.SetActive(true);
                sideWorld.SetActive(true);
                exitPortalObject.SetActive(true);
                isThePortalOn = true;
                anim.SetInteger("onOrOff", 1);
                // Disables other world
                // This is done for efficiency so that only the main world and the current side world are rendered simultaneously
                otherWorld1.SetActive(false);
                otherWorld2.SetActive(false);
                otherWorld3.SetActive(false);
            }
            portalManager.disable();

        }
        else if (this.tag == "PortalKey" && this.name == "DolphinKey" && Inventory.instance.HasItem(key02))
        {
            enterPortal.position = mainWorldPortalPos.position;
            enterPortal.rotation = mainWorldPortalPos.rotation;
            exitPortal.position = sideWorldPortalPos.position;
            exitPortal.rotation = sideWorldPortalPos.rotation;
            if (isThePortalOn)
            {
                enterPortalObject.SetActive(false);
                sideWorld.SetActive(false);
                exitPortalObject.SetActive(false);
                isThePortalOn = false;
                anim.SetInteger("onOrOff", 0);
            }
            else
            {
                enterPortalObject.SetActive(true);
                sideWorld.SetActive(true);
                exitPortalObject.SetActive(true);
                isThePortalOn = true;
                anim.SetInteger("onOrOff", 1);
                // Disables other world
                // This is done for efficiency so that only the main world and the current side world are rendered simultaneously
                otherWorld1.SetActive(false);
                otherWorld2.SetActive(false);
                otherWorld3.SetActive(false);
            }
            portalManager.disable();

        }
        else if (this.tag == "PortalKey" && this.name == "BirdKey" && Inventory.instance.HasItem(key03))
        {
            enterPortal.position = mainWorldPortalPos.position;
            enterPortal.rotation = mainWorldPortalPos.rotation;
            exitPortal.position = sideWorldPortalPos.position;
            exitPortal.rotation = sideWorldPortalPos.rotation;
            if (isThePortalOn)
            {
                enterPortalObject.SetActive(false);
                sideWorld.SetActive(false);
                exitPortalObject.SetActive(false);
                isThePortalOn = false;
                anim.SetInteger("onOrOff", 0);
            }
            else
            {
                enterPortalObject.SetActive(true);
                sideWorld.SetActive(true);
                exitPortalObject.SetActive(true);
                isThePortalOn = true;
                anim.SetInteger("onOrOff", 1);
                // Disables other world
                // This is done for efficiency so that only the main world and the current side world are rendered simultaneously
                otherWorld1.SetActive(false);
                otherWorld2.SetActive(false);
                otherWorld3.SetActive(false);
            }

            portalManager.disable();

        }
        else if (this.tag == "PortalKey")
        {
            enterPortal.position = mainWorldPortalPos.position;
            enterPortal.rotation = mainWorldPortalPos.rotation;
            exitPortal.position = sideWorldPortalPos.position;
            exitPortal.rotation = sideWorldPortalPos.rotation;
            if (isThePortalOn)
            {
                enterPortalObject.SetActive(false);
                sideWorld.SetActive(false);
                exitPortalObject.SetActive(false);
                isThePortalOn = false;
                anim.SetInteger("onOrOff", 0);
            }
            else
            {
                enterPortalObject.SetActive(true);
                sideWorld.SetActive(true);
                exitPortalObject.SetActive(true);
                isThePortalOn = true;
                anim.SetInteger("onOrOff", 1);
                // Disables other world
                // This is done for efficiency so that only the main world and the current side world are rendered simultaneously
                otherWorld1.SetActive(false);
                otherWorld2.SetActive(false);
                otherWorld3.SetActive(false);
            }
        }
       
    }
}
