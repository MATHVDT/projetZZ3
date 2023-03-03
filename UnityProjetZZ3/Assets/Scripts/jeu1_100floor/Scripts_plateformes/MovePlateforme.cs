using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de déplacement d'une plateforme à vitesse constante vers le haut.
/// Gère la collision avec le player et désactive le BoxCollider2D quand la
/// la plateforme doit devenir traversable.
/// </summary>
public class MovePlateforme : MonoBehaviour
{
    public float Speed;    // Vitesse de déplacement de l'objet 

    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rb;

    public void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        // A l'activation de la plateforme, remise en état d'origine
        // ie que la plateforme est non traversable pour pouvoir appliquer ses effets
        PlateformeNonTraversable();
    }

    private void FixedUpdate()
    {
        // Déplacement de la plateforme vers le haut de facon constante
        _rb.velocity = Vector2.up * Speed;
    }

    /// <summary>
    /// Détecte la collision avec le Player sur la plateforme, 
    /// ie avec le BoxCollider2D (non triggered), vérification
    /// que la collision vient bien du desssus.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.contacts[0].normal.y > 0.5f)
                PlateformeTraversable();
        }
    }

    /// <summary>
    /// Détecte lorsque le Player dépasse la plateforme avec l'EdgeCollider2D (triggered),
    /// puis rend la plateforme traversable pour éviter les collisions par le dessous.
    /// </summary>
    /// <param name="collider"></param>
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlateformeTraversable();
        }

    }

    /// <summary>
    /// Rend la plateforme non solide et traversable. (désactive le collider)
    /// </summary>
    public void PlateformeTraversable()
    {
        _boxCollider2D.enabled = false;
    }

    /// <summary>
    /// Rend la plateforme solide et non traversable. (active le collider)
    /// </summary>
    public void PlateformeNonTraversable()
    {
        _boxCollider2D.enabled = true;
    }
}
