using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Tooltip("How much the ghost moves up and down for the floating animation.")]
    public float floatingAmplitude = 1.0f;
    [Tooltip("The speed the ghost moves up and down for the floating animation.")]
    public float floatingPeriod = 1.0f;

    [Tooltip("The player the ghost will chase. Defaults to whatever game object is called player")]
    public GameObject player;

    [Tooltip("Speed of the ghost while chasing the player.")]
    public float chaseSpeed = 3;
    [Tooltip("Speed of the ghost while returning to its sentry position.")]
    public float returnSpeed = 2;
    [Tooltip("Horizontal distance from which the ghost can see the player.")]
    public float xChaseRange = 5;
    [Tooltip("Vertical range from which the ghost can see the player.")]
    public float yChaseRange = 3;
    [Tooltip("How far the ghost will chase the player from its starting point before it gives up.")]
    public float giveUpRange = 10;

    float floatingAnimation = 0;
    float floatingPosition;
    
    Vector3 sentryPosition, ghostPosition;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("player");
        }

        sentryPosition = ghostPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // floating animation
        floatingAnimation += 50 * Time.fixedDeltaTime;
        floatingPosition = floatingAmplitude / 10 * Mathf.Sin(floatingPeriod / 100 * floatingAnimation);

        // chasing AI
        if ((player.transform.position - transform.position).sqrMagnitude <= new Vector3(xChaseRange, yChaseRange, 0).sqrMagnitude
            && (transform.position - sentryPosition).sqrMagnitude <= giveUpRange * giveUpRange)
        {
            ghostPosition += chaseSpeed * (player.transform.position - ghostPosition).normalized * Time.fixedDeltaTime;
        }
        else if((ghostPosition - sentryPosition).sqrMagnitude >= 1f)
        {
            ghostPosition += returnSpeed * (sentryPosition - ghostPosition).normalized * Time.fixedDeltaTime;
        }

        transform.position = new Vector3(ghostPosition.x, ghostPosition.y + floatingPosition, ghostPosition.z);
    }
}
