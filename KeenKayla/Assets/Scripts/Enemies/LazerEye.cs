using UnityEngine;
using System.Collections;

public class LazerEye : Enemy
{
    [Header("LazerEye")]
    public GameObject lazer;
    private bool _firingLazer;

    protected override void Awake()
    {
        base.Awake();
        lazer.SetActive(false);
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
        yield return StartCoroutine(Flash(5, 0.2f, Color.yellow, 0.5f));
        lazer.SetActive(true);
        yield return new WaitForSeconds(4);
        lazer.SetActive(false);
        _firingLazer = false;
    }
}
