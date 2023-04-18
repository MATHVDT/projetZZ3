using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Script d'effet de la plateforme saut.
/// Lance l'animation qui applique la force de saut au player.
/// </summary>
public class PlateformeSaut : MonoBehaviour
{
    public float ForceSaut;
    public bool Saut; // Variable activée dans l'animation

    private Animator _animator;
    private Rigidbody2D _rb;
    private Rigidbody2D _rbPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        Saut = false;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _rbPlayer = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!Saut)
                _animator.SetTrigger("AnimationPlay");
        }
    }

    /// <summary>
    /// Applique une force de saut sur le Player.
    /// La force appliquée est relative à la vitesse de déplacement 
    /// de la plateforme, pour quel soit constante.
    /// </summary>
    public void AddForceSautPlayer()
    {
        Vector2 velocity = _rbPlayer.velocity; // Récupération des forces sur le Player

        // Calcul de la force à appliquer en fonction de la velocity
        Vector2 relativeVelocity = _rbPlayer.velocity - _rb.velocity;

        // Calcule de la force
        float jumpForceFactor = 1.0f; // ajustez le facteur selon vos besoins
        float jumpForce = ForceSaut + relativeVelocity.y * jumpForceFactor;
        Vector2 force = new Vector2(0, jumpForce + Mathf.Abs(velocity.y));

        _rbPlayer.AddForce(force, ForceMode2D.Impulse); // Applique la force
    }
}
