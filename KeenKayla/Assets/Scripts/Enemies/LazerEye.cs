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
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(Flash(8, 0.1f, Color.yellow, 0.5f));
        lazer.SetActive(true);
        yield return new WaitForSeconds(3);
        lazer.SetActive(false);
        _firingLazer = false;
    }
}
