using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealthPickup : MonoBehaviour
{
    [Min(0.5f)] public float healAmount = 1;
    [HideInInspector] [SerializeField] float lastAmount = 1;
    [HideInInspector] [SerializeField] SpriteRenderer spriteRenderer;

    public Sprite fullHeart;
    public Sprite halfHeart;

    public void Heal(CharacterController2D character)
    {
        if(!character.IsHurt)
            return;

        character.Heal(healAmount);
        Destroy(gameObject);
    }

    void OnValidate()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(healAmount < 1)
            spriteRenderer.sprite = halfHeart;
        else
            spriteRenderer.sprite = fullHeart;

        float dif = healAmount - lastAmount;
        float div = healAmount - Mathf.Floor(healAmount);
        float rem = healAmount % 0.5f;
        healAmount = Mathf.Max(0.5f, healAmount - rem);

        if(dif > 0)
        {
            if(div > 0.5f)
                healAmount = Mathf.Ceil(healAmount);
            else if(div < 0.5f)
                healAmount += 0.5f;
        }
        else if(dif < 0)
        {
            healAmount = Mathf.Floor(healAmount);
            if(div > 0.5f)
                healAmount += 0.5f;
        }

        lastAmount = healAmount;
    }
}
