using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    [Tooltip("How much the ghost moves up and down for the floating animation.")]
    public float floatingAmplitude = 1.0f;
    [Tooltip("The speed the ghost moves up and down for the floating animation.")]
    public float floatingPeriod = 1.0f;

    float floatingAnimation = 0;
    float floatingPosition;

    Vector3 GhostPosition;

    void Start()
    {
        GhostPosition = transform.position;
    }

    private void FixedUpdate()
    {
        floatingAnimation += 50 * Time.fixedDeltaTime;

        floatingPosition = floatingAmplitude / 10 * Mathf.Sin(floatingPeriod / 100 * floatingAnimation);

        transform.position = new Vector3(GhostPosition.x, GhostPosition.y + floatingPosition, 0);
    }
}
