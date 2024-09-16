using UnityEngine;

public class Coin : MonoBehaviour
{
    private float _pointZCollided = 0;
    void Update()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
        Debug.Log(PlayerController.playerZCoord + "+ " + _pointZCollided);
        if (PlayerController.playerZCoord > _pointZCollided + 20)
        {
            gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _pointZCollided = other.transform.position.z;
            PlayerController.coinCount += 1;
            gameObject.SetActive(false);
        }

    }

}
