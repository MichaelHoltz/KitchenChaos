using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string Player_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private float _musicVolume = 0.4f;
    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _musicVolume = PlayerPrefs.GetFloat(Player_PREFS_MUSIC_VOLUME, 0.4f);
        _audioSource.volume = _musicVolume;
    }


    /// <summary>
    /// For when clicking on the sound effects button
    /// </summary>
    public void ChangeVolume()
    {
        _musicVolume += 0.1f;
        if (_musicVolume > 1.05f)
        {
            _musicVolume = 0f;
        }
        _audioSource.volume = _musicVolume;
        PlayerPrefs.SetFloat(Player_PREFS_MUSIC_VOLUME, _musicVolume);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// For when changing the volume slider
    /// </summary>
    /// <param name="value"></param>
    public void SetVolume(float value)
    {
        _musicVolume = value;
        _audioSource.volume = _musicVolume;
        PlayerPrefs.SetFloat(Player_PREFS_MUSIC_VOLUME, _musicVolume);
        PlayerPrefs.Save();

    }

    public float GetVolume()
    {
        return _musicVolume;
    }
}
