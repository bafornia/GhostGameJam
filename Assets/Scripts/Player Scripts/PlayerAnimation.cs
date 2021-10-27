using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //animator
    [SerializeField]
    Animator _animator;
    //flips if facing left
    private SpriteRenderer _renderer;
    KeyCode attackButton = KeyCode.E;
    void Start()
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

    void Update()
    {
        //animator stuff
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _renderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _renderer.flipX = true;
        }
        else if (Input.GetKeyDown(attackButton))
        {
            _animator.SetBool("On", true);
        }
        float move = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat("Direction", move);
    }
}
