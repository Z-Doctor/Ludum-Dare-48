using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CallbackContext
{
    public bool performed => phase == InputActionPhase.Performed;
    public InputActionPhase phase;
    private object value;

    public CallbackContext()
    {
    }

    public CallbackContext(object value)
    {
        this.value = value;
    }

    public V ReadValue<V>()
    {
        return (V) value;
    }
}