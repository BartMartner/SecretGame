using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CouncilMember : MonoBehaviour
{
    public float wanderRange = 1;
    public float speed = 0.5f;
    public bool rescued;

    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction = Vector3.right;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    protected void Awake()
    {
        _startingPosition = transform.position;
        _targetPosition = _startingPosition + wanderRange * _direction;
    }

    protected void Update()
    {
        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
        }
        else
        {
            _direction *= -1;
            _targetPosition = _startingPosition + wanderRange * _direction;

            if (_direction.x < 0 && transform.rotation != _flippedFacing)
            {
                transform.rotation = _flippedFacing;
            }
            else if (_direction.x > 0 && transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!rescued && collision.gameObject.layer == LayerConstants.Player)
        {
            rescued = true;
            StartCoroutine(GoToEnding());
        }
    }

    public IEnumerator GoToEnding()
    {
        UIMain.instance.ShowTextBar("Thank You For Rescuing Me!");
        Time.timeScale = 0;
        var timer = 0f;
        while (timer < 4)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("TheEnd");

    }

    public void OnDrawGizmosSelected()
    {
        var _boxCollider = GetComponentInChildren<Collider2D>();
        var left = transform.position - _direction * wanderRange;
        var right = transform.position + _direction * wanderRange;
        Debug.DrawLine(left, right);
        Vector3 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = topRight = bottomLeft = bottomRight = Vector3.zero;

        topLeft.y += _boxCollider.bounds.extents.y;
        topRight.y += _boxCollider.bounds.extents.y;
        bottomLeft.y -= _boxCollider.bounds.extents.y;
        bottomRight.y -= _boxCollider.bounds.extents.y;

        topLeft.x -= _boxCollider.bounds.extents.x;
        topRight.x += _boxCollider.bounds.extents.x;
        bottomLeft.x -= _boxCollider.bounds.extents.x;
        bottomRight.x += _boxCollider.bounds.extents.x;

        Debug.DrawLine(left + topLeft, left + bottomLeft);
        Debug.DrawLine(right + topRight, right + bottomRight);
    }
}
