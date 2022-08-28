using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instace;
    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioMixerGroup bgmMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;

    private void Awake()
    {
        instace = this;
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;

            s.audioSource.loop = s.isLoop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.volume = s.volume;
            switch (s.audioType)
            {
                case Sound.AudioType.SFX:
                    s.audioSource.outputAudioMixerGroup = sfxMixer;
                    break;
                case Sound.AudioType.BGM:
                    s.audioSource.outputAudioMixerGroup = bgmMixer;
                    break;
            }
            if (s.playOnAwake)
            {
                s.audioSource.Play();
            }
        }
    }

    public void PlayClipByName(string _clipname)
    {
        Sound soundToPlay = Array.Find(sounds, x => x.clipName == _clipname);
        if (soundToPlay != null)
            switch (soundToPlay.audioType)
            {
                case Sound.AudioType.SFX:
                    soundToPlay.audioSource.Play();
                    break;
                case Sound.AudioType.BGM:
                    if (!soundToPlay.audioSource.isPlaying)
                        soundToPlay.audioSource.Play();
                    break;
            }
    }

    public void PlayRandomClipType(Sound.AudioType playType)
    {
        Sound[] songs = Array.FindAll(sounds, x => x.audioType == playType);
        Sound soundToPlay = songs[UnityEngine.Random.Range(0, songs.Length)];
        soundToPlay.audioSource.Play();
    }

    public void StopClipByName(string _clipname)
    {
        Sound soundToPlay = Array.Find(sounds, x => x.clipName == _clipname);
        if (soundToPlay != null)
            soundToPlay.audioSource.Stop();
    }

    public void UpdateMixerVolume()
    {
        bgmMixer.audioMixer.SetFloat("BGM", Mathf.Log10(OptionMenu.musicVolume) * 20);
        sfxMixer.audioMixer.SetFloat("SFX", Mathf.Log10(OptionMenu.soundEffectVolume) * 20);
    }

    public void StopAllClip()
    {
        foreach(Sound s in sounds)
        {
            s.audioSource.Stop();
        }
    }
}
