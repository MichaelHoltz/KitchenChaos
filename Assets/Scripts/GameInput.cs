using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGSS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;
    public enum Binding
    { 
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlt,
        Gamepad_Pause
    }

    private PlayerInputActions _playerInputActions;
    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGSS))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGSS));
        }
        _playerInputActions.Player.Enable();

        _playerInputActions.Player.Interact.performed += InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
        _playerInputActions.Player.Pause.performed += PausePerformed;



    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
        _playerInputActions.Player.Pause.performed -= PausePerformed;

        _playerInputActions.Dispose();

    }
    private void PausePerformed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty); 
       
    }

    public Vector2 GetMovementVectorNormalized()
    {
        
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    { 
        switch (binding) {
            default:
            case Binding.Move_Up:
                return _playerInputActions.Player.Move.GetBindingDisplayString(1);
            case Binding.Move_Down:
                return _playerInputActions.Player.Move.GetBindingDisplayString(2);
            case Binding.Move_Left:
                return _playerInputActions.Player.Move.GetBindingDisplayString(3);
            case Binding.Move_Right:
                return _playerInputActions.Player.Move.GetBindingDisplayString(4);
            case Binding.Interact:
                //return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                return _playerInputActions.Player.Interact.GetBindingDisplayString(0);
            case Binding.InteractAlt:
                return _playerInputActions.Player.InteractAlternate.GetBindingDisplayString(0);
            case Binding.Pause:
                return _playerInputActions.Player.Pause.GetBindingDisplayString(0);
            case Binding.Gamepad_Interact:
                return _playerInputActions.Player.Interact.GetBindingDisplayString(1);
            case Binding.Gamepad_InteractAlt:
                return _playerInputActions.Player.InteractAlternate.GetBindingDisplayString(1);
            case Binding.Gamepad_Pause:
                return _playerInputActions.Player.Pause.GetBindingDisplayString(1);
        }

    }

    public void RebindBiding(Binding binding, Action onActionRebound)
    {
        _playerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;


        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlt:
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;

        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete(callback =>
        {
            callback.Dispose();
            _playerInputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGSS, _playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
