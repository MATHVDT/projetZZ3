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
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Xvelocity = VitesseTapisRoulant * sens;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Xvelocity = 0;
        }
    }
}
