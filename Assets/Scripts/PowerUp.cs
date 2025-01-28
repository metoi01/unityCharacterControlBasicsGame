using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject ground;
    private Renderer _powerUpRenderer;
    private Collider _powerUpCollider;

    public PowerUpType powerUpType;
    public float duration = 3f; // Ensure this is set to 3f for invincible power-up

    private void OnEnable()
    {
        GameLogic.OnGameReset += ActivatePowerUp;
        TileManagerObjectPooling.TileArranged += ActivatePowerUpGround;
        
        // Her powerup tipi için sadece kendi event'ine abone ol
        switch (powerUpType)
        {
            case PowerUpType.Invincible:
                CollisionDetection.CollidedInvinciblePowerUp += CheckCollision;
                break;
            case PowerUpType.CoinsDoubled:
                CollisionDetection.CollidedCoinPowerUp += CheckCollision;
                break;
            case PowerUpType.TripleJump:
                CollisionDetection.CollidedJumpPowerUp += CheckCollision;
                break;
        }
    }

    void Start()
    {
        _powerUpRenderer = GetComponent<Renderer>();
        _powerUpCollider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        GameLogic.OnGameReset -= ActivatePowerUp;
        TileManagerObjectPooling.TileArranged -= ActivatePowerUpGround;
        
        // Her powerup tipi için sadece kendi event'inden çık
        switch (powerUpType)
        {
            case PowerUpType.Invincible:
                CollisionDetection.CollidedInvinciblePowerUp -= CheckCollision;
                break;
            case PowerUpType.CoinsDoubled:
                CollisionDetection.CollidedCoinPowerUp -= CheckCollision;
                break;
            case PowerUpType.TripleJump:
                CollisionDetection.CollidedJumpPowerUp -= CheckCollision;
                break;
        }
    }

    private void CheckCollision(GameObject other)
    {
        if (gameObject != other) return;
        
        HidePowerUp();
        StartCoroutine(ActivatePowerUpEffect());
    }

    private void HidePowerUp()
    {
        _powerUpRenderer.enabled = false;
        _powerUpCollider.enabled = false;
    }

    private void ActivatePowerUp()
    {
        _powerUpRenderer.enabled = true;
        _powerUpCollider.enabled = true;
    }

    private void ActivatePowerUpGround(GameObject groundPassed)
    {
        StartCoroutine(ActivatePowerUpGroundWithDelay(groundPassed));
    }

    private IEnumerator ActivatePowerUpGroundWithDelay(GameObject groundPassed)
    {
        yield return new WaitForSeconds(3);

        if (ground.name == groundPassed.name)
        {
            _powerUpRenderer.enabled = true;
            _powerUpCollider.enabled = true;
        }
    }

    private IEnumerator ActivatePowerUpEffect()
    {
        // PowerUp efektini başlat
        switch (powerUpType)
        {
            case PowerUpType.Invincible:
                duration = 3f;
                PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpCollected?.Invoke(duration);
                break;
            case PowerUpType.CoinsDoubled:
                duration = 6f;
                PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpCollected?.Invoke(duration);
                break;
            case PowerUpType.TripleJump:
                duration = 4f;
                PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpCollected?.Invoke(duration);
                break;
        }

        yield return new WaitForSeconds(duration);

        // PowerUp efektini bitir
        switch (powerUpType)
        {
            case PowerUpType.Invincible:
                PowerUpsManager.PowerUpEvents.OnInvinciblePowerUpEnded?.Invoke();
                break;
            case PowerUpType.CoinsDoubled:
                PowerUpsManager.PowerUpEvents.OnCoinsDoubledPowerUpEnded?.Invoke();
                break;
            case PowerUpType.TripleJump:
                PowerUpsManager.PowerUpEvents.OnTripleJumpPowerUpEnded?.Invoke();
                break;
        }
    }

    public enum PowerUpType
    {
        Invincible,
        CoinsDoubled,
        TripleJump
    }
} 