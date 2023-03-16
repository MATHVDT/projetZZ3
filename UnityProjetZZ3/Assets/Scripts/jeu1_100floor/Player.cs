using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly uint VIE_MAX = 10;
    public uint Vie;

    public float Speed;

    public Vector2 PlayerVelocity;
    public bool ContactPlateforme;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SetValueControls _controls;

    //private Transform transform;
    private Vector3 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
        _animator = GetComponent<Animator>();

        Vie = VIE_MAX;
        ContactPlateforme = false;
        PlayerVelocity = Vector2.zero;

        _animator.Play("AnimationTree");

    }

    // Update is called once per frame
    void Update()
    {

        if (_controls.buttonMenu)
            transform.position = _initialPosition;

        if (Vie == 0)
        {
            //Debug.Log("Fin du niveau.");
            //PlayerVelocity = Vector2.zero;
            //this.enabled = false;
        }

    }

    private void FixedUpdate()
    {

        PlayerVelocity = _rb.velocity;
        PlayerVelocity.x = _controls.horizontalAxis * Speed;
        _rb.velocity = PlayerVelocity;

        _animator.SetFloat("moveX", _controls.horizontalAxis / Math.Abs((_controls.horizontalAxis == 0 ? 1 : _controls.horizontalAxis)));
        _animator.SetFloat("moveY", (ContactPlateforme ? 0.0f : 1.0f));
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
            ContactPlateforme = true;
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
            ContactPlateforme = false;
    }


    public void PrendreDamage(uint damage)
    {
        Vie -= (damage < Vie ? damage : Vie);
    }
}
