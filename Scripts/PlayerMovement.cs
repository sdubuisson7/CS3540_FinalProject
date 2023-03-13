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
        playerAnimator.SetFloat("moveHorizontal", moveHorizontal);
        playerAnimator.SetFloat("moveVertical", moveVertical);

        //Create normalized vector of input and multiply it by the move speed
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized; // normalized to prevent faster diagonal movement
        input *= moveSpeed;

        //Checks to see if the player is grounded
        if (controller.isGrounded)
        {
            int animInt = 0;
            if (!SwordAttack.attacked)
            {
                if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    animInt = 1;
                }
                

                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    animInt = 0;// Running Forward/Backward/Strafe animation Off
                }
                playerAnimator.SetInteger("animInt", animInt);
            }
            

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
        if (!SwordAttack.attacked)
        {
            controller.Move(moveDirection * Time.deltaTime); // Moves the controller over time
        }
        
        
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

