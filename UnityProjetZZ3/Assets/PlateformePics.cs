using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformePics : MonoBehaviour
{
    public uint Degat;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PrendreDamage(Degat);
        }
    }

}
