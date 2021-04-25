using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public abstract class CharacterController2D : MonoBehaviour
{
    [HideInInspector] [SerializeField] protected Rigidbody2D body;
    [HideInInspector] [SerializeField] protected SpriteRenderer spriteRenderer;

    [HideInInspector] [SerializeField] protected AudioSource audioSource;

    public float speed = 28;
    public float jumpPower = 10;
    public bool isAlive = true;
    [Min(0)] public float invincibilityTime = 0.5f;

    [Min(0)] public float landingZoneAngle = 45;

    [Min(0)] public float attackDamage = 0.5f;

    [SerializeField] [Min(1)] int maxHealth = 3;
    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            if(maxHealth != value)
            {
                maxHealth = Mathf.Max(value, 0);
            }
            events.onHealthChanged.Invoke();
        }
    }

    [SerializeField] [Min(0)] float health = 3;
    public float Health
    {
        get => health;
        set
        {
            if(health != value)
            {
                health = Mathf.Clamp(value, 0, MaxHealth);
                events.onHealthChanged.Invoke();
            }
        }
    }

    public bool IsMoving => movement.x != 0;
    public bool IsHurt => Health < MaxHealth;

    public Events events;

    protected Vector2 movement;
    protected List<ContactPoint2D> contacts = new List<ContactPoint2D>();
    float invincibilityTimer;

    public bool IsInvulnerable => invincibilityTimer > 0;

    public bool IsTouchingFloor()
    {
        foreach(var contact in contacts)
        {
            if(!contact.enabled)
                continue;

            var dir = body.position - contact.point;
            //var angle = Vector2.SignedAngle(transform.up, dir.normalized);
            var angle = Vector2.Angle(transform.up, dir.normalized);
            if(angle <= landingZoneAngle)
                return true;
            //Debug.Log(angle);
        }
        return false;
    }

    public virtual void Attack(IEnumerable<CharacterController2D> controllers)
    {
        foreach(var item in controllers)
        {
            item.Damage(attackDamage);
        }
    }

    public void PlaySound(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }

    public virtual void Damage(float damage)
    {
        if(IsInvulnerable)
            return;

        if(damage < 0)
            Debug.LogWarning($"Damage is a negative value, acting as a heal: {damage}");
        Health -= damage;
        OnDamaged(damage);
    }

    public void Heal(float healAmount)
    {
        if(healAmount < 0)
            Debug.LogWarning($"Heal is a negative value, acting as a damage: {healAmount}");
        Health += healAmount;
    }

    protected virtual void OnDamaged(float damage)
    {
        invincibilityTimer = invincibilityTime;
        if(invincibilityTimer > 0)
            events.onInvulernable.Invoke();
        events.onDamaged.Invoke(damage);
    }

    public virtual void Move(float direction)
    {
        movement.x = direction;
        Vector3 scale = transform.localScale;
        if(direction < 0)
            scale.x = -Mathf.Abs(scale.x);
        else if(direction > 0)
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    public virtual void Jump()
    {
        if(IsTouchingFloor())
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            events.onJump.Invoke();
        }
    }

    protected virtual void FixedUpdate()
    {
        body.AddForce(movement * speed);
    }

    protected virtual void Update()
    {
        if(isAlive && health == 0)
        {
            isAlive = false;
            OnDied();
        }

        if(!isAlive)
            return;

        if(invincibilityTimer > 0)
        {
            invincibilityTimer = Mathf.Max(0, invincibilityTimer - Time.deltaTime);
            if(invincibilityTimer <= 0)
                events.onInvulernableEnd.Invoke();
        }

        body.GetContacts(contacts);
    }

    protected virtual void OnDied()
    {
        events.onDied.Invoke();
        Destroy(gameObject);
    }

    protected virtual void OnValidate()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    [System.Serializable]
    public struct Events
    {
        public UnityEvent onHealthChanged;
        public UnityEvent onDied;
        public UnityEvent onInvulernable;
        public UnityEvent onInvulernableEnd;
        public UnityEvent<float> onDamaged;

        public UnityEvent onJump;
    }
}
