using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExludeInWebGL : MonoBehaviour
{
    public Events events;

    void Start()
    {
#if UNITY_WEBGL
        events.onDestory.Invoke(gameObject);
        Destroy(gameObject);
#endif
    }

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<GameObject> onDestory;
    }
}
