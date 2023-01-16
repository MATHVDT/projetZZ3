using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourCarte : MonoBehaviour
{
    public float XMinContourCarte;
    public float XMaxContourCarte;

    public float YMinContourCarte;
    public float YMaxContourCarte;

    public Transform[] ContoursCarte;

    // Start is called before the first frame update
    void Start()
    {
        // Get position bord écran
        float objectWidth, objectHeight;

        ContoursCarte = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in ContoursCarte)
        {
            var collider = child.GetComponent<BoxCollider2D>();
            if (collider == null)
                continue;

            objectWidth = collider.size.x + child.transform.lossyScale.x; // récupération de la largeur de l'objet
            objectHeight = collider.size.y + child.transform.lossyScale.y; // récupération de la hauteur de l'objet

            switch (child.name)
            {
                case "MurCarteGauche":                    // Limite gauche
                    XMinContourCarte = (child.transform.position + (objectWidth / 2) * child.transform.right).x;
                    break;
                case "MurCarteDroite":                    // Limite droite
                    XMaxContourCarte = (child.transform.position - (objectWidth / 2) * child.transform.right).x;
                    break;
                case "MurCarteHaut":                    // Limite haut
                    YMinContourCarte = (child.transform.position + (objectHeight / 2) * child.transform.up).y;
                    break;
                case "MurCarteBas":                    // Limite bas
                    YMaxContourCarte = (child.transform.position - (objectHeight / 2) * child.transform.up).y;
                    break;
                default:
                    Debug.Log(child.name); break;

            }
        }
    }
}
