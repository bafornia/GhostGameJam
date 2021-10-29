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
    public float xChaseRange = 10;
    [Tooltip("Vertical range from which the ghost can see the player.")]
    public float yChaseRange = 5;
    [Tooltip("How far the ghost will chase the player from its starting point before it gives up.")]
    public float giveUpRange = 10;

    [Tooltip("Sound that plays when the player gets close to the ghost.")]
    public AudioClip spookySound;
    [Tooltip("How far the player can be from the ghost and still hear the spooky sound.")]
    public float spookySoundDistance;
    AudioSource myAudio;
    bool spookySoundDone = true;

    float floatingAnimation = 0;
    float floatingPosition;
    
    Vector3 sentryPosition, ghostPosition;
     
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = 0;

        if (player == null)
        {
            player = GameObject.Find("player");
        }

        sentryPosition = ghostPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (spookySoundDone)
        {
            StartCoroutine(spookySoundEffect());
        }
        myAudio.volume = (spookySoundDistance - (player.transform.position - transform.position).magnitude) / spookySoundDistance;

        // floating animation

        floatingAnimation += 50 * Time.fixedDeltaTime;
        floatingPosition = floatingAmplitude / 10 * Mathf.Sin(floatingPeriod / 100 * floatingAnimation);

        // chasing AI

        Vector3 distanceToPlayer = player.transform.position - ghostPosition;
        float distanceToSentry = (ghostPosition - sentryPosition).sqrMagnitude;
        float playerToSentryDistance = (player.transform.position - sentryPosition).sqrMagnitude;

        if (Mathf.Abs(distanceToPlayer.x) <= xChaseRange
            && Mathf.Abs(distanceToPlayer.y) <= yChaseRange
            && distanceToSentry <= giveUpRange * giveUpRange
            && playerToSentryDistance <= giveUpRange * giveUpRange)
        {
            ghostPosition += chaseSpeed * (player.transform.position - ghostPosition).normalized * Time.fixedDeltaTime;
        }
        else if((ghostPosition - sentryPosition).sqrMagnitude >= 1f)
        {
            ghostPosition += returnSpeed * (sentryPosition - ghostPosition).normalized * Time.fixedDeltaTime;
        }

        transform.position = new Vector3(ghostPosition.x, ghostPosition.y + floatingPosition, ghostPosition.z);
    }
     
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth healthScript = collision.gameObject.GetComponent<PlayerHealth>();

        if (healthScript != null)
        {
            healthScript.dealDamage();
        }
    }

    IEnumerator spookySoundEffect()
    {
        spookySoundDone = false;
        myAudio.PlayOneShot(spookySound);
        yield return new WaitForSeconds(spookySound.length);
        spookySoundDone = true;
    }
}
