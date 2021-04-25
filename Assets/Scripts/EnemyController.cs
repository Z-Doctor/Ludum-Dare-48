using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterController2D
{
    public SpriteAnimation enemyAnimation;
    [Min(0)] public float attackCooldown = 0.5f;

    public bool AttackReady => !attacking && attackCooldownTimer <= 0;
    protected float attackCooldownTimer;

    protected bool attacking;

    protected override void Update()
    {
        base.Update();

        if(!isAlive)
            return;

        if(attackCooldownTimer > 0)
            attackCooldownTimer = Mathf.Max(attackCooldownTimer - Time.deltaTime, 0);

    }

    public override void Move(float direction)
    {
        base.Move(direction);
        if(IsMoving)
            enemyAnimation.Play("Walk");
        else
            enemyAnimation.Play("Idle");
    }

    protected virtual void Attack(PlayerController playerTarget)
    {
        playerTarget.Damage(attackDamage);
        attackCooldownTimer = attackCooldown;
    }
}
