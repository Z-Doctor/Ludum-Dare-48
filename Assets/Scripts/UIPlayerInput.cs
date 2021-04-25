using UnityEngine;
using UnityEngine.Events;

public class UIPlayerInput : MonoBehaviour
{
    [SerializeField] bool listening;
    public bool Listening { get => listening; set => listening = value; }

    public Events events;

    float lastVerticalValue;

    int direction;

    bool selectStarted;
    bool pauseStarted;

    // Update is called once per frame
    void Update()
    {
        if(!listening)
            return;

        var vertical = Input.GetAxisRaw("Vertical");
        if(vertical != lastVerticalValue)
        {
            lastVerticalValue = vertical;
            if(vertical > 0 && direction != 1)
            {
                direction = 1;
                events.upNav.Invoke(new CallbackContext(vertical)
                {
                    phase = InputActionPhase.Performed
                });
            }
            else if(vertical < 0 && direction != -1)
            {
                direction = -1;
                events.downNav.Invoke(new CallbackContext(vertical)
                {
                    phase = InputActionPhase.Performed
                });
            }
            else if(vertical == 0)
            {
                direction = 0;
            }
        }

        if(Input.GetAxisRaw("Select") != 0 && !selectStarted)
        {
            selectStarted = true;
            events.select.Invoke(new CallbackContext()
            {
                phase = InputActionPhase.Started
            });
        }
        else if(selectStarted && Input.GetAxisRaw("Select") == 0)
        {
            selectStarted = false;
            events.select.Invoke(new CallbackContext()
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
    }
    [System.Serializable]
    public struct Events
    {
        public UnityEvent<CallbackContext> upNav;
        public UnityEvent<CallbackContext> downNav;
        public UnityEvent<CallbackContext> select;
        public UnityEvent<CallbackContext> pause;
    }
}


public enum InputActionPhase
{
    Started,
    Performed,
    Canceled
}