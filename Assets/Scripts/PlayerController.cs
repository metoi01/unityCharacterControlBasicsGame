using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject startingText;
    public float forwardSpeed = 2;
    public float maxSpeed;
    public float sidewaysSpeed = 4;
    public float laneDistance = 4;
    private int _currentLaneIndex = 1;
    private const int MAX_LANES = 2;
    private const float MIN_X_POSITION = -4;
    private float _velocity = 10f;
    private bool _isGameStarted = false;

    void Update()
    {
        if (_isGameStarted)
        {
            MoveForward();
            MoveSideways();
            IncreaseSpeed();
        }
        else
        {
            StartGame();
        }
    }

    private void MoveForward()
    {
        player.transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    }

    private void MoveSideways()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentLaneIndex > 0)
        {
            _currentLaneIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _currentLaneIndex < MAX_LANES)
        {
            _currentLaneIndex++;
        }
        float targetposition = _currentLaneIndex * laneDistance + MIN_X_POSITION;
        float xPosition = Mathf.SmoothDamp(player.transform.position.x, targetposition, ref _velocity, Time.deltaTime * sidewaysSpeed);
        player.transform.position = new Vector3(xPosition, player.transform.position.y, player.transform.position.z);
    }

    private void IncreaseSpeed()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
    }
    private void StartGame()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isGameStarted = true;
            startingText.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            RestartGame();
        }
    }
    private void RestartGame()
    {
        _currentLaneIndex = 1;
        _isGameStarted = false;
        player.transform.position = new Vector3(0, 1, 4);
        startingText.SetActive(true);
    }
}
