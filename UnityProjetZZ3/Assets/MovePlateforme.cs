using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlateforme : MonoBehaviour
{
    // Vitesse de déplacement de l'objet (en unités par seconde)
    public float Speed;

    void Update()
    {
        // On déplace l'objet en utilisant la vitesse et le temps écoulé depuis le dernier frame
        transform.position += Vector3.up * Speed * Time.deltaTime;
    }
}
