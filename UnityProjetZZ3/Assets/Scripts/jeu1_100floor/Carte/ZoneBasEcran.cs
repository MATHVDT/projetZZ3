using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneBasEcran : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var scriptPlayer = collision.gameObject.GetComponent<Player>();
            scriptPlayer.PrendreDegats(scriptPlayer.VIE_MAX);
        }
    }
}
