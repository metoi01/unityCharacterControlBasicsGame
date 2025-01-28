using UnityEngine;

public class ReplayGamePopUp : MonoBehaviour
{
    public void RestartGame()
    {
        Invoke("ResetAndStartGame", 0.1f);
    }

    private void ResetAndStartGame()
    {
        GameLogic.OnGameReset?.Invoke();
        Invoke("StartPlaying", 0.1f);
    }

    private void StartPlaying()
    {
        GameLogic._gameState = GameLogic.GameState.Playing;
        GameLogic.Playing?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
