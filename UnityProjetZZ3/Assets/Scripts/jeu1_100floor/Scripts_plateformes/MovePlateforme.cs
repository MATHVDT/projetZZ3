using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de d�placement d'une plateforme � vitesse constante vers le haut.
/// G�re la collision avec le player et d�sactive le BoxCollider2D quand la
/// la plateforme doit devenir traversable.
/// </summary>
public class MovePlateforme : MonoBehaviour
{
    public float Speed;    // Vitesse de d�placement de l'objet 

    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rb;

    public void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnable()
    {
        // A l'activation de la plateforme, remise en �tat d'origine
        // ie que la plateforme est non traversable pour pouvoir appliquer ses effets
        PlateformeNonTraversable();
    }

    private void FixedUpdate()
    {
        // D�placement de la plateforme vers le haut de facon constante
        _rb.velocity = Vector2.up * Speed;
    }

    /// <summary>
    /// D�tecte la collision avec le Player sur la plateforme, 
    /// ie avec le BoxCollider2D (non triggered), v�rification
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
    /// D�tecte lorsque le Player d�passe la plateforme avec l'EdgeCollider2D (triggered),
    /// puis rend la plateforme traversable pour �viter les collisions par le dessous.
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
    /// Rend la plateforme non solide et traversable. (d�sactive le collider)
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
