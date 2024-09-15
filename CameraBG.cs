using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBG : MonoBehaviour
{
    public GameObject target;

    public void UpdateCameraPosition()
    {
        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;
    }
}
