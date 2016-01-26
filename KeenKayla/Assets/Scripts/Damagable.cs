using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Damagable : MonoBehaviour
{
    [Header("Health and Damage")]
    public DamagableState state;

    public float health = 1;
    public float maxHealth;
    public float healthRatio
    {
        get { return health / maxHealth; }
    }

    protected bool _aegisActive;
    public bool aegisActive
    {
        get { return _aegisActive; }
    }

    public float defaultAegisTime = 0.25f;
    public float deathTime = 0f;
    protected float _deathCounter;
    
    public List<DamageType> immunities;
    public GibType gibs;
    public int gibAmount = 6;
    public bool destroyOnDeath;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip[] hurtSounds;
    public AudioClip[] deathSounds;

    [Header("Visuals")]
    public Animator animator;

    public Color defaultColor;
    public bool flashOnHurt = true;
    protected SpriteRenderer[] _renderers;

    protected bool _flashing;
    public bool flashing
    {
        get { return _flashing; }
    }

    private Color _damageColor;

    [HideInInspector]
    public GroundedCheck groundedCheck;

    protected Collider2D _collider2D;

    protected virtual void Awake()
    {
        _collider2D = GetComponent<Collider2D>();

        if (!audioSource)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }

        maxHealth = health;
        _damageColor = Color.red;
        groundedCheck = GetComponentInChildren<GroundedCheck>();
    }

    protected virtual void Start()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (state)
        {
            case DamagableState.Dead:
                return;
            case DamagableState.Dying:
                _deathCounter += Time.deltaTime;
                if (_deathCounter > deathTime)
                {
                    state = DamagableState.Dead;
                    OnDeath();
                }
                break;
            case DamagableState.Alive:
                UpdateAlive();
                break;
        }
    }

    protected virtual void UpdateAlive()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual IEnumerator Aegis(float aegisTime)
    {
        _aegisActive = true;
        yield return new WaitForSeconds(aegisTime);
        _aegisActive = false;
    }

    public virtual IEnumerator Flash(int flashes, float time, Color color, float amount)
    {
        _flashing = true;
        var flashCounter = 0;

        while (flashCounter < flashes)
        {
            flashCounter++;

            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.material.SetFloat("_FlashAmount", 0);
                }
            }
            yield return new WaitForSeconds(time);

            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.material.SetColor("_FlashColor", color);
                    renderer.material.SetFloat("_FlashAmount", amount);
                }
            }
            yield return new WaitForSeconds(time);
        }

        foreach (var renderer in _renderers)
        {
            if (renderer != null)
            {
                renderer.material.SetColor("_FlashColor", color);
                renderer.material.SetFloat("_FlashAmount", 0);
            }
        }
        _flashing = false;
    }

    public virtual bool Hurt(float damage, GameObject source = null, DamageType damageType = DamageType.Generic)
    {
        if (_aegisActive || state != DamagableState.Alive || (immunities != null && immunities.Contains(damageType)))
        {
            return false;
        }

        health -= damage;

        if (defaultAegisTime > 0)
        {
            StartCoroutine(Aegis(defaultAegisTime));
        }

        if (flashOnHurt)
        {
            StartCoroutine(Flash(1, 0.1f, _damageColor, 0.25f));
        }

        if (audioSource && hurtSounds.Length > 0 && health > 0)
        {
            audioSource.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
        }

        return true;
    }

    public virtual void Die()
    {
        if (state != DamagableState.Alive)
        {
            return;
        }

        state = DamagableState.Dying;
        _deathCounter = 0;

        if (audioSource && deathSounds.Length > 0)
        {
            audioSource.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
        }
    }

    public IEnumerator FadeOut(float time, bool destroy)
    {
        var timer = 0f;

        //Turn on fade
        foreach (var renderer in _renderers)
        {
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;
        }

        while (timer < time)
        {
            timer += Time.deltaTime;
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.color = Color.Lerp(Color.white, Color.clear, timer / time);
                }
            }
            yield return null;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnDeath()
    {
        if (gibs != GibType.None)
        {
            Rect area;
            area = _collider2D ? new Rect(0, 0, _collider2D.bounds.extents.x * 2, _collider2D.bounds.extents.y * 2) : new Rect(0, 0, 1, 1);
            area.center = transform.position;
            GibManager.instance.SpawnGibs(gibs, area, gibAmount);
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }

        //StartCoroutine(FadeOut(1, true));
    }

    public void Knockback(float direction, float time, float distance)
    {
        StartCoroutine(KnockbackCoroutine(direction, time, distance));
    }

    public virtual IEnumerator KnockbackCoroutine(float direction, float time, float distance)
    {
        var timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            transform.position += Vector3.right * distance * direction * (Time.deltaTime / time);
            yield return null;
        }
    }
}
