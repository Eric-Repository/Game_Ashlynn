﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCharacterControl : MonoBehaviour
{
    public float Speed;

	void Update ()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 playerMovement = new Vector3(hor, 0f, ver).normalized * Speed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }
}