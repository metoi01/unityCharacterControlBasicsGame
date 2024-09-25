using System;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public static Action Collided;
    public static Action CollidedCoin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Collided?.Invoke();
        }
        if (other.gameObject.tag == "Coin")
        {
            CollidedCoin?.Invoke();
        }
    }
}
