using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public AudioClip musicClip; // Arrastra aqu� tu archivo de m�sica desde el inspector
    private AudioSource audioSource;

    public void Awake()
    {
        // Aseg�rate de que este GameObject no se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);

        // Configura el AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true; // Hacer que la m�sica se reproduzca en bucle
        audioSource.playOnAwake = false; // No iniciar autom�ticamente

        // Inicia la reproducci�n
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
