using UnityEngine;
using System.Collections;

public class RefillEnemy : Enemy
{
    public float dashSpeed = 5f;
    public float riseSpeed = 1f;
    private Vector3 _riseTarget;
    // Use this for initialization

    protected override void Start()
    {
        base.Start();
        _riseTarget = transform.position;
        _riseTarget.y = Player.instance.transform.position.y;
        StartCoroutine(Spawn());
	}
	
    private IEnumerator Spawn()
    {
        while(transform.position != _riseTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _riseTarget, riseSpeed * Time.deltaTime);
            yield return null;
        }
                
        var direction = Mathf.Sign(Player.instance.transform.position.x - transform.position.x);

        if (direction > 0 && transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        var timer = 0f;
        while (timer < 8 || _renderers[0].isVisible)
        {
            timer += Time.deltaTime;
            transform.position += Vector3.right * direction * dashSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public override void Die()
    {
        StopAllCoroutines();
        base.Die();
    }
}
