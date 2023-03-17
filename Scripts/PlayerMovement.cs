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
            //if (!SwordAttack.attacked)
            //{
                if(moveHorizontal != 0  || moveVertical != 0)
                {
                    playerAnimator.SetBool("IsMoving", true);
                }
                else if(moveHorizontal == 0 || moveVertical == 0)
                {
                    playerAnimator.SetBool("IsMoving", false);
                }
            //}
            

            moveDirection = input;
            //Checks to see if jump button was pressed
            if (Input.GetButton("Jump") && !jumped)
            {
                //sets Jump height to y component of move direction vector
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                playerAnimator.SetInteger("animInt", 3);
                jumped = true;
                Invoke("JumpAnimation", playerAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.25f);
                //StartCoroutine(JumpAnimation());
                
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

        if (SwordAttack.attacked)
        {
            //moveDirection.x = 0;
            //moveDirection.z = 0;
        }
        
       moveDirection.y -= gravity * Time.deltaTime; // applies gravtiy to vector
       controller.Move(moveDirection * Time.deltaTime); // Moves the controller over time
        
    }

    void JumpAnimation()
    {
        //playerAnimator.SetInteger("animInt", 3);
        //jumped = true;
        //yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.25f);
        playerAnimator.SetInteger("animInt", 0);
        jumped = false;
    }
}

