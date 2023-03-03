using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script d'effet de la plateforme tournante.
/// Lance l'animation lorsque le player rentre en contact avec, 
/// et le stop dans sa chute.
/// </summary>
public class PlateformeTournante : MonoBehaviour
{
    private Animator _animator;

    void Start() { _animator = GetComponent<Animator>(); }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Lance l'animation : 
            // - change le sprite
            // - rend la plateforme traversable via un event trigger 
            _animator.SetTrigger("AnimationPlay");

            var rbPlayer = collision.gameObject.GetComponent<Rigidbody2D>();
            rbPlayer.velocity = Vector3.zero;
        }
    }
}
