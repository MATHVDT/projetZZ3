using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombScript : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _bc;
    private Tilemap _tilemap;
    public int _currentTimer;

    public int _initialTimer = 10;
    public int _range = 1;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bc = GetComponent<BoxCollider2D>();
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        _currentTimer = _initialTimer;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (_currentTimer > 0) _currentTimer--;
        else
        {
            Explosion();
            GameObject.Find("Player").GetComponent<PlayerScript>()._currentBombs += 1;
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") _bc.isTrigger = false;
    }

    private void Explosion()
    {
        TileExplosion(true, true);
        TileExplosion(true, false);
        TileExplosion(false, true);
        TileExplosion(false, false);
    }

    private void TileExplosion(bool horizontal, bool positive)
    {
        bool obstacle = false;
        int sign = 1;
        int i = 0;

        if (!positive)
        {
            sign = -1;
        }

        while (i < _range && !obstacle)
        {
            Vector3Int tileCoord;

            i++;

            if (horizontal)
            {
                tileCoord = Vector3Int.FloorToInt(transform.position) + new Vector3Int(sign * i, 0, 0);
            }
            else
            {
                tileCoord = Vector3Int.FloorToInt(transform.position) + new Vector3Int(0, sign * i, 0);
            }
            TileBase tile = _tilemap.GetTile(tileCoord);

            if (tile != null)
            {
                obstacle = true;
                if (tile.name == "Sprites_obstacles_0") _tilemap.SetTile(tileCoord, null);
            }
        }
    }
}
