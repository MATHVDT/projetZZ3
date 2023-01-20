using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContourCarte : MonoBehaviour
{
    public float XMinContourCarte;
    public float XMaxContourCarte;

    public float YMinContourCarte;
    public float YMaxContourCarte;

    // Start is called before the first frame update
    void Start()
    {

        SetCoordMinMaxContourCarte();

        var extremiteObject = GetComponent<ExtremitesObject>();

        Debug.Log($"XMinContourCarte:{XMinContourCarte} et ExtremiteObject:{extremiteObject.GetPositionLeftCollider2D()}");
        Debug.Log($"XMaxContourCarte:{XMaxContourCarte} et ExtremiteObject:{extremiteObject.GetPositionRightCollider2D()}");

        Debug.Log($"YMinContourCarte:{YMinContourCarte} et ExtremiteObject:{extremiteObject.GetPositionDownCollider2D()}");
        Debug.Log($"YMaxContourCarte:{YMaxContourCarte} et ExtremiteObject:{extremiteObject.GetPositionUpCollider2D()}");
    }


    public void SetCoordMinMaxContourCarte()
    {
        // Get position bord écran
        float objectWidth, objectHeight;

        var collider = gameObject.GetComponent<BoxCollider2D>();

        objectWidth = collider.size.x; // récupération de la largeur de l'objet
        objectHeight = collider.size.y; // récupération de la hauteur de l'objet

        XMinContourCarte = (transform.position.x + collider.offset.x - (objectWidth / 2)); // Récupération de la position gauche du contour de l'objet
        XMaxContourCarte = (transform.position.x + collider.offset.x + (objectWidth / 2)); // Récupération de la position droite du contour de l'objet

        YMinContourCarte = (transform.position.y + collider.offset.y - (objectHeight / 2)); // Récupération de la position basse du contour de l'objet
        YMaxContourCarte = (transform.position.y + collider.offset.y + (objectHeight / 2)); // Récupération de la position haute du contour de l'objet
    }
}
