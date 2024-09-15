using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class controls the keyboard inputs and its affects on the player
 **/
public class KeyboardMove : MonoBehaviour
{
    // The character controller object
    public CharacterController controller;
    // References the ground check object
    public Transform groundCheck;
    // The radius of the sphere that will be projected during the ground check
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    // The speed at which the character walks
    public float speed = 12f;
    // The speed at which the character is falling
    Vector3 velocity;
    // The gravitational force that is influencing the player
    public float gravity = -9.81f;
    // Whether or not the player is on the ground
    bool isGrounded;
    // The height that the player can jump
    public float jumpHeight = 0.75f;
    // Update is called once per frame

    void Start()
    {
        updateGravity();
    }

    void Update()
    {
        // Checks if the player is on the ground
        // Uses physics engine to project sphere under character to detect if they are standing on surface
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            // Set to -2f instead of 0f for buffer
            velocity.y = -2f;
        }
        // Gets the keyboard input from WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // The player will jump if they press the space button
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // This is the equation used to determine the amount of velocity used to jump a certain height
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Puts the input into a local transformation of the player
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        // Puts the affects of gravity on the controller
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void updateGravity()
    {
        gravity *= 3f;
    }
}
