using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    private static string BGMBasePath = "Sound/BGM";
    private static string SEBasePath = "Sound/SE";

    [SerializeField]
    private AudioSource _bgmAudioSource;

    [SerializeField]
    private AudioSource _seAudioSource;

    private string _currentBGMSoundName = string.Empty;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void ChangeBGM(string soundName)
    {
        if (_currentBGMSoundName.Equals(soundName))
        {
            return;
        }
        _currentBGMSoundName = soundName;
        _bgmAudioSource.Stop();
        var clip = Resources.Load($"{BGMBasePath}/{soundName}") as AudioClip;
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.volume = 0.1f;
        _bgmAudioSource.Play();
    }

    public void PlaySE(string soundName)
    {
        var clip = Resources.Load($"{SEBasePath}/{soundName}") as AudioClip;
        _seAudioSource.PlayOneShot(clip);
    }
}
