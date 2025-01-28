using System.Collections;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    private PlayerController _playerController;
    private bool _isInvincible;
    private bool _isCoinDoubled;
    private bool _isJumpBoosted;
    
    // Store coroutines to be able to stop them
    private Coroutine _invincibleCoroutine;
    private Coroutine _coinCoroutine;
    private Coroutine _jumpCoroutine;
    
    private const float BASE_JUMP_FORCE = 6f;  // Same value as in PlayerController.Jump()
    
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
        if (_isInvincible)
        {
            // If already active, stop current coroutine and start a new one
            if (_invincibleCoroutine != null)
            {
                StopCoroutine(_invincibleCoroutine);
            }
        }
        _invincibleCoroutine = StartCoroutine(InvinciblePowerUpRoutine());
    }

    public void ActivateCoinPowerUp(GameObject powerUpObject)
    {
        DisablePowerUpVisibility(powerUpObject);
        if (_isCoinDoubled)
        {
            if (_coinCoroutine != null)
            {
                StopCoroutine(_coinCoroutine);
            }
        }
        _coinCoroutine = StartCoroutine(CoinPowerUpRoutine());
    }

    public void ActivateJumpPowerUp(GameObject powerUpObject)
    {
        DisablePowerUpVisibility(powerUpObject);
        if (_isJumpBoosted)
        {
            if (_jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
            }
        }
        _jumpCoroutine = StartCoroutine(JumpPowerUpRoutine());
    }

    private IEnumerator InvinciblePowerUpRoutine()
    {
        _isInvincible = true;
        
        yield return new WaitForSeconds(3f);
        
        _isInvincible = false;
        _invincibleCoroutine = null;
    }

    private IEnumerator CoinPowerUpRoutine()
    {
        _isCoinDoubled = true;
        
        yield return new WaitForSeconds(8f);
        
        _isCoinDoubled = false;
        _coinCoroutine = null;
    }

    private IEnumerator JumpPowerUpRoutine()
    {
        _isJumpBoosted = true;
        
        // Set jump force multiplier
        _playerController.SetJumpForceMultiplier(2f);
        
        yield return new WaitForSeconds(6f);
        
        // Reset jump force multiplier
        _playerController.SetJumpForceMultiplier(1f);
        _isJumpBoosted = false;
        _jumpCoroutine = null;
    }

    private void DisablePowerUpVisibility(GameObject powerUpObject)
    {
        var renderer = powerUpObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
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