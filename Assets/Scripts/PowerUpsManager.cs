using System;
using System.Collections;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    // PowerUpEvents'i buraya taşıyoruz
    public static class PowerUpEvents
    {
        public static Action<float> OnInvinciblePowerUpCollected;
        public static Action OnInvinciblePowerUpEnded;
        
        public static Action<float> OnCoinsDoubledPowerUpCollected;
        public static Action OnCoinsDoubledPowerUpEnded;
        
        public static Action<float> OnTripleJumpPowerUpCollected;
        public static Action OnTripleJumpPowerUpEnded;
    }

    private PlayerController _playerController;
    private bool _isInvincible;
    private bool _isCoinDoubled;
    private bool _isJumpBoosted;
    
    private Coroutine _invincibleCoroutine;
    private Coroutine _coinCoroutine;
    private Coroutine _jumpCoroutine;
    
    private const float BASE_JUMP_FORCE = 6f;  
    
    private void OnEnable()
    {
        CollisionDetection.CollidedInvinciblePowerUp += ActivateInvinciblePowerUp;
        CollisionDetection.CollidedCoinPowerUp += ActivateCoinPowerUp;
        CollisionDetection.CollidedJumpPowerUp += ActivateJumpPowerUp;
        CollisionDetection.CheckInvincible += IsInvincible;
    }

    private void OnDisable()
    {
        CollisionDetection.CollidedInvinciblePowerUp -= ActivateInvinciblePowerUp;
        CollisionDetection.CollidedCoinPowerUp -= ActivateCoinPowerUp;
        CollisionDetection.CollidedJumpPowerUp -= ActivateJumpPowerUp;
        CollisionDetection.CheckInvincible -= IsInvincible;
    }
    
    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void ActivateInvinciblePowerUp(GameObject powerUpObject)
    {
        DisablePowerUpVisibility(powerUpObject);
        _invincibleCoroutine = StartCoroutine(InvinciblePowerUpRoutine());
    }

    public void ActivateCoinPowerUp(GameObject powerUpObject)
    {
        DisablePowerUpVisibility(powerUpObject);
        _coinCoroutine = StartCoroutine(CoinPowerUpRoutine());
    }

    public void ActivateJumpPowerUp(GameObject powerUpObject)
    {
        DisablePowerUpVisibility(powerUpObject);
        _jumpCoroutine = StartCoroutine(JumpPowerUpRoutine());
    }

    private IEnumerator InvinciblePowerUpRoutine()
    {
        if (_isInvincible && _invincibleCoroutine != null)
        {
            StopCoroutine(_invincibleCoroutine);
        }
        
        float duration = 3f;
        
        Debug.Log($"Invincible PowerUp Duration: {duration}");
        
        _isInvincible = true;
        PowerUpEvents.OnInvinciblePowerUpCollected?.Invoke(duration);
        
        yield return new WaitForSeconds(duration);
        
        _isInvincible = false;
        _invincibleCoroutine = null;
        PowerUpEvents.OnInvinciblePowerUpEnded?.Invoke();
    }

    private IEnumerator CoinPowerUpRoutine()
    {
        if (_isCoinDoubled && _coinCoroutine != null)
        {
            StopCoroutine(_coinCoroutine);
        }
        
        float duration = 6f;
        _isCoinDoubled = true;
        PowerUpEvents.OnCoinsDoubledPowerUpCollected?.Invoke(duration);
        
        yield return new WaitForSeconds(duration);
        
        _isCoinDoubled = false;
        _coinCoroutine = null;
        PowerUpEvents.OnCoinsDoubledPowerUpEnded?.Invoke();
    }

    private IEnumerator JumpPowerUpRoutine()
    {
        if (_isJumpBoosted && _jumpCoroutine != null)
        {
            StopCoroutine(_jumpCoroutine);
        }
        
        float duration = 4f;
        _isJumpBoosted = true;
        _playerController.SetJumpForceMultiplier(2f);
        PowerUpEvents.OnTripleJumpPowerUpCollected?.Invoke(duration);
        
        yield return new WaitForSeconds(duration);
        
        _playerController.SetJumpForceMultiplier(1f);
        _isJumpBoosted = false;
        _jumpCoroutine = null;
        PowerUpEvents.OnTripleJumpPowerUpEnded?.Invoke();
    }

    private void DisablePowerUpVisibility(GameObject powerUpObject)
    {
        var renderer = powerUpObject.GetComponent<Renderer>();
        var collider = powerUpObject.GetComponent<Collider>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public static void EnablePowerUp(GameObject powerUpObject)
    {
        var renderer = powerUpObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }

    public bool IsInvincible()
    {
        return _isInvincible;
    }

    public bool IsCoinDoubled()
    {
        return _isCoinDoubled;
    }
} 