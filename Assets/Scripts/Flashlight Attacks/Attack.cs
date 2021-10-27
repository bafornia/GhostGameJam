using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [HideInInspector]
    public float attackDuration;
    [HideInInspector]
    public int attackDamage;

    public void customStart()
    {
        Destroy(gameObject, attackDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GhostHealth healthScript = collision.gameObject.GetComponent<GhostHealth>();

        if (healthScript != null)
        {
            healthScript.DealDamage(attackDamage);
        }
    }
}
