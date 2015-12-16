using UnityEngine;
using System.Collections;

public class FireZone : MonoBehaviour
{
    public float duration = 8;

    private DamagePlayerTrigger _damageTrigger;
    private ParticleSystem[] _particlesSystems;
    private Collider2D _collider2d;
    private Rigidbody2D _ridgibody2D;

    private void Awake ()
    {
        _damageTrigger = GetComponentInChildren<DamagePlayerTrigger>();
        _particlesSystems = GetComponentsInChildren<ParticleSystem>();
        _collider2d = GetComponent<Collider2D>();
        _ridgibody2D = GetComponent<Rigidbody2D>();

        if (duration != 0)
        {
            StartCoroutine(FadeAfterTime(duration));
        }
	}

    private IEnumerator FadeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var system in _particlesSystems)
        {
            system.Stop();
        }

        if (_damageTrigger)
        {
            _damageTrigger.enabled = false;
            _damageTrigger.GetComponent<Collider2D>().enabled = false;
        }

        if (_ridgibody2D)
        {
            _ridgibody2D.isKinematic = true;
        }

        if(_collider2d)
        {
            _collider2d.enabled = false;
        }

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
