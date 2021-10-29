using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightAttack : MonoBehaviour
{

    [Tooltip("Time before the player can attack again.")]
    public float attackBuffer = 0.1f;
    float bufferTimer = 0;
    [Tooltip("Lock player movement while attacking?")]
    public bool doLocking = true;

    [Tooltip("How much damage the AOE flash attack does.")]
    public int flashDamage = 1;
    [Tooltip("How long in seconds the flash appears on screen.")]
    public float flashDuration = 1;
    [Tooltip("The game object used for the flash attack.")]
    public GameObject flashObject;
    [Tooltip("Sound used for the flash attack.")]
    public AudioClip flashSound;

    [Tooltip("How much damage the beam attack does.")]
    public int beamDamage = 3;
    [Tooltip("How long in seconds the beam appears on screen.")]
    public float beamDuration = 1;
    [Tooltip("The game object used for the beam attack.")]
    public GameObject beamObject;
    [Tooltip("Sound used for the beam attack.")]
    public AudioClip beamSound;
    [Tooltip("How long it takes to charge the beam attack.")]
    public float chargeTime = 0.5f;
    float chargeTimer = 0;

    AudioSource myAudio;
    GameObject attackObject;

    public KeyCode attackButton = KeyCode.E;
    public KeyCode positionLock = KeyCode.LeftShift;

    PlayerMovement aimLockScript;

    float lastxInput = 0;

    void Start()
    {
        bufferTimer = attackBuffer;

        aimLockScript = GameObject.Find("player").gameObject.GetComponentInParent<PlayerMovement>();

        myAudio = GetComponent<AudioSource>();

        if (flashObject == null)
        {
            Debug.LogError("Flashlight attack is missing the flash game object");
        }
    }

    void Update()
    {
        bufferTimer += Time.deltaTime;
        chargeTimer += Time.deltaTime;

        if (Input.GetKeyDown(positionLock))
        {
            aimLockScript.aimLock = true;
        }
        else if (Input.GetKeyUp(positionLock))
        {
            aimLockScript.aimLock = false;
        }

        if (Input.GetKeyDown(attackButton) && bufferTimer >= attackBuffer)
        {
            chargeTimer = 0;
        }

        lastxInput -= BoolToInt(Input.GetAxisRaw("Horizontal") != 0) * (lastxInput - Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyUp(attackButton) && bufferTimer >= attackBuffer)
        {
            Quaternion angle = Quaternion.AngleAxis(Mathf.Rad2Deg *
                               Mathf.Atan2(Input.GetAxisRaw("Vertical"), lastxInput), transform.forward);

            if (chargeTimer >= chargeTime)  // beam attack
            {
                myAudio.PlayOneShot(beamSound);

                attackObject = Instantiate(beamObject, transform.position, angle);

                Attack attackScript = attackObject.gameObject.GetComponent<Attack>();

                attackScript.playerObject = gameObject;
                attackScript.attackDuration = beamDuration;
                attackScript.attackDamage = beamDamage;
                attackScript.doLocking = doLocking;

                attackScript.customStart();

                bufferTimer = 0 - beamDuration;
            }
            else // flash attack
            {
                myAudio.PlayOneShot(flashSound);

                attackObject = Instantiate(flashObject, transform.position, angle);

                Attack attackScript = attackObject.gameObject.GetComponent<Attack>();

                attackScript.playerObject = gameObject;
                attackScript.attackDuration = flashDuration;
                attackScript.attackDamage = flashDamage;
                attackScript.doLocking = doLocking;

                attackScript.customStart();

                bufferTimer = 0 - flashDuration;
            }
        }
    }

    int BoolToInt(bool i)
    {
        return System.Convert.ToInt32(i);
    }
}