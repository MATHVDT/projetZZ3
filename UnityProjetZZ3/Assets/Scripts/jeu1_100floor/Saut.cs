using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Saut : MonoBehaviour
{
    private float _forceSaut;
    private Rigidbody2D _rbPlayer;


    private void Start()
    {
        _forceSaut = GetComponent<PlateformeSaut>().ForceSaut;
    }




    // Update is called once per frame
    void Update()
    {
        // TODO A virer apres, juste pour les test
        _forceSaut = GetComponentInParent<PlateformeSaut>().ForceSaut;

    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (this.enabled && collision.gameObject.CompareTag("Player"))
        //{
        //    Debug.Log("Saut");
        //    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, _forceSaut), ForceMode2D.Impulse);
        //    this.enabled = false;
        //}
    }
}
