using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float lerpSpeed = 0.1f;

    [Tooltip("The game object for the player to follow. Defaults to whatever game object is called player.")]
    public GameObject player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("player");
        }
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed);
    }
}
