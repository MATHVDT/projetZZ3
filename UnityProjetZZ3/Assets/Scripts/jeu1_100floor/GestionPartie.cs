using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EtatPartie
{
    Debut,
    EnCours,
    Fin,
}

/// <summary>
/// Ce script sert à gérer tous les évènements du jeu qui sont liées au temps.
/// * incrémentation du score
/// * régénération de la vie
/// * (gestion du début de partie, ie vie texte start, plateforme immobile etc...)
/// </summary>
public class GestionPartie : MonoBehaviour
{
    public EtatPartie Partie { get; private set; }

    public float TimeToScoreInc;
    //public float TimeBeforeStartGame;

    private uint _score = 1;
    private AffichageScore _affichageScore;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Debut jeu1_100FLOOR");
        Partie = EtatPartie.Debut;

        _affichageScore = GameObject.Find("[UI] Score").GetComponent<AffichageScore>();

        // Truc de début regene etc ...
        StartCoroutine(EcranDebut());
        Debug.Log("En cours jeu1_100FLOOR");

       
    }

    private IEnumerator EcranDebut()
    {
        yield return new WaitForSeconds(5f);
        Partie = EtatPartie.EnCours;
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
