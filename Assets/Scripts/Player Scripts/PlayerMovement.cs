using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myRb;
    BoxCollider2D myBoxCol;

    // left/right movement

    [Tooltip("Maximum speed of the player.")]
    public float maxSpeed = 5;
    [Tooltip("How quickly the player comes to a stop. 1 is instant, lower means a slower stop.")][Range(0.1f, 1.0f)]
    public float playerFriction = 0.9f;
    float playerVelocity = 0;

    [HideInInspector]
    public bool lockPlayerMovement = false;
    [HideInInspector]
    public bool aimLock = false;

    // jumping

    [Tooltip("Same as the rigidbody gravity scale, just manipulatable here for convenience." + "Leave at zero to not change it from what is set in the rigidbody component.")]
    public float gravityScale= 0;

    [Tooltip("The highest jump the player can make.")]
    public float maxJumpHeight = 10;
    [Tooltip("How quickly the player jumps")]
    public float jumpSpeed = 15;
    [Tooltip("How precise the player's jump is.")]
    public float jumpPrecision = 3;
    [Tooltip("Amount of time the player has before they land to press space and still jump instantly.")]
    public float jumpBuffer = 0.3f;

    float jumpBufferCounter, jumpHeightCounter = 0;

    [Tooltip("A grace period in seconds where the player can still jump after leaving a platform.")]
    public float coyoteTimeLength = 0.5f;
    float coyoteTimeCounter = 0;

    [Tooltip("Layer of objects the player can jump on.")]
    public LayerMask jumpOn;

    [Tooltip("Sound that plays when the player jumps.")]
    public AudioClip jumpStartSound;
    [Tooltip("Volume for jumpStartSound")]
    public float jumpStartSoundVolume = 1;
    [Tooltip("Sound that plays when the player lands.")]
    public AudioClip jumpEndSound;
    [Tooltip("Volume for jumpEndSound")]
    public float jumpEndSoundVolume = 1;
    AudioSource myAudio;

    bool landingSoundPlayed = true;
    public AudioSource[] sounds;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody2D>();
        myBoxCol = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        float playerSpeed = -((maxSpeed * 50) - (maxSpeed * 50) / playerFriction);
        //since the velocity of the player increases at an exponentially regressive rate, the horizontal
        //asymptote of that graph will be the actual maximum player speed. this math converts the desired
        //player speed into the necessary value to put the asymptote of the velocity graph at the desired value.
        //the * 100 is just to make the number in the editor easier to work with
        //i figured it out here https://www.desmos.com/calculator/qsv0p4v5zg


        // left/right movement


        playerVelocity = (playerVelocity + BoolToInt(!lockPlayerMovement && !aimLock)
                        * Input.GetAxisRaw("Horizontal") * playerSpeed * Time.fixedDeltaTime) * playerFriction;

        if (Input.GetAxisRaw("Horizontal") != 0
            && lockPlayerMovement == false
            && aimLock == false)
        {
            myRb.velocity = new Vector2(maxSpeed * 50 * Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime, myRb.velocity.y);
            playerVelocity = maxSpeed * Input.GetAxisRaw("Horizontal");
        }
        else
        {
            myRb.velocity = new Vector2(playerVelocity, myRb.velocity.y);
        }
    }

    void Update()
    {
        //checks if the gravityScale variable has been set, and if so updates the actual gravity scale to it
        //only in the update function so it can be changed while running the game
        myRb.gravityScale -= BoolToInt(gravityScale != 0) * (myRb.gravityScale - gravityScale);


        // jumping
    

        //only increases coyote time counter if not grounded, if grounded sets to zero
        coyoteTimeCounter = BoolToInt(!IsGrounded()) * (coyoteTimeCounter + BoolToInt(!IsGrounded()) * Time.deltaTime);

        jumpBufferCounter -= Time.deltaTime;
        //resets counter if grounded or space key is pressed
        jumpBufferCounter -= (jumpBufferCounter - jumpBuffer) * BoolToInt(Input.GetKeyDown("space") && !IsGrounded());

        if (lockPlayerMovement == false)
        {
            if ((Input.GetKeyDown("space") && (IsGrounded() || coyoteTimeCounter <= coyoteTimeLength))
            || (jumpBufferCounter >= 0 && Input.GetKey("space") && IsGrounded() && myRb.velocity.y <= 0))
            {
                myAudio.PlayOneShot(jumpStartSound, jumpStartSoundVolume);
                jumpHeightCounter = 1;
                coyoteTimeCounter = coyoteTimeLength;
            }

            if (jumpHeightCounter != 0 && (jumpHeightCounter >= maxJumpHeight || !Input.GetKey("space") || BumpedCeiling()))
            {
                myRb.velocity = new Vector2(myRb.velocity.x, jumpSpeed / jumpPrecision);
                jumpHeightCounter = 0;
            }
            else if (Input.GetKey("space") && jumpHeightCounter != 0)
            {
                myRb.velocity = new Vector2(myRb.velocity.x, jumpSpeed);

                jumpHeightCounter += 50 * Time.deltaTime;
            }

            if (IsGrounded() && !landingSoundPlayed)
            {
                myAudio.PlayOneShot(jumpEndSound, jumpEndSoundVolume);
                landingSoundPlayed = true;
            }
            else if (!IsGrounded())
            {
                landingSoundPlayed = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(myBoxCol.bounds.center, myBoxCol.bounds.size - new Vector3(0.1f, myBoxCol.bounds.size.x * 0.5f, 0), 0f, Vector2.down, myBoxCol.bounds.size.x * 0.25f + 0.1f, jumpOn).collider != null;
        //i learned this method from this video https://www.youtube.com/watch?v=ptvK4Fp5vRY
    }

    private bool BumpedCeiling()
    {
        return Physics2D.BoxCast(myBoxCol.bounds.center, myBoxCol.bounds.size - new Vector3(0.1f, myBoxCol.bounds.size.x * 0.5f, 0), 0f, Vector2.up, myBoxCol.bounds.size.x * 0.25f + 0.1f, jumpOn).collider != null;
    }

    int BoolToInt(bool i)
    {
        return System.Convert.ToInt32(i);
    }
}