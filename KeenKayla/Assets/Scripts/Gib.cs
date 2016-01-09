using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Gib : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Color _orignalColor;
    private float _lifeSpan;
    private float _lifeCounter;

    public void Awake ()
    {
        _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _orignalColor = _spriteRenderer.color;
    }

    public void Update ()
    {
        _lifeCounter += Time.deltaTime;

        if (_lifeCounter > _lifeSpan)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void Spawn(GibType gType, Vector3 origin, float force, float lifeSpan)
    {
        transform.parent = null;
        transform.position = origin;
        _lifeSpan = lifeSpan;
        _lifeCounter = 0;
        _spriteRenderer.color = _orignalColor;
        _rigidbody2D.AddForce(Random.insideUnitCircle.normalized * force);
        gameObject.SetActive(true);

        gameObject.layer = LayerConstants.EnvironmentOnly;
    }

    public IEnumerator FadeOut()
    {
        var targetColor = _spriteRenderer.color;
        targetColor.a = 0f;
        var timer = 0f;
        var fadeTime = 1f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            _spriteRenderer.color = Color.Lerp(_orignalColor, targetColor, timer / fadeTime);
            yield return null;
        }
        transform.parent = GibManager.instance.transform;
        gameObject.SetActive(false);
    }
}

