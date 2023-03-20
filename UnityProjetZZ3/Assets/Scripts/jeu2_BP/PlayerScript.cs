using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Vector3 _initialPosition;
    private Rigidbody2D _rb;
    private SetValueControls _controls;
    private Vector2 _playerVelocity;
    private int _maxBombs;
    public int _currentBombs;

    public float MovementSpeed;
    public GameObject _bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _maxBombs = 1;
        _currentBombs = _maxBombs;

        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (_controls.buttonMenu) // debug
        //    transform.position = _initialPosition;
    }

    private void FixedUpdate()
    {
        if (_controls.buttonA && _currentBombs > 0)
        {
            _currentBombs--;

            GameObject bomb = Instantiate(_bombPrefab);
            Vector3 bombPosition = transform.position + new Vector3(0, - transform.localScale.y / 4, 0);

            //On ajuste les coordonn�es de la bombe pour q'uelle se trouve exactement sur une case de la grille
            bomb.transform.position = new Vector3(Mathf.Floor(bombPosition.x) + (float)0.5,
                                                (Mathf.Floor(bombPosition.y) + (float)0.5) * (float)0.7,
                                                Mathf.Floor(bombPosition.z) + (float)0.5);
            bomb.SetActive(true);
        }

        _playerVelocity = _rb.velocity;
        _playerVelocity.x = _controls.horizontalAxis * MovementSpeed;
        _playerVelocity.y = _controls.verticalAxis * MovementSpeed;
        _rb.velocity = _playerVelocity;
    }
}
