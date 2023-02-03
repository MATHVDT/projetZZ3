using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformePics : MonoBehaviour
{
    public uint Damage;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Verifie si le player est au dessus de la plateforme
            if (collision.gameObject.transform.position.y > transform.position.y)
                collision.gameObject.GetComponent<Player>().PrendreDamage(Damage);
        }
    }

}
