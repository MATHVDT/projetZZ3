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

    private Collider2D[] _colliders2D;
    private Rigidbody2D _rb;

    public void Start()
    {
        _colliders2D = GetComponentsInChildren<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        // A l'activation de la plateforme, réactivation de tous ses colliders
        ActivateTriggerOnAllColliders2D();
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
            if (transform.position.y > collision.transform.position.y)
                DesactivateTriggerOnAllColliders2D();
        }
    }

    /// <summary>
    /// Désactive la propritété Trigger de tous les colliders 2D de la plateforme.
    /// </summary>
    private void DesactivateTriggerOnAllColliders2D()
    {
        if (_colliders2D == null) return;

        foreach (var collider in _colliders2D)
        {
            collider.isTrigger = true;
        }
    }

    /// <summary>
    /// Active la propritété Trigger de tous les colliders 2D de la plateforme.
    /// </summary>
    private void ActivateTriggerOnAllColliders2D()
    {
        if (_colliders2D == null) return;

        foreach (var collider in _colliders2D)
        {
            collider.isTrigger = false;
        }
    }
}
