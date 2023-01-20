using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremitesObject : MonoBehaviour
{

    public float _largeurSpriteRenderer = 0;
    public float _hauteurSpriteRenderer = 0;

    public float _largeurCollider2D = 0;
    public float _hauteurCollider2D = 0;

    public Bounds _boundsSpriteRenderer;
    public Bounds _boundsCollider2D;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        var sprite = GetComponent<SpriteRenderer>();

        if (sprite != null)
        {
            _boundsSpriteRenderer = sprite.bounds;

            _largeurSpriteRenderer = _boundsSpriteRenderer.size.x;
            _hauteurSpriteRenderer = _boundsSpriteRenderer.size.y;
        }
        else
        {
            Debug.Log($"Pas de sprite sur {transform.name}");
        }

        var collider = GetComponent<Collider2D>();

        if (collider != null)
        {
            _boundsCollider2D = GetComponent<Collider2D>().bounds;

            _largeurCollider2D = _boundsCollider2D.max.x - _boundsCollider2D.min.x;
            _hauteurCollider2D = _boundsCollider2D.max.y - _boundsCollider2D.min.y;
        }
        else
        {
            Debug.Log($"Pas de collider sur {transform.name}");
        }

        Debug.Log($"name:{name}, GetPositionLeftSpriteRenderer:{GetPositionLeftSpriteRenderer()}");
        Debug.Log($"name:{name}, GetPositionRightSpriteRenderer:{GetPositionRightSpriteRenderer()}");
        Debug.Log($"name:{name}, GetPositionUpSpriteRenderer:{GetPositionUpSpriteRenderer()}");
        Debug.Log($"name:{name}, GetPositionDownSpriteRenderer:{GetPositionDownSpriteRenderer()}");

        Debug.Log($"name:{name}, GetPositionLeftCollider2D:{GetPositionLeftCollider2D()}");
        Debug.Log($"name:{name}, GetPositionRightCollider2D:{GetPositionRightCollider2D()}");
        Debug.Log($"name:{name}, GetPositionUpCollider2D:{GetPositionUpCollider2D()}");
        Debug.Log($"name:{name}, GetPositionDownCollider2D:{GetPositionDownCollider2D()}");

    }

    // Sprite2D
    public float GetLargeurSpriteRenderer() { return _largeurSpriteRenderer; }
    public float GetHauteurSpriteRenderer() { return _hauteurSpriteRenderer; }

    public Vector3 GetPositionLeftSpriteRenderer() { return new Vector3(transform.position.x - _largeurSpriteRenderer / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionRightSpriteRenderer() { return new Vector3(transform.position.x + _largeurSpriteRenderer / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionUpSpriteRenderer() { return new Vector3(transform.position.x, transform.position.y + _hauteurSpriteRenderer / 2, transform.position.z); }

    public Vector3 GetPositionDownSpriteRenderer() { return new Vector3(transform.position.x, transform.position.y - _hauteurSpriteRenderer / 2, transform.position.z); }


    // Collider2D
    public float GetLargeurCollider2D() { return _largeurCollider2D; }
    public float GetHauteurCollider2D() { return _hauteurCollider2D; }

    public Vector3 GetPositionLeftCollider2D()
    {
        var pos = new Vector3(transform.position.x - _boundsCollider2D.extents.x, transform.position.y, transform.position.z);
        return pos;
    }

    public Vector3 GetPositionRightCollider2D()
    {
        var pos = new Vector3(transform.position.x + _boundsCollider2D.extents.x, transform.position.y, transform.position.z);
        return pos;
    }

    public Vector3 GetPositionUpCollider2D()
    {
        var pos = new Vector3(transform.position.x, transform.position.y + _boundsCollider2D.extents.y, transform.position.z);
        return pos;
    }

    public Vector3 GetPositionDownCollider2D()
    {
        var pos = new Vector3(transform.position.x, transform.position.y - _boundsCollider2D.extents.y, transform.position.z);
        return pos;
    }


}
