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

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
        {
            //Debug.Log($"Sortie de la zone {collision.gameObject.name}");
            collision.gameObject.SetActive(false);
            generator.GeneratePlateforme();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"collision {collision.gameObject.name}");
        collision.gameObject.SetActive(false);
        generator.GeneratePlateforme();
    }
}
