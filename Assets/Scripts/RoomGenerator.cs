using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomGenerator : MonoBehaviour
{
    public TMP_Text score;
    public Transform roomParent;
    public GameObject[] rooms;

    int depth;

    public RoomInfo LoadRandomRoom()
    {
        IncrementDepth();
        return Instantiate(rooms[Random.Range(0, rooms.Length)], roomParent).GetComponent<RoomInfo>();
    }

    void IncrementDepth()
    {
        depth++;
        score.text = $"{depth}";
    }

    private void Start()
    {
        LoadRandomRoom();
    }
}
