using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlateforme : MonoBehaviour
{
    // Vitesse de d�placement de l'objet (en unit�s par seconde)
    public float Speed;

    void Update()
    {
        // On d�place l'objet en utilisant la vitesse et le temps �coul� depuis le dernier frame
        transform.position += Vector3.up * Speed * Time.deltaTime;
    }
}
