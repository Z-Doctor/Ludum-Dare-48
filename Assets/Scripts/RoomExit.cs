using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public RoomInfo roomInfo;
    public LayerMask layers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(roomInfo && layers.MatchLayer(collision.gameObject.layer))
        {
            roomInfo.NextRoom();
        }
    }
}
