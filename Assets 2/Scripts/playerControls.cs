using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    public int forwardSpeed;
    public int lineDistance = 5;
    public GameObject player;
    public int gravity=-10;
    public int jumpLength=20;
    private int _currentLine = 1;
    private UnityEngine.Vector3 _targetPosition;
    private CharacterController _controller;
    void Start()
    {
        _controller = player.GetComponent<CharacterController>();
        _targetPosition = player.transform.position;
    }
    void Update()
    {
    _targetPosition += UnityEngine.Vector3.forward * forwardSpeed * Time.fixedDeltaTime;
    if(Input.GetKeyDown(KeyCode.LeftArrow))
    {
        _currentLine--;
        if(_currentLine<0)
        {
            _currentLine=0;
        }
        else
        {
            Debug.Log(_currentLine);
            _targetPosition += UnityEngine.Vector3.left * lineDistance ;
        }
    }
    else if(Input.GetKeyDown(KeyCode.RightArrow))
    {
        _currentLine++;
        if(_currentLine>2)
        {
            _currentLine=2;
        }
        else
        {
            Debug.Log(_currentLine);
            _targetPosition += UnityEngine.Vector3.right * lineDistance;
        }
    }
    if(Input.GetKeyDown(KeyCode.UpArrow))
    {
        Jump();
    }
    player.transform.position = UnityEngine.Vector3.Lerp(player.transform.position, _targetPosition, Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        
    }
    
    private void Jump()
    {
        _targetPosition+= UnityEngine.Vector3.up * jumpLength * Time.fixedDeltaTime;
    }
}
