using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SpriteAnimation : MonoBehaviour
{

    public string startAnimation;
    public Animation[] animations;
    Animation currentAnimation;
    float timeElasped;
    int frame;

    public Events events;

    public Sprite Frame => currentAnimation?.frames[frame];
    public float FrameTime => currentAnimation?.frameDelay[frame] / 1000f ?? 0;

    private void Start()
    {
        Play(startAnimation);
    }

    private void Update()
    {
        if(currentAnimation != null)
        {
            timeElasped += Time.deltaTime;
            if(timeElasped >= FrameTime)
            {
                AdvanceFrame();
            }
        }
    }

    public void Stop()
    {
        currentAnimation = null;
    }

    public void ClearSprite()
    {
        events.onSpriteChange.Invoke(null);
    }

    public void AdvanceFrame()
    {
        if(currentAnimation is null)
            return;

        frame++;
        if(frame >= currentAnimation.frames.Length)
        {
            if(currentAnimation.loop)
            {
                ChangeFrame(0);
                events.onAnimationLoop.Invoke(currentAnimation);
            }
            else
            {
                events.onAnimationFinished.Invoke(currentAnimation);
                if(currentAnimation.clearOnFinish)
                    ClearSprite();
                currentAnimation = null;
            }
        }
        else
            ChangeFrame(frame);
    }

    public void ChangeFrame(int frame)
    {
        if(currentAnimation is null)
            return;

        timeElasped = 0;

        this.frame = frame;
        events.onFrameChange.Invoke(frame);
        events.onSpriteChange.Invoke(Frame);

        var triggered = currentAnimation.triggers.FirstOrDefault(f => f.frame == frame);
        if(triggered != null)
        {
            triggered.onTrigger.Invoke(currentAnimation);
        }
    }

    public void Play(string animName)
    {
        if(currentAnimation != null && currentAnimation.animationName == animName)
            return;

        var anim = animations.FirstOrDefault(anim => anim.animationName == animName);
        Play(anim);
    }

    public void Restart()
    {
        if(currentAnimation != null)
            ChangeFrame(0);
    }

    public void Play(Animation animation)
    {
        if(animation is null)
        {
            currentAnimation = null;
            return;
        }
        timeElasped = 0;
        currentAnimation = animation;

        ChangeFrame(0);
    }

    [ContextMenu("Validate")]
    private void OnValidate()
    {
        if(animations != null)
            for(int i = 0; i < animations.Length; i++)
            {
                animations[i].Validate();
            }
    }

    [System.Serializable]
    public struct Events
    {
        public UnityEvent<int> onFrameChange;
        public UnityEvent<Sprite> onSpriteChange;
        public UnityEvent<Animation> onAnimationFinished;
        public UnityEvent<Animation> onAnimationLoop;
    }

    [System.Serializable]
    public class Animation
    {
        public string animationName;
        public bool loop = true;
        public bool clearOnFinish;
        public AnimationTrigger[] triggers;
        public Sprite[] frames;
        public int[] frameDelay;

        public void Validate()
        {
            int[] frameDelay = new int[frames?.Length ?? 0];
            int start = 0;
            if(this.frameDelay != null && frames.Length > 0)
            {
                System.Array.Copy(this.frameDelay, frameDelay, this.frameDelay.Length);
                start = this.frameDelay.Length;
            }

            for(int i = start; i < frames.Length; i++)
            {
                frameDelay[i] = 250;
            }

            this.frameDelay = frameDelay;
        }
    }

    [System.Serializable]
    public class AnimationTrigger
    {
        public string triggerName;
        public int frame;

        public UnityEvent<Animation> onTrigger;
    }
}
