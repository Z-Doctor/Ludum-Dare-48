using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Range(0, 1)] public float lootChance = 0.5f;

    public Loot[] lootTable;

    [ContextMenu("Drop Loot")]
    public void DropLoot()
    {
        DropLoot(transform.position);
    }

    public void DropLoot(Vector3 worldPosition)
    {
        if(Random.Range(0, 1f) <= lootChance)
        {
            foreach(var item in lootTable)
            {
                if(Random.Range(0, 1f) <= item.dropChance)
                {
                    var loot = Instantiate(item.prefab, transform.parent);
                    loot.transform.position = worldPosition;
                    //var lootBody = loot.GetComponent<Rigidbody2D>();
                    //if(lootBody)
                    //{
                    //    //Vector2 throwDir = Quaternion.AngleAxis(Random.Range(30, 150), Vector3.forward) * Vector2.left;
                    //    lootBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    //}
                }
            }
        }
    }

    [System.Serializable]
    public class Loot
    {
        [Range(0, 1)] public float dropChance = 0.5f;
        public GameObject prefab;
    }
}
