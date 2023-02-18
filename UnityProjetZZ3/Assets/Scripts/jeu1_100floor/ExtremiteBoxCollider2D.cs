using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremiteBoxCollider2D : MonoBehaviour
{
    private BoxCollider2D _collider2D;
    private EdgeCollider2D _edgeCollider2D;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GetCollider2D();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void GetCollider2D()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _edgeCollider2D = GetComponent<EdgeCollider2D>();

        if (!_collider2D) { Debug.Log($"Pas de box collider sur l'objet {name}."); }
    }

    private void OnEnable()
    {
        GetCollider2D();
    }


    /* ---------------------------- Get Size Collider 2D ------------------------ */
    public Vector2 GetSizeCollider2D() { return _collider2D.size; }

    public float GetLargeurCollider2D() { return _collider2D.size.x; }

    public float GetHauteurCollider2D() { return _collider2D.size.y; }


    /* -------------------------- Get Position Collider 2D ---------------------- */

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


    /* -------------------------- Decalage Collider 2D -------------------------- */
    public void DecalagePositionToRightCollider2D()
    {
        Vector3 newPos = transform.position - GetPositionLeftCollider2D();
        transform.position += newPos;
    }

    public void DecalagePositionToLeftCollider2D()
    {
        Vector3 newPos = transform.position - GetPositionRightCollider2D();
        transform.position += newPos;
    }

    public void DecalagePositionToUpCollider2D()
    {
        Vector3 newPos = transform.position - GetPositionDownCollider2D();
        transform.position += newPos;
    }

    public void DecalagePositionToDownCollider2D()
    {
        Vector3 newPos = transform.position - GetPositionUpCollider2D();
        transform.position += newPos;
    }
}
