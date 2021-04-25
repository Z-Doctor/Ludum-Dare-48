using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    public Events events;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<CharacterController2D>();
        if(controller && layer.MatchLayer(controller.gameObject.layer))
        {
            events.onTriggerEnter.Invoke(controller);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<CharacterController2D>();
        if(controller && layer.MatchLayer(controller.gameObject.layer))
        {
            events.onTriggerStay.Invoke(controller);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var controller = collision.gameObject.GetComponent<CharacterController2D>();
        if(controller && layer.MatchLayer(controller.gameObject.layer))
        {
            events.onTriggerExit.Invoke(controller);
        }
    }

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<CharacterController2D> onTriggerEnter;
        public UnityEvent<CharacterController2D> onTriggerStay;
        public UnityEvent<CharacterController2D> onTriggerExit;
    }
}
