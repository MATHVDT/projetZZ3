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
            _affichageScore.ChangerScore(++_score); // Envoie le nouveau score à l'affichage
        }
    }
}
