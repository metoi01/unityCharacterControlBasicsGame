using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject startingText;
    public Text coinsText;

    private int _coinCount;

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
            startingText.SetActive(true);
            _coinCount = 0;
            coinsText.text = "Coins: " + _coinCount;
            
        }
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            startingText.SetActive(false);
        }
    }
}
