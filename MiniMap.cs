using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    //get the position of the character
    public Transform player;

    private void LateUpdate()
    {
        //make the camera move with character
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        //make the camera rotate with character
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
