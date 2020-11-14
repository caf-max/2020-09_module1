using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance { get; private set; }
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        GameObject _go1 = new GameObject("music");
        _go1.transform.SetParent(this.transform);
        musicSource = _go1.AddComponent<AudioSource>();

        soundSources = new AudioSource[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject _go = new GameObject("sound_" + i);
            _go.transform.SetParent(this.transform);
            soundSources[i] = _go.AddComponent<AudioSource>();
        }
    }

    private AudioSource musicSource;
    private AudioSource[] soundSources;
    public AudioData audioData;

    public void PlaySound(string name)
    {
        Sound sound = audioData.GetSound(name);
        if (sound != null)
        {
            AudioSource source = musicSource;
            if (!sound.loop)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!soundSources[i].isPlaying)
                    {
                        source = soundSources[i];
                    }
                    if (soundSources[i].isPlaying && soundSources[i].clip == sound.clip)
                    {
                        return;
                    }
                }
            }
            source.clip = sound.clip;
            source.pitch = sound.pitch;
            source.volume = sound.volume;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.audioMixerGroup;

            source.Play();
        }
        else
        {
            Debug.Log("Cant find sound " + name);
        }
    }

}
