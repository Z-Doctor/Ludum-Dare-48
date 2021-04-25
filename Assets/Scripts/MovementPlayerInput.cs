using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class MovementPlayerInput : MonoBehaviour
{
    [SerializeField] bool listening;
    public bool Listening { get => listening; set => SetListening(value); }

    public Events events;

    bool jumpStarted;
    bool pauseStarted;
    bool attackStarted;

    void Update()
    {
        if(!listening)
            return;

        events.move.Invoke(new CallbackContext(Input.GetAxisRaw("Horizontal")));

        if(Input.GetAxisRaw("Jump") != 0 && !jumpStarted)
        {
            jumpStarted = true;
            events.jump.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Started
            });
        }
        else if(jumpStarted && Input.GetAxisRaw("Jump") == 0)
        {
            jumpStarted = false;
            events.jump.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Performed
            });
        }

        if(Input.GetAxisRaw("Pause") != 0 && !pauseStarted)
        {
            pauseStarted = true;
        }
        else if(pauseStarted && Input.GetAxisRaw("Pause") == 0)
        {
            pauseStarted = false;
            events.pause.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Performed
            });
        }

        if(Input.GetAxisRaw("Attack") != 0 && !attackStarted)
        {
            attackStarted = true;
            events.attack.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Started
            });
        }
        else if(attackStarted && Input.GetAxisRaw("Attack") == 0)
        {
            attackStarted = false;
            events.attack.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Performed
            });
        }
    }

    void SetListening(bool value)
    {
        listening = value;

        if(!listening)
        {
            jumpStarted = pauseStarted = false;
        }
    }

    [Serializable]
    public struct Events
    {
        public UnityEvent<CallbackContext> move;
        public UnityEvent<CallbackContext> jump;
        public UnityEvent<CallbackContext> pause;
        public UnityEvent<CallbackContext> attack;
    }
}
