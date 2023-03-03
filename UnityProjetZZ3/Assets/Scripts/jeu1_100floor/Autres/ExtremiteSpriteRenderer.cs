using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtremiteSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GetSpriteRenderer2D();
    }

    void GetSpriteRenderer2D()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!_spriteRenderer) { Debug.Log($"Pas de sprite renderer sur l'objet {name}."); }
    }

    private void OnEnable()
    {
        GetSpriteRenderer2D();
    }

    /* ---------------------------- Get Size SpriteRenderer ------------------------ */

    public Vector2 GetSizeSpriteRenderer() { return _spriteRenderer.size; }

    public float GetLargeurSpriteRenderer() { return _spriteRenderer.size.x; }

    public float GetHauteurSpriteRenderer() { return _spriteRenderer.size.y; }


    /* -------------------------- Get Position SpriteRenderer ---------------------- */

    public Vector3 GetPositionLeftSpriteRenderer()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(-_spriteRenderer.bounds.extents.x, 0, 0);
        pos = pos + decalage;
        return pos;

    }

    public Vector3 GetPositionRightSpriteRenderer()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(_spriteRenderer.bounds.extents.x, 0, 0);
        pos = pos + decalage;
        return pos;
    }

    public Vector3 GetPositionUpSpriteRenderer()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(0, _spriteRenderer.bounds.extents.y, 0);
        pos = pos + decalage;
        return pos;
    }

    public Vector3 GetPositionDownSpriteRenderer()
    {
        Vector3 pos = transform.position;
        Vector3 decalage = new Vector3(0, -_spriteRenderer.bounds.extents.y, 0);
        pos = pos + decalage;
        return pos;
    }

    /* -------------------------- Decalage SpriteRenderer -------------------------- */
    public void DecalagePositionToRightSpriteRenderer()
    {
        Vector3 newPos = transform.position - GetPositionLeftSpriteRenderer();
        transform.position += newPos;
    }

    public void DecalagePositionToLeftSpriteRenderer()
    {
        Vector3 newPos = transform.position - GetPositionRightSpriteRenderer();
        transform.position += newPos;
    }

    public void DecalagePositionToUpSpriteRenderer()
    {
        Vector3 newPos = transform.position - GetPositionDownSpriteRenderer();
        transform.position += newPos;
    }

    public void DecalagePositionToDownSpriteRenderer()
    {
        Vector3 newPos = transform.position - GetPositionUpSpriteRenderer();
        transform.position += newPos;
    }

}
