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

    private void Update()
    {
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            //mAnimator.SetTrigger("trigPlaying");
        }
    }

    private void OnEnable()
    {
        GameLogic.PrepareScene += PrepareScene;
        GameLogic.Playing += Playing;
        GameLogic.OnGameRestartHold += RestartingHold;
        GameLogic.OnGameStarted += Playing;
        CollisionDetection.PowerUpCollected += PowerUpCollected;
    }
    private void OnDisable()
    {
        GameLogic.PrepareScene -= PrepareScene;
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
        mAnimator.SetTrigger("trigPlaying");
    }
    void PrepareScene()
    {
        mAnimator.SetTrigger("trigIdle");
    }

    void PowerUpCollected()
    {
        StartCoroutine(PlayPowerUpAnimation());
    }

    IEnumerator PlayPowerUpAnimation()
    {
        mAnimator.SetTrigger("trigVictory");
        
        // Wait for the victory animation to complete
        // Adjust this time to match your victory animation length
        yield return new WaitForSeconds(1f); 
        
        // Return to running animation if game is still in playing state
        if (GameLogic._gameState == GameLogic.GameState.Playing)
        {
            mAnimator.SetTrigger("trigPlaying");
        }
    }
}
