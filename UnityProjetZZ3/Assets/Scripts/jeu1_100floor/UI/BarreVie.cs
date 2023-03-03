using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreVie : MonoBehaviour
{
    // Images des barres de vie
    private Image _barreViePleine;
    private Image _barreVieVide;

    // Valeur pour le fill l'image
    public float SizeBlocVieFill; // largeur pour fill 1 barre de vie
    public float MinFillAmount;
    public float MaxFillAmount;

    private uint _vie;

    // Start is called before the first frame update
    void Start()
    {
        // Récupération des images
        var imagesComponents = GetComponentsInChildren<Image>();

        // Image barre vide récupérée en 1 (affichée d'abord)
        // car en dessous de l'image barre pleine (réaffichée sur l'autre)
        _barreVieVide = imagesComponents[0].GetComponent<Image>();
        _barreViePleine = imagesComponents[1].GetComponent<Image>();
    }

    /// <summary>
    /// Change la valeur d'affichage de la barre de vie.
    /// </summary>
    /// <param name="vie">Nouvelle valeur de la vie.</param>
    public void ChangeVie(uint vie)
    {
        _vie = vie;
        _barreViePleine.fillAmount = MinFillAmount + _vie * SizeBlocVieFill;
    }
}
