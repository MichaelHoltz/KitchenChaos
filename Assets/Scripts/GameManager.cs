using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChagned;
    private enum State
    { 
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver    
    }
    private State _state;

    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer = 0f;
    private float _gamePlayingTimerMax = 30f;

    private void Awake()
    {
        Instance = this;
        _state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer <= 0)
                {
                    _state = State.CountdownToStart;
                    OnStateChagned?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                // Start countdown
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer <= 0)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChagned?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                // Game is playing
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer <= 0)
                {
                    _state = State.GameOver;
                    OnStateChagned?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Game over
                break;
        }
        Debug.Log(_state);
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
}
