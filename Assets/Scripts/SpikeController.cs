using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpikeController : EnemyController
{

    [Min(0)] public float minMoveTime = 1;
    [Min(0)] public float maxMoveTime = 3;
    [Min(0)] public float minJumpTime = 1;
    [Min(0)] public float maxJumpTime = 3;
    [Min(0)] public float attackCooldownPenalty = 0.25f;

    public SpriteAnimation attackAnimation;

    int direction;

    float moveTimer;
    float jumpTimer;

    PlayerController playerTarget;

    public void PlayerInRange(PlayerController player)
    {
        playerTarget = player;
    }

    public void PlayerOutOfRange(PlayerController player)
    {
        if(player == playerTarget)
            playerTarget = null;
    }

    protected override void OnDamaged(float damage)
    {
        base.OnDamaged(damage);
        if(!attacking)
            attackCooldownTimer += attackCooldownPenalty;
    }

    public void StartAttack()
    {
        if(attacking)
            return;

        attacking = true;
        attackAnimation.Play("Attack");
    }

    public void Attack()
    {
        if(!attacking || !playerTarget)
            return;
        Attack(playerTarget);
    }

    public void EndAttack()
    {
        attacking = false;
    }

    protected override void Update()
    {
        base.Update();

        if(!isAlive)
            return;

        if(AttackReady && playerTarget && !playerTarget.IsInvulnerable)
        {
            StartAttack();
        }

        if(moveTimer <= 0)
        {
            // Need to move
            direction = Random.Range(-1, 2);

            moveTimer = Random.Range(minMoveTime, maxMoveTime);
            Move(speed * direction);
        }
        else
        {
            // Moving
            moveTimer = Mathf.Max(0, moveTimer - Time.deltaTime);
        }

        if(jumpTimer <= 0)
        {
            Jump();
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        }
        else
        {
            // Moving
            jumpTimer = Mathf.Max(0, jumpTimer - Time.deltaTime);
        }
    }

}
