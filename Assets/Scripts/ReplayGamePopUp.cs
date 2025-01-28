using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayGamePopUp : MonoBehaviour
{
    public void RestartGame()
    {
        if (GameLogic._gameState == GameLogic.GameState.RestartingHold)
        {
            GameLogic._gameState = GameLogic.GameState.Playing;
            GameLogic.OnGameReset?.Invoke();
            GameLogic.Playing?.Invoke();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuctGame()
    {
        Application.Quit();
    }
}
