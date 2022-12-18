using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly uint VIE_MAX = 10;
    public uint vie { get; private set; }

    public float speed;

    public Vector2 PlayerVelocity;

    private Rigidbody2D _rb;
    private SetValueControls _controls;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerVelocity = new Vector2(_controls.horizontalAxis * speed, _rb.velocity.y);
    }

    private void FixedUpdate()
    {
        _rb.velocity = PlayerVelocity * Time.deltaTime;
    }
}
