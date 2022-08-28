using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Sound
{
    public enum AudioType
    {
        SFX,
        BGM
    }
    public AudioType audioType;

    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip audioClip;
    public string clipName;

    public bool isLoop;
    public bool playOnAwake;
    [Range(0,1)]
    public float volume = 0.5f;
}