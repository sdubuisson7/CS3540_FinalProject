using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10; // Reference to the players move speed
    public float jumpHeight = 10; // Reference to the jump height of the player
    public float gravity = 9.81f; //Reference to the gravity of the player
    public float airControl = 10; //Reference to how much control the player has in the air

    CharacterController controller; // Reference to the players character controller
    Vector3 input, moveDirection;
    Animator playerAnimator;
    bool jumped = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); // get character controller component from the player
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerAnimator").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Create normalized vector of input and multiply it by the move speed
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized; // normalized to prevent faster diagonal movement
        input *= moveSpeed;

        //Checks to see if the player is grounded
        if (controller.isGrounded)
        {
            int animInt = 0;
            if (!SwordAttack.attacked)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    animInt = 1; // Running Forward animation On
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    animInt = -1;//Running Backward animation On
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    animInt = -2;//A Strafe animation On
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    animInt = -3;//D Strafe animation On
                }

                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    animInt = 0;// Running Forward/Backward/Strafe animation Off
                }
                playerAnimator.SetInteger("animInt", animInt);
            }
            
            /* if (Input.GetKey(KeyCode.W))
             {
                 playerAnimator.SetInteger("animInt", 1); // Running Forward animation On
             }
             if (Input.GetKey(KeyCode.S))
             {
                 playerAnimator.SetInteger("animInt", -1);//Running Backward animation On
             }
             if (Input.GetKey(KeyCode.A))
             {
                 playerAnimator.SetInteger("animInt", -2);//A Strafe animation On
             }
             if (Input.GetKey(KeyCode.D))
             {
                 playerAnimator.SetInteger("animInt", -3);//D Strafe animation On
             }
             if (Input.GetKeyUp(KeyCode.W))
             {
                 playerAnimator.SetInteger("animInt", 0);// Running Forward animation Off

             }
             if (Input.GetKeyUp(KeyCode.S))
             {
                 playerAnimator.SetInteger("animInt", 0);//Running Backward animation Off

             }
             if (Input.GetKeyUp(KeyCode.A))
             {
                 playerAnimator.SetInteger("animInt", 0);// A Strafe animation Off

             }
             if (Input.GetKeyUp(KeyCode.D))
             {
                 playerAnimator.SetInteger("animInt", 0);//D Strafe animation Off

             }*/

            moveDirection = input;
            //Checks to see if jump button was pressed
            if (Input.GetButton("Jump") && !jumped)
            {
                //sets Jump height to y component of move direction vector
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                StartCoroutine(JumpAnimation());
                
            }
            else
            {
                //Makes move direction's y equal 0 if not jumping
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            //Allows player to move while in the air
            input.y = moveDirection.y; 
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }
        moveDirection.y -= gravity * Time.deltaTime; // applies gravtiy to vector
        controller.Move(moveDirection * Time.deltaTime); // Moves the controller over time
    }

    IEnumerator JumpAnimation()
    {
        playerAnimator.SetInteger("animInt", 3);
        jumped = true;
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.1f);
        playerAnimator.SetInteger("animInt", 0);
        jumped = false;
    }
}

