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
    public Text scoreText;
    public Text coinsCollected;
    public Text scoreCollected;

    private int _coinCount = 0;
    private float _score = 0f;
    private PlayerController _playerController;

    private Coroutine _invincibleTimerCoroutine;
    private Coroutine _coinsDoubledTimerCoroutine;
    private Coroutine _tripleJumpTimerCoroutine;

    private PowerUpsManager _powerUpsManager;

    private bool _shouldResetValues = false;
    private int _lastCoinCount = 0;
    private float _lastScore = 0f;

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

        GameLogic.OnGameReset += ResetUI;
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

        GameLogic.OnGameReset -= ResetUI;
    }

    private void IncrementCoinCount(GameObject oth)
    {
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
            scoreText.enabled = false;
            
            _lastCoinCount = _coinCount;
            _lastScore = _score;
            
            coinsCollected.text = "Coins Collected: " + _lastCoinCount;
            scoreCollected.text = "Final Score: " + Mathf.FloorToInt(_lastScore);
            
            _shouldResetValues = true;
            
            HideAllPowerUpTexts();
        }
        else if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            if (_shouldResetValues)
            {
                _coinCount = 0;
                _lastCoinCount = 0;
                _score = 0;
                _lastScore = 0;
                coinsText.text = "Coins: " + _coinCount;
                UpdateScoreText();
                
                coinsCollected.text = "Coins Collected: 0";
                scoreCollected.text = "Final Score: 0";
                
                _shouldResetValues = false;
            }
            
            gameOverPanel.SetActive(false);
            startingText.SetActive(false);
            coinsText.enabled = true;
            scoreText.enabled = true;
        }
    }

    private void Start()
    {
        HideAllPowerUpTexts();
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
        _playerController = FindObjectOfType<PlayerController>();
        UpdateScoreText();
    }

    private void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            float speedMultiplier = _playerController.forwardSpeed / _playerController._initialSpeed;
            _score += Time.deltaTime * 10 * speedMultiplier;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(_score);
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

    private void ResetUI()
    {
        _shouldResetValues = false;
        _coinCount = 0;
        _score = 0f;
        coinsText.text = "Coins: " + _coinCount;
        scoreText.text = "Score: " + Mathf.FloorToInt(_score);
        coinsCollected.text = "Coins Collected: " + _lastCoinCount;
        scoreCollected.text = "Final Score: " + Mathf.FloorToInt(_lastScore);
    }
}
