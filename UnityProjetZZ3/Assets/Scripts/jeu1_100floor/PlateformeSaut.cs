using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateformeSaut : MonoBehaviour
{

    public GameObject Repliee;
    public GameObject Ecrasee;
    public GameObject Depliee;

    public float ForceSaut;




    // Start is called before the first frame update
    void Start()
    {
        Repliee = GameObject.Find("PlateformeSautRepliee");
        Ecrasee = GameObject.Find("PlateformeSautEcrasee");
        Depliee = GameObject.Find("PlateformeSautDepliee");

        Repliee.SetActive(true);
        Ecrasee.SetActive(false);
        Depliee.SetActive(false);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        { // Player sur la plateforme
            float timePause = 0.25f;

            Repliee.SetActive(false);
            Ecrasee.SetActive(true);
            //Debug.Log("Plateforme ecrasée");

            yield return new WaitForSeconds(timePause / 2);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceSaut), ForceMode2D.Impulse);
            Ecrasee.SetActive(false);
            Depliee.SetActive(true);
            //Debug.Log("Plateforme depliée");

            yield return new WaitForSeconds(timePause);

            Depliee.SetActive(false);
            Repliee.SetActive(true);
            //Debug.Log("Plateforme repliée");

        }
    }

}
