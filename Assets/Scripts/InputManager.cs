using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Action OnMouseClicked;
    public static Action<int> ArrowClicked;
    public static Action OnJumpTriggered;

    private int _arrow;

    void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            HandleGameplayInputs();
        }
        else if (GameLogic._gameState == GameLogic.GameState.ReadyToStart || 
                GameLogic._gameState == GameLogic.GameState.RestartingHold)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClicked?.Invoke();
            }
        }
    }

    private void HandleGameplayInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _arrow = -1;
            ArrowClicked?.Invoke(_arrow);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _arrow = 1;
            ArrowClicked?.Invoke(_arrow);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnJumpTriggered?.Invoke();
        }
    }
}
