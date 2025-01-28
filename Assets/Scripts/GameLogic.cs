using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static Action OnGameStarted;
    public static Action OnGameRestartHold;
    public static Action OnGameReset;
    public static Action Playing;
    
    public static GameState _gameState;

    private void Start()
    {
        _gameState = GameState.ReadyToStart; 
    }

    private void OnEnable()
    {
        InputManager.OnMouseClicked += GameLogicArranger;
        CollisionDetection.Collided += Collided;
        OnGameReset += HandleGameReset;
        Playing += HandlePlaying;
    }

    private void OnDisable()
    {
        InputManager.OnMouseClicked -= GameLogicArranger;
        CollisionDetection.Collided -= Collided;
        OnGameReset -= HandleGameReset;
        Playing -= HandlePlaying;
    }
    
    private void GameLogicArranger()
    {
        if (_gameState == GameState.ReadyToStart)
        {
            _gameState = GameState.Playing;
            OnGameStarted?.Invoke();
            Playing?.Invoke();
        }
    }

    private void Collided()
    {
        _gameState = GameState.RestartingHold;
        OnGameRestartHold?.Invoke();
    }
    private void HandleGameReset()
    {
        _gameState = GameState.ReadyToStart;
    }

    private void HandlePlaying()
    {
        _gameState = GameState.Playing;
    }
    
    public enum GameState
    {
        ReadyToStart,
        Playing,
        RestartingHold
    }
}
