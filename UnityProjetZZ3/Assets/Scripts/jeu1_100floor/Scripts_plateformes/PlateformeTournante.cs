using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTournante : MonoBehaviour
{
    public float ForceFreinage;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        animator.SetTrigger("AnimationPlay");
    //        // Ajoute des frottements pour ralentir la chute
    //        //collision.gameObject.GetComponent<Rigidbody2D>().drag = ForceFreinage;
    //        //this.GetComponent<Collider2D>().enabled = false;
    //    }
    //}

    //public void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        // Supprime les frottements 
    //        //collision.gameObject.GetComponent<Rigidbody2D>().drag = 0;
    //    }
    //}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lance l'animation : 
            // - change le sprite
            // - rend le collider triggered
            animator.SetTrigger("AnimationPlay");

            var rbPlayer = collision.gameObject.GetComponent<Rigidbody2D>();
            rbPlayer.velocity = Vector3.zero;
        }
    }
}
