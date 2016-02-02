using UnityEngine;
using System.Collections;

public class LazerEye : Enemy
{
    [Header("LazerEye")]
    public GameObject lazer;
    public AreaEffector2D push;
    private bool _firingLazer;

    protected override void Awake()
    {
        base.Awake();
        lazer.SetActive(false);
        push.forceAngle = transform.localScale.x < 0 ? 0 : -180;
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        if(!_firingLazer)
        {
            StartCoroutine(FireLazer());
        }
    }

    public override void OnDeath()
    {
        lazer.SetActive(false);
        StopAllCoroutines();
        base.OnDeath();
    }

    private IEnumerator FireLazer()
    {
        _firingLazer = true;
        yield return new WaitForSeconds(3);
        animator.SetTrigger("Charge");
        yield return StartCoroutine(Flash(1, 0.1f, new Color32(252,84,252,255), 0.1f));
        yield return StartCoroutine(Flash(1, 0.1f, new Color32(252, 84, 252, 255), 0.2f));
        yield return StartCoroutine(Flash(1, 0.1f, new Color32(252, 84, 252, 255), 0.3f));
        yield return StartCoroutine(Flash(1, 0.1f, new Color32(252, 84, 252, 255), 0.4f));
        yield return StartCoroutine(Flash(1, 0.1f, new Color32(252, 84, 252, 255), 0.5f));
        lazer.SetActive(true);
        animator.SetBool("Firing", true);
        yield return StartCoroutine(Flash(15, 0.05f, new Color32(252, 84, 252, 255), 0.25f));
        animator.SetBool("Firing", false);
        lazer.SetActive(false);
        _firingLazer = false;
    }
}
