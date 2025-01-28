using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public GameObject startingText;
    public GameObject gameOverPanel;
    
    public Text coinsText;
    public Text invinciblePowerUp;
    public Text coinsDoubledPowerUp;
    public Text tripleJumpPowerUp;
    public Text coinsCollected;

    private int _coinCount = 0;

    private Coroutine _invincibleTimerCoroutine;
    private Coroutine _coinsDoubledTimerCoroutine;
    private Coroutine _tripleJumpTimerCoroutine;

    private PowerUpsManager _powerUpsManager;

    private void OnEnable()
    {
        GameLogic.OnGameRestartHold += TextArrange;
        GameLogic.Playing += TextArrange;
        GameLogic.OnGameStarted += TextArrange;
        CollisionDetection.CollidedCoin += IncrementCoinCount;
        
        PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpCollected += StartInvincibleTimer;
        PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpEnded += HideInvincibleText;
        
        PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpCollected += StartCoinsDoubledTimer;
        PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpEnded += HideCoinsDoubledText;
        
        PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpCollected += StartTripleJumpTimer;
        PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpEnded += HideTripleJumpText;
    }

    private void OnDisable()
    {
        GameLogic.OnGameRestartHold -= TextArrange;
        GameLogic.OnGameStarted -= TextArrange;
        CollisionDetection.Collided -= TextArrange;
        CollisionDetection.CollidedCoin -= IncrementCoinCount;
        
        PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpCollected -= StartInvincibleTimer;
        PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpEnded -= HideInvincibleText;
        
        PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpCollected -= StartCoinsDoubledTimer;
        PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpEnded -= HideCoinsDoubledText;
        
        PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpCollected -= StartTripleJumpTimer;
        PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpEnded -= HideTripleJumpText;
    }

    private void IncrementCoinCount(GameObject oth)
    {
        // Coin doubled powerup aktifse 2 coin ekle
        int coinIncrement = _powerUpsManager.IsCoinDoubled() ? 2 : 1;
        _coinCount += coinIncrement;
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
            
            HideAllPowerUpTexts();
        }
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            gameOverPanel.SetActive(false);
            startingText.SetActive(false);
            coinsText.enabled = true;
        }
    }

    private void Start()
    {
        HideAllPowerUpTexts();
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    private void HideAllPowerUpTexts()
    {
        invinciblePowerUp.gameObject.SetActive(false);
        coinsDoubledPowerUp.gameObject.SetActive(false);
        tripleJumpPowerUp.gameObject.SetActive(false);
    }

    private void StartInvincibleTimer(float duration)
    {
        if (_invincibleTimerCoroutine != null)
            StopCoroutine(_invincibleTimerCoroutine);
        _invincibleTimerCoroutine = StartCoroutine(UpdatePowerUpTimer(invinciblePowerUp, "Invincible PowerUp: ", duration));
    }

    private void StartCoinsDoubledTimer(float duration)
    {
        if (_coinsDoubledTimerCoroutine != null)
            StopCoroutine(_coinsDoubledTimerCoroutine);
        _coinsDoubledTimerCoroutine = StartCoroutine(UpdatePowerUpTimer(coinsDoubledPowerUp, "Coins Doubled: ", duration));
    }

    private void StartTripleJumpTimer(float duration)
    {
        if (_tripleJumpTimerCoroutine != null)
            StopCoroutine(_tripleJumpTimerCoroutine);
        _tripleJumpTimerCoroutine = StartCoroutine(UpdatePowerUpTimer(tripleJumpPowerUp, "Triple Jump: ", duration));
    }

    private IEnumerator UpdatePowerUpTimer(Text powerUpText, string prefix, float duration)
    {
        powerUpText.gameObject.SetActive(true);
        float remainingTime = duration;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            remainingTime = duration - (Time.time - startTime);
            powerUpText.text = $"{prefix}{Mathf.Max(0, remainingTime):F1}s";
            yield return null;
        }

        powerUpText.gameObject.SetActive(false);
    }

    private void HideInvincibleText() => invinciblePowerUp.gameObject.SetActive(false);
    private void HideCoinsDoubledText() => coinsDoubledPowerUp.gameObject.SetActive(false);
    private void HideTripleJumpText() => tripleJumpPowerUp.gameObject.SetActive(false);
}
