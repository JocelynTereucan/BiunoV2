using UnityEngine;

public class ReproductorAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clipInicio;

    void Start()
    {
        audioSource.clip = clipInicio;
        audioSource.Play();
    }

    public void ReproducirDeNuevo()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
