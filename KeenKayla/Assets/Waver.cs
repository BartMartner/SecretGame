using UnityEngine;
using System.Collections;

public class Waver : Enemy
{
    public Vector2 direction = new Vector2(1,1);
    public float frequency = 1;
    public Vector2 speed = new Vector2(2,1);
    public float _timer;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);
    private bool _justChangedDirectionX;
    private bool _justChangedDirectionY;

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        if (_timer < 1000)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer = 0;
        }

        if (direction.x < 0 && transform.rotation != _flippedFacing)
        {
            transform.rotation = _flippedFacing;
        }
        else if (direction.x > 0 && transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
        }

        transform.position += Vector3.right * direction.x * speed.x * Time.deltaTime;
        transform.position += Vector3.up * direction.y * speed.y * Mathf.Sin(_timer * frequency * Mathf.PI) * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var normal = collision.contacts[0].normal;
        if (Mathf.Abs(normal.y) > 0)
        {
            if (_justChangedDirectionY)
            {
                return;
            }

            _timer = 0;
            direction.y = Mathf.Sign(normal.y);
            StartCoroutine(JustChangedDirectionY());
        }
        else
        {
            if (_justChangedDirectionX)
            {
                return;
            }

            direction.x = -direction.x;
            StartCoroutine(JustChangedDirectionX());
        }
    }

    private IEnumerator JustChangedDirectionX()
    {
        _justChangedDirectionX = true;
        yield return new WaitForSeconds(0.25f);
        _justChangedDirectionX = false;
    }

    private IEnumerator JustChangedDirectionY()
    {
        _justChangedDirectionY = true;
        yield return new WaitForSeconds(0.25f);
        _justChangedDirectionY = false;
    }
}
