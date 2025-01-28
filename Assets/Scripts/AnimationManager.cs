using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator mAnimator;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mAnimator.SetTrigger("trigIdle");
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
        mAnimator.SetTrigger("trigStop");
    }
    void Playing()
    {
        // Reset any other triggers first to ensure clean transition
        mAnimator.ResetTrigger("trigStop");
        mAnimator.ResetTrigger("trigIdle");
        mAnimator.ResetTrigger("trigVictory");
        
        mAnimator.SetTrigger("trigPlaying");
    }
    void PrepareScene()
    {
        // Reset any other triggers first
        mAnimator.ResetTrigger("trigStop");
        mAnimator.ResetTrigger("trigPlaying");
        mAnimator.ResetTrigger("trigVictory");
        
        mAnimator.SetTrigger("trigIdle");
    }

    void PowerUpCollected()
    {
        StartCoroutine(PlayPowerUpAnimation());
    }

    IEnumerator PlayPowerUpAnimation()
    {
        mAnimator.SetTrigger("trigVictory");
        
        yield return new WaitForSeconds(1f); 
        
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            // Reset victory trigger before setting playing
            mAnimator.ResetTrigger("trigVictory");
            mAnimator.SetTrigger("trigPlaying");
        }
    }
}
