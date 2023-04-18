using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Infilge des dégats au Player lorsqu'il touche les pics du haut de l'écran.
/// </summary>
public class PicsHautEcran : MonoBehaviour
{
    // Start is called before the first frame update
    public uint Damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enleve la Vie au Player
            collision.gameObject.GetComponent<Player>().PrendreDegats(Damage);
        }
    }
}
