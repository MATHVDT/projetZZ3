using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            // Récupère le collider de la plateforme pour sur laquelle 
            // le player est posée, et la rend trigger
            List<ContactPoint2D> listContacts = new List<ContactPoint2D>();
            collision.GetContacts(listContacts);
            foreach (ContactPoint2D contact in listContacts)
            {
                contact.collider.isTrigger = true;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.isTrigger = false;
        }
    }
}
