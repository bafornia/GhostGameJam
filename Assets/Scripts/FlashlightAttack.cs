using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightAttack : MonoBehaviour
{
    [Tooltip("How much damage the AOE flash attack does.")]
    public int flashDamage = 1;

    [Tooltip("How long in seconds the flash appears on screen.")]
    public float flashDuration = 1;

    [Tooltip("The game object used for the flash attack.")]
    public GameObject flashObject;
    GameObject tempFlash;

    public KeyCode attackButton = KeyCode.E;

    void Start()
    {
        if (flashObject == null)
        {
            Debug.LogError("Flashlight attack is missing the flash game object");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(attackButton))
        {
            tempFlash = Instantiate(flashObject, transform.position, transform.rotation);

            Flash flashScript = tempFlash.gameObject.GetComponent<Flash>();

            flashScript.flashDuration = flashDuration;
            flashScript.flashDamage = flashDamage;

            tempFlash.gameObject.GetComponent<Flash>().customStart();
        }
    }
}