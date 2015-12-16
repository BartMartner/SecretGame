using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour
{
    public DamagableState state;

    public float health = 1;
    public float maxHealth;
    public float healthRatio
    {
        get { return health / maxHealth; }
    }

    public float defaultAegisTime = 0.25f;
    public float deathTime = 1f;

    public Animator animator;

    public AudioSource audioSource;
    public AudioClip[] hurtSounds;
    public AudioClip[] deathSounds;

    public Color defaultColor;
    public bool flashOnHurt = true;
    protected Renderer[] _renderers;
    protected float _deathCounter;

    protected bool _aegisActive;
    public bool aegisActive
    {
        get { return _aegisActive; }
    }

    protected bool _flashing;
    public bool flashing
    {
        get { return _flashing; }
    }

    private Color _damageColor;

    [HideInInspector]
    public GroundedCheck groundedCheck;
    public bool isKnockedUp;

    protected virtual void Awake()
    {
        if (!audioSource)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }

        maxHealth = health;
        _damageColor = new Color(0.333f, 0.01f, 0.01f);
    }

    protected virtual void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        if (_renderers != null && _renderers.Length > 0)
        {
            bool defaultColorSet = false;
            foreach (var renderer in _renderers)
            {
                renderer.material.EnableKeyword("_EMISSION");

                if (!defaultColorSet && renderer.material.HasProperty("_EmissionColor"))
                {
                    defaultColorSet = true;
                    defaultColor = renderer.material.GetColor("_EmissionColor");
                }
            }
        }
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

    public virtual IEnumerator Flash(int flashes, float time, Color color)
    {
        _flashing = true;
        var flashCounter = 0;

        while (flashCounter < flashes)
        {
            flashCounter++;

            SetColors(color, "_EmissionColor");
            yield return new WaitForSeconds(time);

            SetColors(defaultColor, "_EmissionColor");
            yield return new WaitForSeconds(time);
        }

        SetColors(defaultColor, "_EmissionColor");
        _flashing = false;
    }

    public virtual bool Hurt(float damage, GameObject source = null)
    {
        if (_aegisActive || state != DamagableState.Alive)
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
            StartCoroutine(Flash(1, 0.1f, _damageColor));
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
            SetColors(Color.Lerp(Color.white, Color.clear, timer / time), "_Color");
            yield return null;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnDeath()
    {
        StartCoroutine(FadeOut(1, true));
    }

    //TODO: string for property
    protected virtual void SetColors(Color color, string property)
    {
        foreach (var renderer in _renderers)
        {
            if (renderer != null)
            {
                if (string.IsNullOrEmpty(property))
                {
                    renderer.material.SetColor("_Color", color);
                }
                else
                {
                    renderer.material.SetColor(property, color);
                }
            }
        }
    }

    public virtual void OnKnockUpStart()
    {

    }

    public virtual void OnKnockUpEnd()
    {

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
