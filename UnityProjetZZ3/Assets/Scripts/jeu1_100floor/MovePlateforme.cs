using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlateforme : MonoBehaviour
{
    // Vitesse de déplacement de l'objet (en unités par seconde)
    public float Speed;

    private Collider2D[] _colliders2D;
    private Transform _playerTransform;
    private Collider2D _playerCollider2D;
    private ExtremiteBoxCollider2D _extremiteBoxCollider2D;

    public void Start()
    {
        _colliders2D = GetComponentsInChildren<Collider2D>();
        _playerTransform = GameObject.Find("Player").transform;
        _playerCollider2D = GameObject.Find("Player").GetComponent<Collider2D>();
        _extremiteBoxCollider2D = GetComponent<ExtremiteBoxCollider2D>();
    }

    public void OnEnable()
    {
        ActivateColliders2D();
    }

    void Update()
    {
        // On déplace l'objet en utilisant la vitesse et le temps écoulé depuis le dernier frame
        transform.position += Vector3.up * Speed * Time.deltaTime;

        //if (transform.position.y > _playerTransform.position.y)
        //    DesactivateColliders2D();

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (_extremiteBoxCollider2D == null)
                return; // Pas encore récupéré

            float yHautCollider2D = _extremiteBoxCollider2D.GetPositionUpCollider2D().y;
            float yPlayerBasCollider2D = collision.gameObject.GetComponent<ExtremiteBoxCollider2D>().GetPositionDownCollider2D().y;

            if (transform.position.y > collision.transform.position.y)
                DesactivateColliders2D();
        }
    }


    private void DesactivateColliders2D()
    {
        foreach (var collider in _colliders2D)
        {
            collider.isTrigger = true;
        }
    }

    private void ActivateColliders2D()
    {
        foreach (var collider in _colliders2D)
        {
            collider.isTrigger = false;
        }
    }
}
