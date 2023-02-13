using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AffichageScore : MonoBehaviour
{
    // Nombre de digit pour afficher le score
    private const uint N = 4;

    // Element contenant le texte � afficher
    private TextMeshProUGUI _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        // R�cup�ration du component
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// R�cup�re le neouveau score et met � jour correctement l'affichage.
    /// </summary>
    /// <param name="newScore">Le nouveau score � afficher.</param>
    public void ChangerScore(uint newScore)
    {
        // Transformation du score en Texte � 4 digits
        string scoreString = newScore.ToString().PadLeft((int)N, '0');
        _scoreText.text = scoreString; // Set du texte sur le component
    }

}
