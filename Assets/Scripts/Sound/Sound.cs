using UnityEngine;

[System.Serializable]
public class Sound {
    public AudioConfig.Track track;

    public AudioClip clip;

    [Range(0.1f, 1f)]
    public float volumeMultiply = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource;
}