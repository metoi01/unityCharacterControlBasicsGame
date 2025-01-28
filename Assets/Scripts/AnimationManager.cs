using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _mAnimator;

    void Start()
    {
        _mAnimator = GetComponent<Animator>();
        _mAnimator.SetTrigger("trigIdle");
    }

    private void OnEnable()
    {
        GameLogic.OnGameReset += PrepareScene;
        GameLogic.Playing += Playing;
        GameLogic.OnGameRestartHold += RestartingHold;
        GameLogic.OnGameStarted += Playing;
        CollisionDetection.PowerUpCollected += PowerUpCollected;
    }
    private void OnDisable()
    {
        GameLogic.OnGameReset -= PrepareScene;
        GameLogic.Playing -= Playing;
        GameLogic.OnGameRestartHold -= RestartingHold;
        GameLogic.OnGameStarted -= Playing;
        CollisionDetection.PowerUpCollected -= PowerUpCollected;
    }

    void RestartingHold()
    {
        _mAnimator.SetTrigger("trigStop");
    }
    void Playing()
    {
        _mAnimator.ResetTrigger("trigStop");
        _mAnimator.ResetTrigger("trigIdle");
        _mAnimator.ResetTrigger("trigVictory");
        
        _mAnimator.SetTrigger("trigPlaying");
    }
    void PrepareScene()
    {
        _mAnimator.ResetTrigger("trigStop");
        _mAnimator.ResetTrigger("trigPlaying");
        _mAnimator.ResetTrigger("trigVictory");
        
        _mAnimator.SetTrigger("trigIdle");
    }

    void PowerUpCollected()
    {
        StartCoroutine(PlayPowerUpAnimation());
    }

    IEnumerator PlayPowerUpAnimation()
    {
        _mAnimator.SetTrigger("trigVictory");
        
        yield return new WaitForSeconds(1f); 
        
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            _mAnimator.ResetTrigger("trigVictory");
            _mAnimator.SetTrigger("trigPlaying");
        }
    }
}
