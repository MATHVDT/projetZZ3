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


    //private bool _replieeActive;
    //private bool _ecraseeActive;
    //private bool _deplieeActive;

    // Start is called before the first frame update
    void Start()
    {
        //_replieeActive = true;
        //_ecraseeActive = false;
        //_deplieeActive = false;

        Repliee = GameObject.Find("PlateformeSautRepliee");
        Ecrasee = GameObject.Find("PlateformeSautEcrasee");
        Depliee = GameObject.Find("PlateformeSautDepliee");


        Repliee.SetActive(true);
        Ecrasee.SetActive(false);
        Depliee.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision plateforme saut");


        if (collision.gameObject.CompareTag("Player"))
        { // Player sur la plateforme

            float timePause = 0.25f;

            Repliee.SetActive(false);
            Ecrasee.SetActive(true);
            //Debug.Log("Plateforme ecrasée");

            yield return new WaitForSeconds(timePause);

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
