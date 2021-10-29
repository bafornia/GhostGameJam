using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public int health = 2;

    [Tooltip("Sound that plays when the ghost gets hurt.")]
    public AudioClip hurtSound;
    AudioSource myAudio;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
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
        myAudio.PlayOneShot(hurtSound);
        health -= damage;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
