using UnityEngine;

public class ScrollBackground : MonoBehaviour
{

    void Start()
    {
        string stateName = "ScrollVertical";
        Animator[] animators = GetComponentsInChildren<Animator>();

        foreach (var animator in animators)
        {
            // Récupère un nombre float aléatoire entre 0 et 1
            float startClip = Random.Range(0.0f, 1.0f);

            // Lance l'animation en debutant a :
            // startClip % de la longueur de l'animation
            animator.Play(stateName, -1, startClip);
        }
    }
}
