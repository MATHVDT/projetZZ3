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
    public float Gravity;
    public float GravityVelocity;

    public Vector2 PlayerVelocity;
    public bool ContactPlateforme;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SetValueControls _controls;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
        _animator = GetComponent<Animator>();

        ContactPlateforme = false;
        PlayerVelocity = Vector2.zero;

        Yvelocity = Gravity;

        _animator.Play("AnimationTree");

    }

    // Update is called once per frame
    void Update()
    {
        PlayerVelocity = new Vector2(_controls.horizontalAxis * Speed, Yvelocity);

        if (Yvelocity > Gravity)
            Yvelocity -= GravityVelocity;

        //Debug.Log($"x :  {PlayerVelocity.x / Math.Abs((PlayerVelocity.x == 0 ? 1 : PlayerVelocity.x))}, y : {(ContactPlateforme ? 0.0f : -1.0f)}");
    }

    private void FixedUpdate()
    {
        _rb.velocity = PlayerVelocity * Time.deltaTime;
        _animator.SetFloat("moveX", PlayerVelocity.x / Math.Abs((PlayerVelocity.x == 0 ? 1 : PlayerVelocity.x)));
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

}
