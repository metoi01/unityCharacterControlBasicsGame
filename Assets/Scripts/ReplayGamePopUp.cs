using UnityEngine;

public class ReplayGamePopUp : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log($"RestartGame called. Current state: {GameLogic._gameState}");
        
        // First reset the game
        GameLogic.OnGameReset?.Invoke();
        Debug.Log("OnGameReset invoked");
        
        // Small delay to ensure everything is reset
        Invoke("StartPlaying", 0.1f);
    }

    private void StartPlaying()
    {
        Debug.Log("StartPlaying called");
        GameLogic._gameState = GameLogic.GameState.Playing;
        GameLogic.Playing?.Invoke();
        Debug.Log($"Game state set to: {GameLogic._gameState}");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
