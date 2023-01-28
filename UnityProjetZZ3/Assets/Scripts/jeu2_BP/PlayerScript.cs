using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Rigidbody2D _rb;
    private SetValueControls _controls;
    private Vector2 _playerVelocity;

    public float MovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();

        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controls.buttonMenu)
            transform.position = _initialPosition;
    }

    private void FixedUpdate()
    {

        _playerVelocity = _rb.velocity;
        _playerVelocity.x = _controls.horizontalAxis * MovementSpeed;
        _playerVelocity.y = _controls.verticalAxis * MovementSpeed;
        _rb.velocity = _playerVelocity;
    }
}
