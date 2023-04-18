using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inflige les d�gats max au Player lorsqu'il sort de l'�cran.
/// </summary>
public class ZoneBasEcran : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var scriptPlayer = collision.gameObject.GetComponent<Player>();
            scriptPlayer.PrendreDegats(scriptPlayer.VIE_MAX);
        }
    }
}
