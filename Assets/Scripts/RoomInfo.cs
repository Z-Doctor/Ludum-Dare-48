using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [HideInInspector] [SerializeField] RoomGenerator roomGenerator;

    public GameObject roomPrefab;
    public Transform playerSpawn;

    public void NextRoom()
    {
        roomGenerator.LoadRandomRoom();
        Destroy(gameObject);
    }

    private void Awake()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }

    private void OnEnable()
    {
        var player = FindObjectOfType<PlayerController>();
        if(player && playerSpawn)
            player.transform.position = playerSpawn.transform.position;
    }

}
