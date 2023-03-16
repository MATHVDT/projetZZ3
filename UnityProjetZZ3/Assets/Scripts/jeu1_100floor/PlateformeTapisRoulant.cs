using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTapisRoulant : MonoBehaviour
{
    public float VitesseTapisRoulant;
    public int sens;

    public void Start()
    {
        GetComponent<Animator>().SetInteger("sens", sens);
        VitesseTapisRoulant *= sens;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(VitesseTapisRoulant, 0), ForceMode2D.Force);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        }
    }
}
