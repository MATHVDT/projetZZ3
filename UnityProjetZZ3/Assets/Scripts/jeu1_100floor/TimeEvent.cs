using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Ce script sert � g�rer tous les �v�nements du jeu qui sont li�es au temps.
/// * incr�mentation du score
/// * r�g�n�ration de la vie
/// * (gestion du d�but de partie, ie vie texte start, plateforme immobile etc...)
/// </summary>
public class TimeEvent : MonoBehaviour
{
    public float TimeToScoreInc;
    //public float TimeBeforeStartGame;

    private uint _score = 1;

    private AffichageScore _affichageScore;

    // Start is called before the first frame update
    void Start()
    {
        _affichageScore = GameObject.Find("[UI] Score").GetComponent<AffichageScore>();
        Debug.Log("start jeu1_100FLOOR");
        StartCoroutine(IncrementScore());
    }

    private IEnumerator IncrementScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToScoreInc);
            _affichageScore.ChangerScore(++_score); // Envoie le nouveau score � l'affichage
        }
    }
}
