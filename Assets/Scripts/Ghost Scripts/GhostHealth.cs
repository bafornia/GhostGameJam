using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public int health = 2;

    void Start()
    {
        
    }

    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
