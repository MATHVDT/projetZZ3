using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GestionPartie _gestionPartie;

    public readonly uint VIE_MAX = 10;
    public uint Vie;
    public float TimeToRegenVie;
    public float Speed;

    public Vector2 PlayerVelocity;
    public bool ContactPlateforme;

    private Renderer _renderer;
    private Rigidbody2D _rb;
    private Animator _animator;
    private AudioSource _audioSource;


    private Launcher _launcher;
    private SetValueControls _controls;
    private BarreVie _barreVie;

    //private Transform transform;
    private Vector3 _initialPosition; // TODO à dégager
    private bool _finPartie = false;

    // Start is called before the first frame update
    void Start()
    {
        _gestionPartie = GetComponentInParent<GestionPartie>();

        _initialPosition = transform.position; // TODO à dégager

        // Récupération des components
        _renderer = GetComponent<Renderer>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        // Récupération des scripts liés à l'UI
        _launcher = GameObject.Find("[UI] Launcher").GetComponent<Launcher>();
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
        _barreVie = GameObject.Find("[UI] BarreVie").GetComponent<BarreVie>();


        // Initialisation des variables
        Vie = VIE_MAX;
        ContactPlateforme = false;
        PlayerVelocity = Vector2.zero;
        _animator.Play("AnimationTree");

        //while (_gestionPartie.Partie == EtatPartie.Debut) ;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO Bouton de DEBUG 
        if (_controls.buttonB)
        {
            transform.position = _initialPosition;
            PlayerVelocity = _rb.velocity;
            PlayerVelocity.y = 0;
            _rb.velocity = PlayerVelocity;
        }

        // Gestion de la FIN du niveau
        if (Vie == 0)
        {
            if (!_finPartie)
            {
                Debug.Log("Fin du niveau.");
                _finPartie = true;
                StopAllCoroutines();
                _audioSource.Play();
            }
            else
            {
                if (!_audioSource.isPlaying)
                    _launcher.ChargerMenuPrincipal();
            }

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

    /// <summary>
    /// Détecte quand le Player attérit sur une plateforme et active le booléen ContactPlateforme à true.
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateforme"))
            ContactPlateforme = true;
    }


    /// <summary>
    /// Détecte quand le Player n'est plus sur une plateforme et désactive le booléen ContactPlateforme à false.
    /// </summary>
    /// <param name="collision"></param>
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
        StartCoroutineUnique(Clignoter()); // 
        Vie -= (degats < Vie ? degats : Vie);
        _barreVie.ChangeVie(Vie); // Mise à jour de l'affichage
        StartCoroutineUnique(RegenerationVie(TimeToRegenVie));
    }

    /// <summary>
    /// [Coroutine] Augmente la vie d'une certaine valeur (1 par défaut) et actualise l'affichage de la barre de vie,
    /// toutes les TimeToRegenVie secondes. 
    /// La Vie de doit pas dépasser la constante VIE_MAX.
    /// </summary>
    /// <param name="pt">Valeur de la régénération de la vie, 1 par défaut.</param>
    private IEnumerator RegenerationVie(float tempsRegen, uint value = 1)
    {
        while (!_finPartie && Vie < VIE_MAX)
        {
            yield return new WaitForSeconds(tempsRegen);
            Vie += value;
            _barreVie.ChangeVie(Vie); // Mise à jour de l'affichage
        }
    }

    /// <summary>
    /// [Coroutine] Permet de faire clignoter la texture du Player en modifiant la couleur du renderer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Clignoter()
    {
        int nbTransitionTotale = 5; // Nombre de transitions totales (Rouge -> Normal ou N -> R)
        // Temps d'attente entre chaque transition : R (0.11) -> N (0.07) -> R (0.11) -> N (0.05) -> R (0.09) -> N
        float[] tempsAttenteAvantTransition = { 0.11f, 0.07f, 0.11f, 0.05f, 0.09f };
        Color couleurSprite;

        for (int i = 0; i < nbTransitionTotale; ++i)
        {
            couleurSprite = (i % 2 == 0 ? Color.red : Color.white); // Choix du filtre
            _renderer.material.color = couleurSprite; // Application du filtre sur le renderer
            yield return new WaitForSeconds(tempsAttenteAvantTransition[i]); // Attente avant changement filtre
        }
        _renderer.material.color = Color.white; // Reset du filtre 
    }

    /// <summary>
    /// Permet de lancer une coroutine de manière unique en vérifiant que la coroutine précédente s'est terminée avant de lancer la nouvelle.
    /// Cela permet d'éviter des conflits ou des effets indésirables.
    /// </summary>
    /// <param name="coroutine">La coroutine que l'on souhaite executée de façon unique.</param>
    void StartCoroutineUnique(IEnumerator coroutine)
    {
        StopCoroutine(coroutine);
        StartCoroutine(coroutine);
    }
}
