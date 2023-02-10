using UnityEngine.Audio;
using UnityEngine;

// Sound class
// everything is self explanatory here
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    [Range(0f, 1f)]
    public float maxVolume;


    [HideInInspector]
    public AudioSource source;

    public float fadeTimeVolume;

    public float fadeTime;
}
