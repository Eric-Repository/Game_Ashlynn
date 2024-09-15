using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour
{
   public Transform cam;
    public CharacterController controller;
    // The speed at which the character walks
    public float speed = 6f;
    //head turn spped
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // References the ground check object
    public Transform groundCheck;
    // The radius of the sphere that will be projected during the ground check
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
   
    // The speed at which the character is falling
    Vector3 velocity;
    // The gravitational force that is influencing the player
    public float gravity = -0.25f;
    // Whether or not the player is on the ground
    bool isGrounded;

    public PlayerController playerController;
    public Animator anim;
    public bool isAttacking = false;
    public bool isInAnimation = false;
    public GameObject smashCollider;

    public SmashColliderHandler smashHandler;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        updateGravity();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if the player is on the ground
        // Uses physics engine to project sphere under character to detect if they are standing on surface
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            // Set to -2f instead of 0f for buffer
            velocity.y = -0.5f;
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // The player will jump if they press the space button
        if (Input.GetButtonDown("Jump") && isGrounded && !isInAnimation && smashHandler.destroyable != null)
        {
            // Dino will not jump
            // Jummp action is replaced with headbutt
            isAttacking = true;
            if (smashHandler.otherObject != null)
            {
                smashHandler.DestroyInteractable();
            }
        }


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            if (isGrounded && !isAttacking)
                anim.SetInteger("movement", 1);
            else if (isAttacking && !isInAnimation)
            {
                isAttacking = false;
                isInAnimation = true;
                anim.SetInteger("movement", 3);
                Invoke("animationEnd", 2.5f);
            }
        }
        else
        {
            if (isGrounded && !isAttacking)
                anim.SetInteger("movement", 0);
            else if (isAttacking && !isInAnimation)
            {
                isAttacking = false;
                isInAnimation = true;
                anim.SetInteger("movement", 2);
                Invoke("animationEnd", 2.5f);
            }
        }

        // Puts the affects of gravity on the controller
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F) && playerController.withinTriggerRange == true)
        {
            playerController.otherScript.interaction();
        }
        if (Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && !playerController.questScript.questWindow.activeSelf)
        {
            playerController.questScript.interaction();
        }
        else if(Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && playerController.questScript.questWindow.activeSelf)
        {
            playerController.questScript.CloseQuestWindow();
        }
    }

    public void animationEnd()
    {
        if (isInAnimation)
        {
            isInAnimation = false;
        }
    }

    void updateGravity()
    {
        gravity *= 2f;
    }
}
