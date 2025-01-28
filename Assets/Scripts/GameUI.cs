using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject startingText;
    public GameObject gameOverPanel;
    
    public Text coinsText;
    public Text coinsCollected;

    private int _coinCount = 0;

    private void OnEnable()
    {
        GameLogic.OnGameRestartHold += TextArrange;
        GameLogic.Playing += TextArrange;
        GameLogic.OnGameStarted += TextArrange;
        CollisionDetection.CollidedCoin += IncrementCoinCount;
    }
    private void OnDisable()
    {
        GameLogic.OnGameRestartHold -= TextArrange;
        GameLogic.OnGameStarted -= TextArrange;
        CollisionDetection.Collided -= TextArrange;
        CollisionDetection.CollidedCoin -= IncrementCoinCount;
    }

    private void IncrementCoinCount(GameObject oth)
    {
        _coinCount++;
        coinsText.text = "Coins: " + _coinCount;
    }

    private void TextArrange()
    {
        if (GameLogic._gameState == GameLogic.GameState.RestartingHold)
        {
            gameOverPanel.SetActive(true);
            coinsText.enabled = false;
            coinsCollected.text = "Coins Collected: " + _coinCount;
            _coinCount = 0;
            coinsText.text = "Coins: " + _coinCount;
            
        }
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            gameOverPanel.SetActive(false);
            startingText.SetActive(false);
            coinsText.enabled = true;
        }
    }
}
