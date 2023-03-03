using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script d'effet de la plateforme tapis roulant.
/// Choix du sens de la plateforme, ajustement de la vitesse et animation
/// en fonction du sens. Application d'une force horitontale sur le Player.
/// </summary>
public class PlateformeTapisRoulant : MonoBehaviour
{
    public enum Sens { Gauche = -1, Droite = +1 };

    public float VitesseTapisRoulant;
    public Sens sens; // Prend les valeurs -1 ou +1

    /// <summary>
    /// Set up du sens de la plateforme quand elle est créée.
    /// </summary>
    public void Start()
    {
        // Choix de la direction 
        switch (Random.Range(0, 2))
        {
            case 0:
                sens = Sens.Gauche;
                break;
            case 1:
                sens = Sens.Droite;
                break;
        }

        // En fonction du sens
        VitesseTapisRoulant *= (int)sens; // Calcule de la vitesse 
        GetComponent<Animator>().SetInteger("sens", (int)sens); // Choix de l'animation 
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { // Application d'une force horizontale sur le Player 
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(VitesseTapisRoulant, 0), ForceMode2D.Force);
        }
    }
}
