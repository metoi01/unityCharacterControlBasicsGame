using System;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static Action OnGameStarted;
    public static Action OnGameRestartHold;
    public static Action Playing;
    
    public static GameState _gameState;
    private void OnEnable()
    {
        InputManager.OnMouseClicked += GameLogicArranger;
        CollisionDetection.Collided += Collided;
    }
    private void OnDisable()
    {
        InputManager.OnMouseClicked -= GameLogicArranger;
        CollisionDetection.Collided -= Collided;
    }
    
    private void GameLogicArranger()
    {
        if (_gameState == GameState.ReadyToStart)
        {
            _gameState = GameState.Playing;
            OnGameStarted?.Invoke();
        }
        else if (_gameState == GameState.RestartingHold)
        {
            _gameState = GameState.Playing;
            Playing?.Invoke();
        }
    }

    private void Collided()
    {
        _gameState = GameState.RestartingHold;
        OnGameRestartHold?.Invoke();
    }
    
    public enum GameState
    {
        ReadyToStart,
        Playing,
        RestartingHold
    }
}
