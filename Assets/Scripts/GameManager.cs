using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    { 
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver    
    }
    private State _state;


    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer = 0f;
    private float _gamePlayingTimerMax = 5f * 60f; // 5 minutes
    private bool _isGamePaused = false;

    private void Awake()
    {
        Instance = this;
        _state = State.WaitingToStart;
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction; 
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_state == State.WaitingToStart)
        {
            _state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                // Start countdown
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer <= 0)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Game is playing
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer <= 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Game over
                break;
        }
        //Debug.Log(_state);
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    { 
        return _state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        if (_gamePlayingTimer > 0)
        {
            return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
        }
        else
        { 
            return 0; 
        }
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if(_isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1f;
        }
    }
}
