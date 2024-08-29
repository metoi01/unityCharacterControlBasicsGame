using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 2;
    public float maxSpeed;
    public float sidewaysSpeed = 4;
    public float laneDistance = 4;
    public GameObject player;
    private int _currentLaneIndex = 1;
    private const int MAX_LANES = 2;
    private const float MIN_X_POSITION = -4;
    private float _velocity = 10f;

    void Start()
    {
    }
    void Update()
    {
        MoveForward();
        MoveSideways();
        increaseSpeed();
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

    private void increaseSpeed()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
    }
}
