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

    [Tooltip("How much damage the powerful beam attack does.")]
    public int beamDamage = 3;
    [Tooltip("How logn it takes to charge the beam attack.")]
    public float chargeTime = 0.5f;
    [Tooltip("How long in seconds the beam appears on screen.")]
    public float beamDuration = 1;
    [Tooltip("The game object used for the beam attack.")]
    public GameObject beamObject;
    float chargeTimer = 0;

    GameObject attackObject;

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
        chargeTimer += Time.deltaTime;

        if (Input.GetKeyDown(attackButton))
        {
            chargeTimer = 0;
        }

        if (Input.GetKeyUp(attackButton))
        {
            Quaternion angle = Quaternion.AngleAxis(Mathf.Rad2Deg *
                               Mathf.Atan2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")), transform.forward);

            if (chargeTimer >= chargeTime)  // beam attack
            {
                attackObject = Instantiate(beamObject, transform.position, angle);

                Attack attackScript = attackObject.gameObject.GetComponent<Attack>();

                attackScript.attackDuration = beamDuration;
                attackScript.attackDamage = beamDamage;

                attackScript.customStart();
            }
            else // flash attack
            {
                attackObject = Instantiate(flashObject, transform.position, angle);

                Attack attackScript = attackObject.gameObject.GetComponent<Attack>();

                attackScript.attackDuration = flashDuration;
                attackScript.attackDamage = flashDamage;

                attackScript.customStart();
            }
        }
    }
}