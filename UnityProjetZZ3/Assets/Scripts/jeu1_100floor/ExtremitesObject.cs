using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremitesObject : MonoBehaviour
{

    private float _largeurSprite = 0;
    private float _hauteurSprite = 0;

    private float _largeurCollider2D = 0;
    private float _hauteurCollider2D = 0;


    // Start is called before the first frame update
    void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        _largeurSprite = (float)(GetComponent<SpriteRenderer>()?.bounds.size.x);
        _hauteurSprite = (float)(GetComponent<SpriteRenderer>()?.bounds.size.y);

        _largeurCollider2D = (float)(GetComponent<Collider2D>()?.bounds.size.x);
        _hauteurCollider2D = (float)(GetComponent<Collider2D>()?.bounds.size.y);
    }

    // Sprite2D
    public float GetLargeurSprite() { return _largeurSprite; }
    public float GetHauteurSprite() { return _hauteurSprite; }

    public Vector3 GetPositionLeftSprite() { return new Vector3(transform.position.x - _largeurSprite / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionRightSprite() { return new Vector3(transform.position.x + _largeurSprite / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionUpSprite() { return new Vector3(transform.position.x, transform.position.y + _hauteurSprite / 2, transform.position.z); }

    public Vector3 GetPositionDownSprite() { return new Vector3(transform.position.x, transform.position.y - _hauteurSprite / 2, transform.position.z); }


    // Collider2D
    public float GetLargeurCollider2D() { return _largeurCollider2D; }
    public float GetHauteurCollider2D() { return _hauteurCollider2D; }

    public Vector3 GetPositionLeftCollider2D() { return new Vector3(transform.position.x - _largeurCollider2D / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionRightCollider2D() { return new Vector3(transform.position.x + _largeurCollider2D / 2, transform.position.y, transform.position.z); }

    public Vector3 GetPositionUpCollider2D() { return new Vector3(transform.position.x, transform.position.y + _hauteurCollider2D / 2, transform.position.z); }

    public Vector3 GetPositionDownCollider2D() { return new Vector3(transform.position.x, transform.position.y - _hauteurCollider2D / 2, transform.position.z); }


}
