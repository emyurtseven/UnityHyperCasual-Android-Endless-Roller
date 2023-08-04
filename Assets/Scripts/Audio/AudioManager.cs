using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages seperate playing of music and/or sfx from a central static class.
/// The AudioManager needs two seperate objects with AudioSource components on the scene: 
/// MusicPlayer and SfxPlayer, with their respective scripts attached. 
/// </summary>
public static class AudioManager
{
    // are scene objects initialized?
    static bool musicInitialized = false;
    static bool sfxInitialized = false;

    // script references on game objects in scene
    static MusicPlayer musicPlayer;
    static SoundEffectsPlayer sfxPlayer;

    // AudioClip library
    public static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();


    /// <summary>
    /// Gets whether or not the audio manager has been initialized
    /// </summary>
    public static bool SfxInitialized { get => sfxInitialized; }
    public static bool MusicInitialized { get => musicInitialized; }
    
    public static MusicPlayer MusicPlayer { get => musicPlayer; }
    public static SoundEffectsPlayer SfxPlayer { get => sfxPlayer; }


    /// <summary>
    /// Initializes music player object.
    /// </summary>
    /// <param name="source">MusicPlayer script attached to the object</param>
    public static void Initialize(MusicPlayer player)
    {
        musicPlayer = player;
        musicInitialized = true;
    }

    /// <summary>
    /// Initializes sfx player object and populates AudioClip library.
    /// </summary>
    /// <param name="source">SoundEffectsPlayer script attached to the object</param>
    public static void Initialize(SoundEffectsPlayer player)
    {
        sfxPlayer = player;
        sfxInitialized = true;

        foreach (AudioClipName clipName in Enum.GetValues(typeof(AudioClipName)))
        {
            string clipNameString = Enum.GetName(typeof(AudioClipName), clipName);
            AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + clipNameString);

            if (audioClip != null)
            {
                audioClips.Add(clipName, audioClip);
            }
        }
    }

    /// <summary>
    /// Plays audio clip with the given name without looping.
    /// </summary>
    /// <param name="name">name of the audio clip to play</param>
    public static void PlaySfx(AudioClipName name, float volume=1f)
    {
        if (audioClips.ContainsKey(name))
        {
            sfxPlayer.AudioSource.PlayOneShot(audioClips[name], volume);
        }
    }

    /// <summary>
    /// Plays audio clip with the given name with looping enabled. 
    /// Use for music clips
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="volume"></param>
    public static void PlayMusic(AudioClipName clipName, float volume=1f)
    {
        musicPlayer.AudioSource.Stop();
        
        if (!audioClips.ContainsKey(clipName))
        {
            Debug.LogWarning("Audio file {name} missing");
            return;
        }
        musicPlayer.AudioSource.clip = audioClips[clipName];
        musicPlayer.AudioSource.loop = true;
        musicPlayer.AudioSource.volume = volume;
        musicPlayer.AudioSource.Play();
    }

    /// <summary>
    /// Static wrapper function. Implemented in MusicPlayer script.
    /// </summary>
    public static void PlayMusicFadeIn(AudioClipName clipName, float volume, float fadeDuration, float fadeDelay = 0)
    {
        musicPlayer.StopAllCoroutines();
        musicPlayer.AudioSource.loop = true;
        musicPlayer.PlayMusicFadeIn(audioClips[clipName], volume, fadeDuration, fadeDelay);
    }

    /// <summary>
    /// Static wrapper function. Implemented in MusicPlayer script.
    /// </summary>
    public static void FadeInMusic(float finalVolume, float fadeDuration, float fadeDelay=0)
    {
        musicPlayer.StopAllCoroutines();
        musicPlayer.StartCoroutine(musicPlayer.FadeInAudio(finalVolume, fadeDuration, fadeDelay));
    }

    /// <summary>
    /// Static wrapper function. Implemented in MusicPlayer script.
    /// </summary>
    public static void FadeOutMusic(float finalVolume, float fadeDuration, float fadeDelay=0)
    {
        musicPlayer.StopAllCoroutines();
        musicPlayer.StartCoroutine(musicPlayer.FadeOutAudio(finalVolume, fadeDuration, fadeDelay));
    }

    public static void StopMusic()
    {
        musicPlayer.Stop();
    }
}
