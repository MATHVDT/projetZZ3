using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioPlateforme : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();
        }
    }
}
