using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private BoxCollider2D _bc;
    public int _currentTimer;

    public int _initialTimer;

    // Start is called before the first frame update
    void Start()
    {
        _bc = GetComponent<BoxCollider2D>();
        _currentTimer = _initialTimer;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (_currentTimer > 0) _currentTimer--;
        else GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") _bc.isTrigger = false;
    }
}
