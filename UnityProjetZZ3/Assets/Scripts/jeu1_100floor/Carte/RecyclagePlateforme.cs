using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclagePlateforme : MonoBehaviour
{
    public PlateformeGenerator generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("GenerateurPlateforme").GetComponent<PlateformeGenerator>();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(EdgeCollider2D))
        {
            if (collision.gameObject.CompareTag("Plateforme"))
            {
                //Debug.Log($"Sortie de la zone {collision.gameObject.name}");
                collision.gameObject.SetActive(false);
                generator.GeneratePlateforme();
            }
        }
    }
}
