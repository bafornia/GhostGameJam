using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("The player dies when they have 0 health remaining.")]
    public int health = 5;
    [Tooltip("The y value at which the player will instantly die.")]
    public float deathDepth = -100;

    [Tooltip("How many seconds the player is invincible after getting hit.")]
    public float invincibilityFrames = 1;
    [HideInInspector]
    public bool isInvicible = false;

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
        healthUI = Instantiate(heartObject, heartPosition, transform.rotation);

        heartRenderer = healthUI.GetComponent<SpriteRenderer>();

        heartRenderer.drawMode = SpriteDrawMode.Tiled;
    }

    void Update()
    {
        heartRenderer.size = new Vector3(health, 1, 1);
        healthUI.transform.position = new Vector3(heartPosition.x + heartWidth * (health - 1) / 2.0f, heartPosition.y, heartPosition.z)
                                    + new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

        if(health <= 0 || transform.position.y <= deathDepth)
        {
            death();
        }
    }

    void death()
    {

    }

    public void dealDamage()
    {
        if (isInvicible == false)
        {
            isInvicible = true;
            health--;
            StartCoroutine(invincible());
        }
    }

    IEnumerator invincible()
    {
        yield return new WaitForSeconds(invincibilityFrames);
        isInvicible = false;
    }
}