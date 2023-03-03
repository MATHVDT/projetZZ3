using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script d'effet de la plateforme avec de pics.
/// Applique des dégats quand le Player entre en collision avec par le haut.
/// </summary>
public class PlateformePics : MonoBehaviour
{
    public uint Degat;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verifie si le player est au dessus de la plateforme
            if (collision.gameObject.transform.position.y > transform.position.y)
                collision.gameObject.GetComponent<Player>().PrendreDegats(Degat);
        }
    }

}
