using UnityEngine;
using System.Collections;

public class PosionPuddle : DamagePlayerTrigger
{
    [Range(2f, 10f)]
    public float lifeSpan;
    public Sprite nearDead;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(LifeSpan());
    }

    private IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifeSpan - 1);
        _renderer.sprite = nearDead;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
