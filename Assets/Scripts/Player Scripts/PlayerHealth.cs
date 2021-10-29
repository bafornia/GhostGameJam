using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The player dies when they have 0 health remaining.")]
    public int health = 5;
    [Tooltip("The y value at which the player will instantly die.")]
    public float deathDepth = -100;

    [Tooltip("Key the player presses to restart.")]
    public KeyCode restartKey = KeyCode.R;

    [Tooltip("How many seconds the player is invincible after getting hit.")]
    public float invincibilityFrames = 1;
    [Tooltip("Flashes per second of the invincibility flashing animation.")]
    public int flashSpeed = 10;
    [HideInInspector]
    public bool isInvincible = false;
    float flashingAnimation = 0;
    SpriteRenderer myRend;

    [Tooltip("Sound made when the player dies or restarts.")]
    public AudioClip deathSound;
    [Tooltip("Sound made when the player takes damage.")]
    public AudioClip hurtSound;
    AudioSource myAudio;

    [Tooltip("Prefab that will be used to make the hearts.")]
    public GameObject heartObject;
    [Tooltip("The width of the heart sprite, needed to calculate the position of the hearts")]
    public float heartWidth = 1;
    [Tooltip("Position of the first heart")]
    public Vector3 heartPosition = new Vector3(-15, 7.5f, 0);

    GameObject healthUI;
    SpriteRenderer heartRenderer;

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myRend = GetComponent<SpriteRenderer>();

        healthUI = Instantiate(heartObject, heartPosition, transform.rotation);

        heartRenderer = healthUI.GetComponent<SpriteRenderer>();

        heartRenderer.drawMode = SpriteDrawMode.Tiled;
    }

    void Update()
    {
        heartRenderer.size = new Vector3(health, 1, 1);
        healthUI.transform.position = new Vector3(heartPosition.x + heartWidth * (health - 1) / 2.0f, heartPosition.y, heartPosition.z)
                                    + new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

        if ((health <= 0
         || transform.position.y <= deathDepth
         || Input.GetKey(restartKey))
         && isDying == false)
        {
            StartCoroutine(death());
            isDying = true;
        }
    }

    private void FixedUpdate()
    {
        flashingAnimation += Time.fixedDeltaTime * flashSpeed * 2;

        myRend.enabled = (BoolToInt(isInvincible == true) * (Mathf.Round(flashingAnimation) % 2) == 0);
    }

    bool isDying = false;

    public void dealDamage()
    {
        if (isInvincible == false)
        {
            isInvincible = true;
            myAudio.PlayOneShot(hurtSound);
            health--;
            StartCoroutine(invincible());
        }
    }

    IEnumerator death()
    {
        myAudio.PlayOneShot(deathSound);

        yield return new WaitForSeconds(deathSound.length + 0.1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator invincible()
    {
        yield return new WaitForSeconds(invincibilityFrames);
        isInvincible = false;
    }

    int BoolToInt(bool i)
    {
        return System.Convert.ToInt32(i);
    }
}
