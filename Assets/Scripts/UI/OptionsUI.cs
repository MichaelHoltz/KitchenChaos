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

    [SerializeField] private Button _moveUpButton;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private Button _interactButton;
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private Button _interactAltButton;
    [SerializeField] private TextMeshProUGUI _interactAltText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TextMeshProUGUI _pauseText;
    [SerializeField] private Transform _presssToRebindKeyTransform;


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
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        _moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        _moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        _moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        _interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        _interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlt); });
        _pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });


    }
    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        UpdateVisual();
        HidePressToRebindKey();
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
        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        _pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    { 
        _presssToRebindKeyTransform.gameObject.SetActive(true); 
    }
    private void HidePressToRebindKey()
    {
        _presssToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBiding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
