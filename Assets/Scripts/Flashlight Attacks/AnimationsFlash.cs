using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsFlash : MonoBehaviour
{
    //animator
    [SerializeField]
    Animator _animator;
    //flips if facing left
    private SpriteRenderer _renderer;
    KeyCode attackButton = KeyCode.E;
    public float IdleTimer = 0.3f;
    //bool On = false;
    private void Start()
    {
        //making sure everything is there for animation
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("The player sprite is missing an Animator Component");
        }
        //for facing left, makes sure nothing is missing
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
        {
            Debug.LogError("Player Sprite is missing a renderer");
        }
    }


    private void Update()
    {
        //checks if flashlight needs to be flipped
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _renderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _renderer.flipX = true;
        }
        //checks if flashlight needs to be turned on
        else if (Input.GetKeyDown(attackButton))
        {
           _animator.SetBool("On", true);  
        }
        //turns off flashlight shortly after attack button is released
        else if (Input.GetKeyUp(attackButton))
        {
            StartCoroutine(Idle());
        }
        //starts and stops idling
        //float move = Input.GetAxisRaw("Horizontal");
        if (Input.GetAxisRaw("Horizontal") != 0 )
        {
            _animator.SetBool("Idling", false);
        }
        else
        {
            _animator.SetBool("Idling", true);
        }

    }
    IEnumerator Idle()
    {
        yield return new WaitForSeconds(IdleTimer);
        _animator.SetBool("On", false);
    }

}
