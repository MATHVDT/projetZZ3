using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlateforme : MonoBehaviour
{
    // Vitesse de d�placement de l'objet (en unit�s par seconde)
    public float Speed;

    private Collider2D[] _colliders2D;
    private Transform _playerTransform;

    public void Start()
    {
        _colliders2D = GetComponentsInChildren<Collider2D>();
        _playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        // On d�place l'objet en utilisant la vitesse et le temps �coul� depuis le dernier frame
        transform.position += Vector3.up * Speed * Time.deltaTime;

        //if(transform.position.y > _playerTransform.position.y)
        //    DesactivateColliders2D();

    }

    private void DesactivateColliders2D()
    {
        foreach(var collider in _colliders2D) {
            collider.isTrigger = true;
        }
    }
}
