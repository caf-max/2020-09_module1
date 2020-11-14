using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;

    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    [Range(-3.0f, 3.0f)]
    public float pitch = 1.0f;

    public bool loop = false;
}

[CreateAssetMenu(fileName = "AudioData", menuName = "AudioData", order = 50)]
public class AudioData : ScriptableObject
{
    [SerializeField]
    private Sound[] sounds = default;

    public Sound GetSound(string nameClip)
    {
        return sounds.Where(sound => sound.name == nameClip).FirstOrDefault();
    }

    public Sound[] GetSounds()
    {
        return sounds;
    }
}
