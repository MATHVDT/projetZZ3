using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremitePlateforme : MonoBehaviour
{
    public BoxCollider2D _collider2D;

    public float _largeurCollider2D = 0;
    public float _hauteurCollider2D = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateValueCollider2D();
    }

    void UpdateValueCollider2D()
    {
        _collider2D = GetComponent<BoxCollider2D>();

        if (_collider2D)
        {
            Debug.Log($"BoxCollider found pour {name}");
            _largeurCollider2D = _collider2D.size.x;
            _hauteurCollider2D = _collider2D.size.y;
        }
        else
        {
            Debug.Log($"Pas de box collider sur l'objet {name}.");
        }
    }


    private void OnEnable()
    {
        UpdateValueCollider2D();
    }

    public void Update()
    {
        //Debug.Log($"GetPositionLeftCollider2D:{GetPositionLeftCollider2D()}");
        //Debug.Log($"GetPositionRightCollider2D:{GetPositionRightCollider2D()}");
        //Debug.Log($"GetPositionUpCollider2D:{GetPositionUpCollider2D()}");
        //Debug.Log($"GetPositionDownCollider2D:{GetPositionDownCollider2D()}");
    }


    public Vector2 GetSizeCollider2D() { return _collider2D.size; }

    public float GetLargeurCollider2D() { return _collider2D.size.x; }

    public float GetHauteurCollider2D() { return _collider2D.size.y; }



    public Vector3 GetPositionLeftCollider2D()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(_collider2D.offset.x - _collider2D.size.x / 2, 0, 0);
        pos = pos + transform.lossyScale.x * decalage;
        return pos;
    }
    public Vector3 GetPositionRightCollider2D()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(_collider2D.offset.x + _collider2D.size.x / 2, 0, 0);
        pos = pos + transform.lossyScale.x * decalage;
        return pos;
    }

    public Vector3 GetPositionUpCollider2D()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(0, _collider2D.offset.y + _collider2D.size.y / 2, 0);
        pos = pos + transform.lossyScale.y * decalage;
        return pos;
    }

    public Vector3 GetPositionDownCollider2D()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(0, _collider2D.offset.y - _collider2D.size.y / 2, 0);
        pos = pos + transform.lossyScale.y * decalage;
        return pos;
    }

}
