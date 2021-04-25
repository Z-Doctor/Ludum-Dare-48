using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

public class PlayerController : CharacterController2D
{
    public SpriteAnimation playerAnimation;
    public SpriteAnimation attackAnimation;

    public AttackTrigger attackTrigger;

    public void AddMonsterShard(int amount)
    {
        
    }

    bool attacking;
    List<CharacterController2D> attackedTargets = new List<CharacterController2D>();

    public void Move(CallbackContext context)
    {
        Move(context.ReadValue<float>());
        if(IsMoving)
            playerAnimation.Play("Walk");
        else
            playerAnimation.Play("Idle");
    }

    public void StartAttack(CallbackContext context)
    {
        if(attacking)
            return;
        if(context.phase == InputActionPhase.Started)
        {
            attacking = true;
            attackedTargets.Clear();
            attackAnimation.Play("Attack");
        }
    }

    protected override void OnDied()
    {
        events.onDied.Invoke();
        gameObject.SetActive(false);
    }

    public void EndAttack()
    {
        if(!attacking)
            return;
        attacking = false;
        attackedTargets.Clear();
    }

    protected override void Update()
    {
        base.Update();

        if(!isAlive)
            return;
        if(attacking)
        {
            var targets = attackTrigger.targets.Keys.Where(target => !attackedTargets.Contains(target)).ToArray();
            Attack(targets);
            attackedTargets.AddRange(targets);
        }
    }

    public void Jump(CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
            Jump();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        StartCoroutine(Validate());

        IEnumerator Validate()
        {
            yield return null;
            events.onHealthChanged.Invoke();
        }
    }

}
