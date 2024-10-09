using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_X_POSITION = -4;
    private const int MAX_LANES = 2;
    
    public GameObject player;

    public float forwardSpeed = 10;
    public float maxSpeed;
    public float sidewaysSpeed = 4;
    public float laneDistance = 4;
    
    private int _currentLaneIndex = 1;

    private float _initialSpeed;
    private float _velocity = 10f;
    private void OnEnable()
    {
        InputManager.ArrowClicked += LaneChange;
        GameLogic.OnGameStarted += Playing;
        GameLogic.Playing += Playing;
    }

    private void OnDisable()
    {
        InputManager.ArrowClicked -= LaneChange;
        GameLogic.OnGameStarted -= Playing;
        GameLogic.Playing -= Playing;
    }

    private void Start()
    {
        _initialSpeed = forwardSpeed;
    }

    private void LaneChange(int direction)
    {
        if (direction == -1)
        {
            _currentLaneIndex--;
        }

        if (direction == 1)
        {
            _currentLaneIndex++;
        }
        _currentLaneIndex = Mathf.Clamp(_currentLaneIndex, 0, MAX_LANES);
    }

    void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            MoveForward();
            MoveSideWays();
            IncreaseSpeed();
        }
    }

    private void MoveForward()
    {
        player.transform.position += Vector3.forward * (forwardSpeed * Time.deltaTime);
    }

    private void MoveSideWays()
    {
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
    private void Playing()
    {
        _currentLaneIndex = 1;
        forwardSpeed = _initialSpeed;
        player.transform.position = new Vector3(0, 1, 4);
    }
}
