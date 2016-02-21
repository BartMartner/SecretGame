using UnityEngine;
using System.Collections;

public class Door : Damagable
{
    [Header("Door")]
    public BoxCollider2D boxCollider2D;
    public GameObject shield;
    private bool _locked;
    private SpriteRenderer _shieldRenderer;
    private Color _originalShieldColor;
    public AudioClip open;
    public AudioClip close;

    public bool locked
    {
        get
        {
            return _locked;
        }

        set
        {
            _locked = value;

            if(_shieldRenderer)
            {
                if (_locked)
                {
                    _shieldRenderer.color = Color.gray;
                }
                else
                {
                    _shieldRenderer.color = _originalShieldColor;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _shieldRenderer = shield.GetComponent<SpriteRenderer>();
        _originalShieldColor = shield.GetComponent<SpriteRenderer>().color;
    }

    public override bool Hurt(float damage, GameObject source = null, DamageType damageType = DamageType.Generic)
    {
        if (locked)
        {
            return false;
        }

        if (!immunities.Contains(damageType))
        {
            Open();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Open()
    {
        boxCollider2D.enabled = false;
        shield.gameObject.SetActive(false);
        audioSource.PlayOneShot(open);
    }

    public void Close()
    {
        boxCollider2D.enabled = true;
        shield.gameObject.SetActive(true);
        audioSource.PlayOneShot(close);
    }
}
