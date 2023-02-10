using UnityEngine.Audio;
using UnityEngine;
using System;

/**
 * This class handles the audio world of our game.
 * It plays sounds and handles their volume and so on
 * 
 **/

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] overlappingSounds;

    // set volume and pitch and clip of every sound of sounds and explosions
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        foreach (Sound s in overlappingSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        // find sound in sounds
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // only play sound if it's not playing already
        if (s != null && !s.source.isPlaying)
        {

            if (s.fadeTime == 0)
            {
                s.source.Play();
            }
            else
            {
                // in case we want the wind to fade in, well start this coroutine
                //StartCoroutine(AudioHelper.FadeIn(s.source, s.fadeTime, s.volume));
            }
        }
    }

    // basically same as above just reverted
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source.isPlaying)
        {
            if (s.fadeTime == 0)
            {
                s.source.Stop();
            }
            else
            {
                //StartCoroutine(AudioHelper.FadeOut(s.source, s.fadeTime));
            }
        }

    }

    // we want to increase the volume fadingly at certain events for some sounds
    public void IncreaseVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null && s.source.isPlaying)
        {

            //StartCoroutine(AudioHelper.FadeInVolume(s.source, s.fadeTimeVolume, s.maxVolume));
        }
    }

    // decrease the volume fadingly
    public void DecreaseVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null && s.source.isPlaying)
        {
            //StartCoroutine(AudioHelper.FadeOutVolume(s.source, s.fadeTimeVolume, s.volume));
        }
    }

    // play explosion sound, separate function because we dont care if the sound is already playing
    public void PlayOverlapping(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    // play fireball sound , separate function because we dont care if the sound is already playing
    public void PlayFireball()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "Fireball2");
        if (s != null)
        {
            s.source.Play();
        }
    }
}