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

        Debug.Log($"force saut : {_forceSaut}");
    }




    // Update is called once per frame
    void Update()
    {
        _forceSaut = GetComponentInParent<PlateformeSaut>().ForceSaut;

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Collision stay");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter");
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, _forceSaut), ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Player>().Yvelocity = _forceSaut;
        }
    }
}
