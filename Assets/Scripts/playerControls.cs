using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    public int forwardSpeed;
    public int laneDistance = 5;
    public GameObject player;
    private int _currentLane = 1;
    private UnityEngine.Vector3 _targetPosition;
    
    void Start()
    {
        _targetPosition = player.transform.position;
    }
    void Update()
    {
    _targetPosition += UnityEngine.Vector3.forward * forwardSpeed * Time.fixedDeltaTime;
    if(Input.GetKeyDown(KeyCode.LeftArrow))
    {
        _currentLane--;
        if(_currentLane<0)
        {
            _currentLane=0;
        }
        else
        {
            Debug.Log(_currentLane);
            _targetPosition += UnityEngine.Vector3.left * laneDistance ;
        }
    }
    else if(Input.GetKeyDown(KeyCode.RightArrow))
    {
        _currentLane++;
        if(_currentLane>2)
        {
            _currentLane=2;
        }
        else
        {
            Debug.Log(_currentLane);
            _targetPosition += UnityEngine.Vector3.right * laneDistance;
        }
    }
    player.transform.position = _targetPosition;
    _targetPosition = UnityEngine.Vector3.zero;
    }

    private void FixedUpdate()
    {
        
    }
}
