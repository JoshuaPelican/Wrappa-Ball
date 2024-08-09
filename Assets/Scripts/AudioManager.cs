using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioMixerGroup MixerGroup;

    private void Awake()
    {
        MixerGroup = GetComponent<AudioSource>().outputAudioMixerGroup;
    }

    public static void PlayAudio(AudioClip clip, float volume, float pitch)
    {
        AudioSource source = new GameObject("Audio Source").AddComponent<AudioSource>();

        DontDestroyOnLoad(source.gameObject);

        source.outputAudioMixerGroup = MixerGroup;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }

    public static void PlayAudioRandom(AudioClip[] clips, float volume, float pitch)
    {
        PlayAudio(clips[Random.Range(0, clips.Length)], volume, pitch);
    }
}
