using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_X_POSITION = -4;
    private const int MAX_LANES = 2;
    
    public GameObject player;

    public float forwardSpeed = 10;
    public float maxSpeed;
    public float sidewaysSpeed = 2f;
    public float laneDistance = 4;
    
    private int _currentLaneIndex = 1;

    private float _initialSpeed;
    private float _velocity = 0f;
    public float _verticalVelocity;
    private bool _isJumping;
    private CharacterController _characterController;

    private const float BASE_JUMP_FORCE = 6f;
    private float _jumpForceMultiplier = 1f;

    private void OnEnable()
    {
        InputManager.ArrowClicked += LaneChange;
        InputManager.OnJumpTriggered += Jump;
        GameLogic.OnGameStarted += Playing;
        GameLogic.Playing += Playing;
        GameLogic.OnGameReset += PrepareScene;
    }

    private void OnDisable()
    {
        InputManager.ArrowClicked -= LaneChange;
        InputManager.OnJumpTriggered -= Jump;
        GameLogic.OnGameStarted -= Playing;
        GameLogic.Playing -= Playing;
        GameLogic.OnGameReset -= PrepareScene;
    }

    private void Start()
    {
        _initialSpeed = forwardSpeed;
        _characterController = player.GetComponent<CharacterController>();
        if (_characterController == null)
        {
            _characterController = player.AddComponent<CharacterController>();
        }
        
        _characterController.radius = 0.5f;
        _characterController.height = 2f;
        _characterController.center = new Vector3(0, 1f, 0);
        _characterController.slopeLimit = 45f;
        _characterController.stepOffset = 0.3f;
    }

    private void LaneChange(int direction)
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
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
    }

    private void Jump()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing && !_isJumping)
        {
            _verticalVelocity = BASE_JUMP_FORCE * _jumpForceMultiplier;
            _isJumping = true;
        }
    }

    void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            Vector3 moveVector = Vector3.zero;
            
            moveVector += Vector3.forward * (forwardSpeed * Time.deltaTime);
            
            float targetposition = _currentLaneIndex * laneDistance + MIN_X_POSITION;
            float xPosition = Mathf.SmoothDamp(
                player.transform.position.x, 
                targetposition, 
                ref _velocity, 
                0.15f 
            );
            moveVector.x = xPosition - player.transform.position.x;
            
            if (!_characterController.isGrounded)
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity = -2f;
                _isJumping = false;
            }
            
            moveVector.y = _verticalVelocity * Time.deltaTime;
            
            const float MIN_HEIGHT = 1f; 
            if (player.transform.position.y + moveVector.y <= MIN_HEIGHT)
            {
                moveVector.y = MIN_HEIGHT - player.transform.position.y;
                _verticalVelocity = 0;
                _isJumping = false;
                player.transform.position = new Vector3(
                    player.transform.position.x,
                    MIN_HEIGHT,
                    player.transform.position.z
                );
            }
            
            _characterController.Move(moveVector);
            
            IncreaseSpeed();
        }
    }

    private void IncreaseSpeed()
    {
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime;
        }
    }

    private void PrepareScene()
    {
        _currentLaneIndex = 1;
        _verticalVelocity = 0;
        _isJumping = false;
        _characterController.enabled = false;
        player.transform.position = new Vector3(0, 1, 4);
        _characterController.enabled = true;
    }
    private void Playing()
    {
        forwardSpeed = _initialSpeed;
    }

    public void SetJumpForceMultiplier(float multiplier)
    {
        _jumpForceMultiplier = multiplier;
    }
}
