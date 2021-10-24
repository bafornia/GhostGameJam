using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myRb;
    BoxCollider2D myBoxCol;

    [Tooltip("Maximum speed of the player.")]
    public float playerSpeed = 5; //player speed

    [Tooltip("How quickly the player comes to a stop. 1 is instant, lower ")][Range(0.1f, 1.0f)]
    public float playerFriction = 0.9f; //player friction

    public float gravityScale= 0;

    public float jumpHeight = 10;

    public LayerMask jumpOn;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myBoxCol = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        float maxSpeed = -(playerSpeed - playerSpeed / playerFriction) * 100;
        //since the velocity of the player increases at an exponentially regressive rate, the horizontal
        //asymptote of that graph will be the actual maximum player speed. this math converts the desired
        //player speed into the necessary value to put the asymptote of the velocity graph at the desired value.
        //the * 100 is just to make the number in the editor easier to work with
        //i figured it out here https://www.desmos.com/calculator/qsv0p4v5zg

        //checks if the gravityScale variable has been set, and if so updates the actual gravity scale to it
        myRb.gravityScale -= BoolToInt(gravityScale != 0) * (myRb.gravityScale - gravityScale);

        myRb.velocity = new Vector2((myRb.velocity.x +
                        Mathf.Round(Input.GetAxisRaw("Horizontal")) * maxSpeed * Time.fixedDeltaTime) * playerFriction,
                        myRb.velocity.y);

        if (Input.GetKey("space") && IsGrounded())
        {
            myRb.velocity = (new Vector2(myRb.velocity.x, jumpHeight));
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(myBoxCol.bounds.center, myBoxCol.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, 0.1f, jumpOn).collider != null;
        //i learned this method from this video https://www.youtube.com/watch?v=ptvK4Fp5vRY
        //i adjusted the boxcast size in order to prevent the player from jumping off the sides of walls
    }

    int BoolToInt(bool i)
    {
        return System.Convert.ToInt32(i);
    }
}