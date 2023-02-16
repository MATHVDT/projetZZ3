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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("enter on collision avec player");
            _animator.SetTrigger("AnimationPlay");
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (Saut && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = _rb.velocity;

            // Calcul de la force a appliquée en fonction de la velocity
            Vector2 force = new Vector2(0, ForceSaut + Mathf.Abs(velocity.y));
            playerRb.AddForce(force, ForceMode2D.Impulse); // Applique la force

            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
            Saut = false;
        }
    }
}
