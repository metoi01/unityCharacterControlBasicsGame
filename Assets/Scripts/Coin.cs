using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private List<GameObject> coins = new List<GameObject>();
    private void OnEnable()
    {
        CollisionDetection.CollidedCoin += Collided;
        GameLogic.PrepareScene += ActivateCoins;
    }
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }
    private void OnDisable()
    {
        CollisionDetection.CollidedCoin -= Collided;
        GameLogic.PrepareScene -= ActivateCoins;
    }
    private void Collided(GameObject other)
    {
        if (gameObject == other)
        {
            coins.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void ActivateCoins()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            coins[i].SetActive(true);
        }
    }
}
