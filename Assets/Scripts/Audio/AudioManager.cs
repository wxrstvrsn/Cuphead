using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioClip[] sfxClipsNarratorA;
    [SerializeField] private AudioClip[] sfxClipsNarratorB;
    
    [SerializeField] private AudioClip pickUpSound;

    private Dictionary<string, AudioClip> musicDict;
    private Dictionary<string, AudioClip> sfxDict;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // сохраняем между сценами
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация словарей
        musicDict = new Dictionary<string, AudioClip>();
        sfxDict = new Dictionary<string, AudioClip>();

        foreach (var clip in musicClips)
            musicDict[clip.name] = clip;

        foreach (var clip in sfxClips)
            sfxDict[clip.name] = clip;
    }

    public void PlayNarratorA()
    {
        int index = GetRandomIndex(sfxClipsNarratorA);
        sfxSource.PlayOneShot(sfxClipsNarratorA[index]);
    }
    
    public void PlayNarratorB()
    {
        int index = GetRandomIndex(sfxClipsNarratorB);
        sfxSource.PlayOneShot(sfxClipsNarratorB[index]);
    }

    public void PlayMusic(string name, bool loop = true)
    {
        if (musicDict.TryGetValue(name, out var clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"[AudioManager] Музыка '{name}' не найдена.");
        }
    }

    public void PlayAmbient()
    {
        sfxSource.PlayOneShot(pickUpSound);
        musicSource.Stop();
        if (sfxDict.TryGetValue("sfx_OpticalLoop", out var clip))
        {
            ambientSource.clip = clip;
            musicSource.loop = true;
            ambientSource.Play();
        }
    }

    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    private int GetRandomIndex(AudioClip[] clips)
    {
        return Random.Range(0, clips.Length);
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] Звук '{name}' не найден.");
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}