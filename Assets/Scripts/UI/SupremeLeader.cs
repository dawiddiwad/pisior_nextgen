using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupremeLeader : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        PlayerController.OnHitEnemyRocket += EnemyHit;
        GameController.OnGameEnd += GameOver;
    }

    private void OnDisable()
    {
        PlayerController.OnHitEnemyRocket -= EnemyHit;
        GameController.OnGameEnd -= GameOver;
    }

    private void EnemyHit(Vector3 collisionPosition)
    {
        if (GameController.playerController.getHealthPrcnt() != 1)
        {
            animator.SetTrigger("EnemyHit");
        }
    }
    private void GameOver(GameController.GameResult gameResult)
    {
        if (gameResult == GameController.GameResult.Lost)
        {
            animator.SetTrigger("GameOver");
        } else if (gameResult == GameController.GameResult.Won)
        {
            animator.SetTrigger("Win");
        }
    }
}
