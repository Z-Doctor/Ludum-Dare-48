using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpriteBlinkTint : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public float frequency = 10;

    Vector4 startTint;
    public Color endTint = Color.white;

    bool blinking;
    float elapsedTime;

    private void Update()
    {
        if(!blinking)
            return;

        spriteRenderer.color = Vector4.Lerp(startTint, endTint, Mathf.Abs(Mathf.Sin(elapsedTime / (1/ frequency))));
        elapsedTime += Time.deltaTime;
    }

    [ContextMenu("Start Blinking")]
    public void StartBlink()
    {
        startTint = spriteRenderer.color;
        blinking = true;
        elapsedTime = 0;
    }

    [ContextMenu("Stop Blinking")]
    public void StopBlink()
    {
        blinking = false;
        spriteRenderer.color = startTint;
    }
}
