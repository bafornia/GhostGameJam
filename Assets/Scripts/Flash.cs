using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [HideInInspector]
    public float flashDuration;
    [HideInInspector]
    public int flashDamage;

    public void customStart()
    {
        Destroy(gameObject, flashDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GhostHealth healthScript = collision.gameObject.GetComponent<GhostHealth>();

        if (healthScript != null)
        {
            healthScript.DealDamage(flashDamage);

        }
    }
}
