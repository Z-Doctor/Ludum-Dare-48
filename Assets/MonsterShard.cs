using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShard : MonoBehaviour
{
    public void Pickup(CharacterController2D character)
    {
        if(character is PlayerController player) {
            player.AddMonsterShard(1);
            Destroy(gameObject);
        }
    }
}
