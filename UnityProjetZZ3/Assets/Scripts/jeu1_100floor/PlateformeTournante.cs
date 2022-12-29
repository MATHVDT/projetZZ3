using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTournante : MonoBehaviour
{
    public float ForceFreinage;

    public float GravityWithFreinage;
    private float _gravity;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _gravity = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Gravity;
        GravityWithFreinage = _gravity / ForceFreinage;
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("AnimationPlay");
            collision.gameObject.GetComponent<Player>().GravityVelocity = GravityWithFreinage;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().ResetGravityPlayer();
        }
    }
}
