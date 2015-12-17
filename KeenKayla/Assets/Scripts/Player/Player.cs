using UnityEngine;
using System.Collections;

public class Player : Damagable 
{
    public static Player instance;

    public LayerMask playerLayerMask;
    private PlayerController _controller;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
        _controller = GetComponentInChildren<PlayerController>();
        playerLayerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
        deathTime = 1.75f;
    }

    public override bool Hurt(float damage, GameObject source = null)
    {
        if (_aegisActive || state != DamagableState.Alive)
        {
            return false;
        }

        var result = base.Hurt(damage, source);

        if (result && health > 0)
        {
            var direction = transform.position.x > source.transform.position.x ? 1 : -1;
            Knockback(direction, 0.25f, 0.75f);
            StartCoroutine(_controller.OnHit());
        }

        return result;
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("Dead", true);
        PlayerController.instance.OnDeath();
    }

    public bool ValidBlock(GameObject source)
    {
        Vector3 direction;
        var result = Physics2D.Raycast(source.transform.position, source.transform.right, 100, playerLayerMask);

        if (result)
        {
            direction = (Vector3)result.point - transform.position;
            //Debug.DrawLine(transform.position, result.point, Color.red, 1);
        }
        else
        {
            direction = (source.transform.position - transform.position).normalized;
            //Debug.DrawLine(transform.position, source.transform.position, Color.red, 1);
        }

        var angle = Vector3.Angle(direction, transform.right);
        var validAngle = angle < 45;
        return validAngle;
    }

    public virtual IEnumerator StaminaFlash()
    {
        var timer = 0f;
        var staminaColor = new Color(0.33f,0.33f,0.33f, 1);
        SetColors(staminaColor, "_EmissionColor");
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            SetColors(Color.Lerp(staminaColor,defaultColor, timer/0.5f), "_EmissionColor");
            yield return null;
        }

        SetColors(defaultColor, "_EmissionColor");
        _flashing = false;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
