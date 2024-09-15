using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashColliderHandler : MonoBehaviour
{
    public GameObject otherObject;
    public DestroyInteraction destroyable;
    public DinoController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "destroyable")
        {
            print("destroyable in range");
            otherObject = other.gameObject.transform.parent.gameObject;
            destroyable = otherObject.GetComponent<DestroyInteraction>();
            if (controller.isInAnimation)
            {
                DestroyInteractable();
            }
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (other.tag == "destroyable")
        {
            destroyable = null;
        }
    }

    public void DestroyInteractable()
    {
        destroyable.destroy();
        otherObject = null;
    }
}
