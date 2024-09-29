using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;

    [SerializeField] private Button _musicButton;
    [SerializeField] private TextMeshProUGUI _musicText;

    [SerializeField] private Button _closeButton;

    [SerializeField] private Slider _soundEffectsSlider;
    [SerializeField] private Slider _musicSlider;


    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        _soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _soundEffectsSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.SetVolume(value);
            UpdateVisual();
        });
        _musicSlider.onValueChanged.AddListener((float value) =>
        {
            MusicManager.Instance.SetVolume(value);
            UpdateVisual();
        });
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

    }
    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }


    // Update is called once per frame
    private void UpdateVisual()
    {
        float soundVolume = SoundManager.Instance.GetVolume();
        float musicVolume = MusicManager.Instance.GetVolume();
        _soundEffectsText.text = $"SOUND EFFECTS: {Mathf.Round(soundVolume * 10f)}";
        _musicText.text = $"MUSIC: {Mathf.Round(musicVolume * 10f)}";

        if(_soundEffectsSlider.value != soundVolume)
        {
            _soundEffectsSlider.value = soundVolume;
        }
        if (_musicSlider.value != musicVolume)
        {
            _musicSlider.value = musicVolume;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
