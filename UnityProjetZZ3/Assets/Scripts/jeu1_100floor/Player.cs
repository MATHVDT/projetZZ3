using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public readonly uint VIE_MAX = 10;
    public uint Vie;
    public float Speed;

    public Vector2 PlayerVelocity;
    public bool ContactPlateforme;

    private Rigidbody2D _rb;
    private Animator _animator;

    private SetValueControls _controls;
    private BarreVie _barreVie;

    //private Transform transform;
    private Vector3 _initialPosition; // TODO à dégager

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position; // TODO à dégager

        // Récupération des components
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Récupération des scripts liés à l'UI
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
        _barreVie = GameObject.Find("[UI] BarreVie").GetComponent<BarreVie>();

        // Initialisation des variables
        Vie = VIE_MAX;
        ContactPlateforme = false;
        PlayerVelocity = Vector2.zero;
        _animator.Play("AnimationTree");

    }

    // Update is called once per frame
    void Update()
    {
        // TODO Bouton de DEBUG 
        if (_controls.buttonMenu)
        {
            transform.position = _initialPosition;
            PlayerVelocity = _rb.velocity;
            PlayerVelocity.y = 0;
            _rb.velocity = PlayerVelocity;
        }

        // Gestion de la FIN du niveau
        if (Vie == 0)
        {
            //Debug.Log("Fin du niveau.");
            //PlayerVelocity = Vector2.zero;
            //this.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        // Calcul du déplacement du player 
        PlayerVelocity = _rb.velocity; // Récupération de la force appliquée sur le player
        PlayerVelocity.x = _controls.horizontalAxis * Speed; // Calcul de la nouvelle composante X de cette force en fonction des controls
        _rb.velocity = PlayerVelocity; // Application de la force sur le player

        // Lancement des animations du player en fonction de son déplacement
        _animator.SetFloat("moveX", _controls.horizontalAxis / Math.Abs((_controls.horizontalAxis == 0 ? 1 : _controls.horizontalAxis)));
        _animator.SetFloat("moveY", (ContactPlateforme ? 0.0f : 1.0f));
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
            ContactPlateforme = true;
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
            ContactPlateforme = false;
    }

    /// <summary>
    /// Réduit la vie d'une certaine valeur et actualise l'affichage de la barre de vie.
    /// </summary>
    /// <param name="degats">Valeur de degats recu.</param>
    public void PrendreDegats(uint degats)
    {
        Vie -= (degats < Vie ? degats : Vie);
        _barreVie.ChangeVie(Vie); // Mise à jour de l'affichage
    }

    /// <summary>
    /// Augmente la vie d'une certaine valeur (1 par défaut) et actualise l'affichage de la barre de vie.
    /// La Vie de doit pas dépasser la constante VIE_MAX.
    /// </summary>
    /// <param name="pt">Valeur de la régénération de la vie, 1 par défaut.</param>
    public void RegenerationVie(uint value = 1)
    {
        if (Vie < VIE_MAX)
        {
            Vie += value;
            _barreVie.ChangeVie(Vie); // Mise à jour de l'affichage
        }
    }
}
