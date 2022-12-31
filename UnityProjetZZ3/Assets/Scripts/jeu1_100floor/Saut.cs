using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saut : MonoBehaviour
{
    private float _forceSaut;

    private void Start()
    {
        _forceSaut = GetComponentInParent<PlateformeSaut>().ForceSaut;

    }




    // Update is called once per frame
    void Update()
    {
        _forceSaut = GetComponentInParent<PlateformeSaut>().ForceSaut;

    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<Player>().Yvelocity = _forceSaut;
        }
    }
}
