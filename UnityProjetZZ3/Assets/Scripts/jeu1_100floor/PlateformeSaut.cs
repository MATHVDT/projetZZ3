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
    public bool Saut;

    public Animator animator;
    private Rigidbody2D _rbPlayer;


    // Start is called before the first frame update
    void Start()
    {
        Saut = false;
        //Repliee = GameObject.Find("PlateformeSautRepliee");
        //Ecrasee = GameObject.Find("PlateformeSautEcrasee");
        //Depliee = GameObject.Find("PlateformeSautDepliee");

        //Repliee.SetActive(true);
        //Ecrasee.SetActive(false);
        //Depliee.SetActive(false);

        animator = GetComponent<Animator>();

    }

    //private IEnumerator OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.gameObject.CompareTag("Player"))
    //    { // Player sur la plateforme
    //        float timePause = 0.25f;

    //        Repliee.SetActive(false);
    //        Ecrasee.SetActive(true);
    //        //Debug.Log("Plateforme ecrasée");

    //        yield return new WaitForSeconds(timePause / 2);

    //        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
    //        Ecrasee.SetActive(false);
    //        Depliee.SetActive(true);
    //        //Debug.Log("Plateforme depliée");

    //        yield return new WaitForSeconds(timePause);

    //        Depliee.SetActive(false);
    //        Repliee.SetActive(true);
    //        //Debug.Log("Plateforme repliée");

    //    }
    //}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("AnimationPlay");
            //_rbPlayer = collision.gameObject.GetComponent<Rigidbody2D>();
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (Saut && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Saut");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
            Saut = false;
        }
    }

    public void AddForceJumpPlayer()
    {
        //_rbPlayer.AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
    }
}
