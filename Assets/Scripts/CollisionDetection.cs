using System;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public static Action Collided;
    public static Action<GameObject> CollidedCoin;
    public static Action<GameObject> CollidedInvinciblePowerUp;
    public static Action<GameObject> CollidedCoinPowerUp;
    public static Action<GameObject> CollidedJumpPowerUp;
    public static Action PowerUpCollected;
    public static Func<bool> CheckInvincible;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (CheckInvincible == null || !CheckInvincible.Invoke())
            {
                GameLogic._gameState = GameLogic.GameState.RestartingHold;
                Collided?.Invoke();
                FindObjectOfType<AudioManager>().PlaySound("GameOver");
            }
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            CollidedCoin?.Invoke(other.gameObject);
            FindObjectOfType<AudioManager>().PlaySound("PickUpCoin");
        }
        else if (other.gameObject.CompareTag("InvinciblePowerUp"))
        {
            CollidedInvinciblePowerUp?.Invoke(other.gameObject);
            PowerUpCollected?.Invoke();
            FindObjectOfType<AudioManager>().PlaySound("PowerUp");
        }
        else if (other.gameObject.CompareTag("CoinPowerUp"))
        {
            CollidedCoinPowerUp?.Invoke(other.gameObject);
            PowerUpCollected?.Invoke();
            FindObjectOfType<AudioManager>().PlaySound("PowerUp");
        }
        else if (other.gameObject.CompareTag("JumpPowerUp"))
        {
            CollidedJumpPowerUp?.Invoke(other.gameObject);
            PowerUpCollected?.Invoke();
            FindObjectOfType<AudioManager>().PlaySound("PowerUp");
        }
    }
}
