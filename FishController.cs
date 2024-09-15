using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public Transform cam;
    public CharacterController controller;
    // The speed at which the character walks
    public float speed = 6f;
    //head turn spped
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // The speed at which the character is falling
    Vector3 velocity;

    public PlayerController playerController;
    public Animator anim;
    public bool isInWater = false;
    public Renderer render;
    public Material mat;
    public Material mat2;
    public GameObject sonarVFX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInWater)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            // The player will jump if they press the space button
            if (Input.GetButtonDown("Jump"))
            {
                //Sonar Code goes here
                render.sharedMaterial = mat;
                sonarVFX.SetActive(true);
                Invoke("sonarEnd", 10f);
            }

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                transform.rotation = Quaternion.Euler(cam.transform.localEulerAngles.x, angle, 0f);

                //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                Vector3 moveDir = Quaternion.Euler(cam.transform.localEulerAngles.x, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                //Animator Script
                anim.SetInteger("Movement", 1);
            }
            else
            {
                //Animator Script
                anim.SetInteger("Movement", 0);
            }

            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F) && playerController.withinTriggerRange == true)
            {
                playerController.otherScript.interaction();
            }

            if (playerController.colliderWithQuest1)
            {
                if (Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && !playerController.questScript.questWindow.activeSelf)
                {
                    print("quest1");
                    playerController.questScript.interaction();
                }
                else if (Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && playerController.questScript.questWindow.activeSelf)
                {
                    playerController.questScript.CloseQuestWindow();
                }
            }
            else if (playerController.colliderWithQuest2)
            {
                if (Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && !playerController.questScript2.questWindow.activeSelf)
                {
                    print("quest2");
                    playerController.questScript2.interaction();
                }
                else if (Input.GetKeyDown(KeyCode.F) && playerController.withinQuestRange == true && playerController.questScript2.questWindow.activeSelf)
                {
                    playerController.questScript2.CloseQuestWindow();
                }
            }
        }
    }

    void sonarEnd()
    {
        render.sharedMaterial = mat2;
        sonarVFX.SetActive(false);
    }
}
