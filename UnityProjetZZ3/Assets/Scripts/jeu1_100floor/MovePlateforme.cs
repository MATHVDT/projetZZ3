using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de déplacement d'une plateforme à vitesse constante vers le haut.
/// Gère la collision par le dessous de la plateforme, 
/// en rendant trigger tous les colliders2D pour ne pas bloquer le player.
/// </summary>
public class MovePlateforme : MonoBehaviour
{
    // Vitesse de déplacement de l'objet (en unités par seconde)
    public float Speed;

    private EdgeCollider2D _edgeCollider2D;
    private BoxCollider2D _boxCollider2D;

    private Rigidbody2D _rb;

    public void Awake()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        // A l'activation de la plateforme, réactivation de tous ses colliders
        PlateformeNonTraversable();
    }

    private void FixedUpdate()
    {
        // Déplacement de la plateforme vers le haut
        _rb.velocity = Vector2.up * Speed;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //if (_extremiteBoxCollider2D == null) // Pour éviter les soucis au start (avec des colliders pas encore récup)
            //    return; // Pas encore récupéré

            // Rend la plateforme traversable dès qu'elle passe au dessus du player
            // Collision avec le pLayer par en dessous la plateforme
            //if (transform.position.y > collision.transform.position.y)
            //    DesactivateTriggerOnAllColliders2D();

            if (collision.contacts[0].normal.y > 0.5f)
                PlateformeTraversable();

        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            //if (collider)
            //    Debug.Log("Pas normal c'est le collider box qui aurait du detecer ca !");

            PlateformeTraversable();
        }

    }

    /// <summary>
    /// Rend la plateforme non solide et traversable.
    /// </summary>
    public void PlateformeTraversable()
    {
        _boxCollider2D.enabled = false;
    }

    /// <summary>
    /// Rend la plateforme solide et non traversable. 
    /// </summary>
    public void PlateformeNonTraversable()
    {
        _boxCollider2D.enabled = true;
    }
}
