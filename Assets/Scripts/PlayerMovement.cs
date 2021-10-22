using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D myRb;

    public float playerSpeed = 5;

    public float jumpHeight = 10;

    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        int xInput = BoolToInt(Input.GetKey("right")) - BoolToInt(Input.GetKey("left"));

        myRb.AddForce(new Vector2(xInput * playerSpeed * 100 * Time.fixedDeltaTime, 0));
    }

    int BoolToInt(bool i)
    {
        return System.Convert.ToInt32(i);
    }
}