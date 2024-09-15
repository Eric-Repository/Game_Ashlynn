using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    // The entrance portal in the main world
    public Transform MainWorldPortal1;
    // The exit portal in the tutorial world
    public Transform WorldPortal1;
    // tutorial world transform
    public Transform World1;

    // The entrance portal in the main world
    public Transform MainWorldPortal2;
    // The exit portal in the tutorial world
    public Transform WorldPortal2;
    // Smash world transform
    public Transform World2;

    // The entrance portal in the main world
    public Transform MainWorldPortal3;
    // The exit portal in the tutorial world
    public Transform WorldPortal3;
    // Smash world transform
    public Transform World3;

    // The entrance portal in the main world
    public Transform MainWorldPortal4;
    // The exit portal in the tutorial world
    public Transform WorldPortal4;
    // Smash world transform
    public Transform World4;

    // PortalObjectts
    public Transform enterPortal;
    public Transform exitPortal;

    // the actual portal object
    public GameObject enterPortalObject;
    public GameObject exitPortalobject;
    public GameObject tutorialWorld;
    public GameObject smashWorld;
    public GameObject swimWorld;
    public GameObject flyWorld;

    // The "aura"/light FX of the portal
    public GameObject aura;

    public GameObject Easle;

    public static readonly Vector3 flipCameraYAxis = new Vector3(0, 180, 0);

    public void SetInitialPortals()
    {
        World1.rotation = MainWorldPortal1.rotation * Quaternion.Euler(flipCameraYAxis);
        enterPortal.position = MainWorldPortal1.position;
        enterPortal.rotation = MainWorldPortal1.rotation;
        exitPortal.position = WorldPortal1.position;
        exitPortal.rotation = WorldPortal1.rotation;

        World2.rotation = MainWorldPortal2.rotation * Quaternion.Euler(flipCameraYAxis);
        World3.rotation = MainWorldPortal3.rotation * Quaternion.Euler(flipCameraYAxis);
        World4.rotation = MainWorldPortal4.rotation * Quaternion.Euler(flipCameraYAxis);

        enterPortalObject.SetActive(false);
        exitPortalobject.SetActive(false);
        tutorialWorld.SetActive(false);
        smashWorld.SetActive(false);
        swimWorld.SetActive(false);
        flyWorld.SetActive(false);
    }

    public void openTutorialWorld()
    {
        aura.SetActive(true);
        aura.transform.position = MainWorldPortal2.position;
        aura.transform.rotation = MainWorldPortal2.rotation;
        //aura.SetActive(false);
    }

    public void openDinoWorld()
    {
        aura.SetActive(true);

        aura.transform.position = MainWorldPortal3.position;
        aura.transform.rotation = MainWorldPortal3.rotation;
    }

    public void openDolphinWorld()
    {
        aura.SetActive(true);

        aura.transform.position = MainWorldPortal4.position;
        aura.transform.rotation = MainWorldPortal4.rotation;
    }

    public void openBirdWorld()
    {
        aura.SetActive(true);

        aura.transform.position = Easle.transform.position;
        aura.transform.rotation = Easle.transform.rotation;
    }

    public void disable()
    {
        aura.SetActive(false);
    }
}
