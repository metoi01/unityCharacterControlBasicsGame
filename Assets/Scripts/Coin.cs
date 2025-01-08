using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject ground;
    private Renderer _coinRenderer;
    private Collider _coinCollider;
    private void OnEnable()
    {
        CollisionDetection.CollidedCoin += Collided;
        GameLogic.PrepareScene += ActivateCoin;
        TileManagerObjectPooling.TileArranged += ActivateCoinGround;
    }

    void Start()
    {
        _coinRenderer = GetComponent<Renderer>();
        _coinCollider = GetComponent<Collider>();
    }
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }
    private void OnDisable()
    {
        CollisionDetection.CollidedCoin -= Collided;
        GameLogic.PrepareScene -= ActivateCoin;
        TileManagerObjectPooling.TileArranged -= ActivateCoinGround;
    }
    private void Collided(GameObject other)
    {
        if (gameObject == other)
        {
            HideCoin();
        }
    }
    
    private void HideCoin()
    {
        _coinRenderer.enabled = false;
        _coinCollider.enabled = false;
    }

    private void ActivateCoin()
    {
        _coinRenderer.enabled = true;
        _coinCollider.enabled = true;
    }

    private void ActivateCoinGround(GameObject groundPassed)
    {
        StartCoroutine(ActivateCoinGroundWithDelay(groundPassed));
    }
    
    private IEnumerator ActivateCoinGroundWithDelay(GameObject groundPassed)
    {
        yield return new WaitForSeconds(3);

        if (ground.name == groundPassed.name)
        {
            _coinRenderer.enabled = true;
            _coinCollider.enabled = true; 
        }
    }
}
