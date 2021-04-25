using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [HideInInspector] [SerializeField] PlayerController player;

    public bool follow = true;
    public bool enforceBounds = false;

    public Bounds bounds;

    public Vector3 GetPlayerPosition()
    {
        var position = player.transform.position;
        //position.z = transform.position.z;
        return position;
    }

    public void SetPosition(Vector3 position)
    {
        if(enforceBounds)
        {
            position.x = Mathf.Clamp(position.x, bounds.center.x, bounds.extents.x);
            position.y = Mathf.Clamp(position.y, bounds.center.y, bounds.extents.y);
        }
        position.z = Mathf.Clamp(position.z, bounds.center.z, bounds.extents.z);
        transform.position = position;
    }

    [ContextMenu("Recenter Camera")]
    public void RecenterCamera()
    {
        SetPosition(GetPlayerPosition());
    }

    void Update()
    {
        if(follow)
            RecenterCamera();
    }

    void OnValidate()
    {
        player = FindObjectOfType<PlayerController>();
    }
}
