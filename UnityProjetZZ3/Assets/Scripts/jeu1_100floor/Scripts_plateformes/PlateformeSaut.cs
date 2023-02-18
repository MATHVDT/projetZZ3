using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateformeSaut : MonoBehaviour
{

    //public GameObject Repliee;
    //public GameObject Ecrasee;
    //public GameObject Depliee;

    public float ForceSaut;
    public bool Saut; // Variable activé dans l'animation

    private Animator _animator;
    private Rigidbody2D _rb;

    private Rigidbody2D _rbPlayer;


    // Start is called before the first frame update
    void Start()
    {
        Saut = false;

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _rbPlayer = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!Saut)
                _animator.SetTrigger("AnimationPlay");
        }
    }

    public void AddForceSautPlayer()
    {
        Vector2 velocity = _rbPlayer.velocity;

        // Calcul de la force a appliquée en fonction de la velocity
        Vector2 relativeVelocity = _rbPlayer.velocity - _rb.velocity;
        float jumpForceFactor = 1.0f; // ajustez ce facteur selon vos besoins
        float jumpForce = ForceSaut + relativeVelocity.y * jumpForceFactor;

        Vector2 force = new Vector2(0, jumpForce + Mathf.Abs(velocity.y));
        _rbPlayer.AddForce(force, ForceMode2D.Impulse); // Applique la force
    }
}
