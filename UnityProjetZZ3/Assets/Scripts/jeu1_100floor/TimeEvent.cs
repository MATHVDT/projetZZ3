using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Ce script sert à gérer tous les évènements du jeu qui sont liées au temps.
/// * incrémentation du score
/// * régénération de la vie
/// * (gestion du début de partie, ie vie texte start, plateforme immobile etc...)
/// </summary>
public class TimeEvent : MonoBehaviour
{
    public float TimeToVieInc;
    public float TimeToScoreInc;
    //public float TimeBeforeStartGame;

    private Player _player;

    public uint _score = 1;

    private float _scoreTime;
    private float _vieTime;

    private float _currentTime;

    private AffichageScore _affichageScore;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _affichageScore = GameObject.Find("[UI] Score").GetComponent<AffichageScore>();
        Debug.Log("start jeu1_100FLOOR");
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = Time.time;

        // Augmente le score suivant un chrono
        if (_currentTime - _scoreTime > TimeToScoreInc)
        {
            _scoreTime = _currentTime;
            _affichageScore.ChangerScore(++_score); // Envoie le nouveau score à l'affichage
        }

        // Régénère le player suivant un chrono
        if (_currentTime - _vieTime > TimeToVieInc)
        {
            _vieTime = _currentTime;
            _player.RegenerationVie();
        }
    }

}
