using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [HideInInspector]
    public float attackDuration;
    [HideInInspector]
    public int attackDamage;

    public GameObject playerObject;

    public void customStart()
    {
<<<<<<< Updated upstream
        playerObject = GameObject.Find("player").gameObject;
        playerTransform = playerObject.transform;

        StartCoroutine(Death());
=======
        StartCoroutine(death());
>>>>>>> Stashed changes
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
        transform.position = playerObject.transform.position;
    }

    IEnumerator Death()
    {
        PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();

        playerMovement.lockPlayerMovement = true;

        yield return new WaitForSeconds(attackDuration);

        playerMovement.lockPlayerMovement = false;

        Destroy(gameObject);
    }
}
