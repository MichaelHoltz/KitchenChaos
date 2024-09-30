using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _KeyMoveUpText;
    [SerializeField] private TextMeshProUGUI _KeyMoveDownText;
    [SerializeField] private TextMeshProUGUI _KeyMoveLeftText;
    [SerializeField] private TextMeshProUGUI _KeyMoveRightText;
    [SerializeField] private TextMeshProUGUI _keyMoveInteractText;
    [SerializeField] private TextMeshProUGUI _keyMoveInteractAltText;
    [SerializeField] private TextMeshProUGUI _keyMovePauseText;

    [SerializeField] private TextMeshProUGUI _keyMoveGamepadInteractText;
    [SerializeField] private TextMeshProUGUI _keyMoveGamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI _keyMoveGamepadPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if(GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    { 
        _KeyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        _KeyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        _KeyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        _KeyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        _keyMoveInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _keyMoveInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
        _keyMovePauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        _keyMoveGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        _keyMoveGamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlt);
        _keyMoveGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }   
}
