using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [HideInInspector]
    public float attackDuration;
    [HideInInspector]
    public int attackDamage;

    GameObject playerObject;
    Transform playerTransform;

    public void customStart()
    {
        playerObject = GameObject.Find("player").gameObject;
        playerTransform = playerObject.transform;

        StartCoroutine(death());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GhostHealth healthScript = collision.gameObject.GetComponent<GhostHealth>();

        if (healthScript != null)
        {
            healthScript.DealDamage(attackDamage);
        }
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    IEnumerator death()
    {
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

        playerMovement.lockPlayerMovement = true;

        yield return new WaitForSeconds(attackDuration);

        playerMovement.lockPlayerMovement = false;

        Destroy(gameObject);
    }
}
