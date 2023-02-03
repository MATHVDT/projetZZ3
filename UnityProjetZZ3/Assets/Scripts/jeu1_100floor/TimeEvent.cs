using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Ce script sert � g�rer tous les �v�nements du jeu qui sont li�es au temps.
/// * incr�mentation du score
/// * r�g�n�ration de la vie
/// * (gestion du d�but de partie, ie vie texte start, plateforme immobile etc...)
/// </summary>
public class TimeEvent : MonoBehaviour
{
    public float TimeToVieInc;
    public float TimeToScoreInc;

    Player _player;

    public uint _score;

    public float _scoreTime;
    public float _vieTime;

    public float _currentTime;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = Time.time;

        // Augmente le score suivant un chrono
        if (_currentTime - _scoreTime > TimeToScoreInc)
        {
            _scoreTime = _currentTime;
            _score++;
        }

        // R�g�n�re le player suivant un chrono
        if (_currentTime - _vieTime > TimeToVieInc)
        {
            _vieTime = _currentTime;
            _player.RegenerationVie();
        }

    }
}
