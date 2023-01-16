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
    }


    public void SetCoordMinMaxContourCarte()
    {
        // Get position bord écran
        float objectWidth, objectHeight;

        var collider = gameObject.GetComponent<BoxCollider2D>();

        objectWidth = collider.size.x; // récupération de la largeur de l'objet
        objectHeight = collider.size.y; // récupération de la hauteur de l'objet

        XMinContourCarte = (transform.position.x + collider.offset.x - (objectWidth / 2));
        XMaxContourCarte = (transform.position.x + collider.offset.x + (objectWidth / 2));

        YMinContourCarte = (transform.position.y + collider.offset.y - (objectHeight / 2));
        YMaxContourCarte = (transform.position.y + collider.offset.y + (objectHeight / 2));
    }
}
