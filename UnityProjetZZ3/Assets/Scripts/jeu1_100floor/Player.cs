using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly uint VIE_MAX = 10;
    public uint vie { get; private set; }

    public float Speed;
    public float Yvelocity;
    public float Xvelocity;
    public float Gravity;
    public float GravityVelocity;

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

        ContactPlateforme = false;
        PlayerVelocity = Vector2.zero;

        Yvelocity = Gravity;
        GravityVelocity = Gravity;
        Xvelocity = 0;

        _animator.Play("AnimationTree");

    }

    // Update is called once per frame
    void Update()
    {
        if (_controls.buttonMenu)
            transform.position = _initialPosition;

        PlayerVelocity = new Vector2(_controls.horizontalAxis * Speed + Xvelocity, Yvelocity);

        if (Yvelocity > 0)
        {
            Yvelocity += GravityVelocity * Time.deltaTime;
        }
        else
        {
            Yvelocity = GravityVelocity;
        }

        //if (Xvelocity > 0)
        //{
        //    Xvelocity -= 10*GravityVelocity * Time.deltaTime;
        //}
        //else
        //{
        //    Xvelocity = 0;
        //}

    }

    private void FixedUpdate()
    {
        _rb.velocity = PlayerVelocity;
        _animator.SetFloat("moveX", _controls.horizontalAxis / Math.Abs((_controls.horizontalAxis == 0 ? 1 : _controls.horizontalAxis)));
        _animator.SetFloat("moveY", (ContactPlateforme ? 0.0f : 1.0f));

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPlateforme = true;
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        ContactPlateforme = false;
    }


    public void ResetGravityPlayer()
    {
        GravityVelocity = Gravity;
    }
}
