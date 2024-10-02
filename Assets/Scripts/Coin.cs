using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnEnable()
    {
        CollisionDetection.CollidedCoin += Collided;
    }
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }
    private void OnDisable()
    {
        CollisionDetection.CollidedCoin -= Collided;
    }
    private void Collided()
    {
        if(this == CollisionDetection.CollidedCoin)
        gameObject.SetActive(false);
    }
}
