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
    }
    private void OnDisable()
    {
        GameLogic.PrepareScene -= PrepareScene;
        GameLogic.Playing -= Playing;
        GameLogic.OnGameRestartHold -= RestartingHold;
        GameLogic.OnGameStarted -= Playing;
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
}
