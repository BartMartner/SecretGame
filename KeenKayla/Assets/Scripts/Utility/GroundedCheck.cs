using UnityEngine;
using System.Collections;

public class GroundedCheck : MonoBehaviour
{
    public float groundedDistance = 0.1f;
    public bool onGround;
    public bool nearGround;
    public LayerMask groundLayer;

    public RaycastHit2D rightRayHit;
    public RaycastHit2D leftRayHit;
    public RaycastHit2D middleRayHit;

    public AudioClip landingSound;
    public bool justLanded
    {
        get
        {
            return !_lastOnGround && onGround;
        }
    }

    private bool _lastOnGround;
    private float _halfHeight, _halfWidth;
    private Vector3 _leftOffset, _rightOffset;
    private BoxCollider2D _collider2D;
    private AudioSource _audioSource;
    

    private void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _halfHeight = _collider2D.bounds.extents.y;
        _halfWidth = _collider2D.bounds.size.x;
        _leftOffset = Vector3.left * _halfWidth * 0.5f;
        _rightOffset = Vector3.right * _halfWidth * 0.5f;

        _audioSource = GetComponentInChildren<AudioSource>();
    }

    public void UpdateRaycasts()
    {
        _lastOnGround = onGround;

        var offset = -_collider2D.offset.y + _halfHeight * 0.5f;
        var deltaToBottom = (_halfHeight - offset);
        var origin = transform.position + new Vector3(0, -offset, 0);
        var distance = deltaToBottom + (_halfHeight * 1.5f);

        rightRayHit = Physics2D.Raycast(origin + _rightOffset, Vector3.down, distance, groundLayer);
        middleRayHit = Physics2D.Raycast(origin, Vector3.down, distance, groundLayer);
        leftRayHit = Physics2D.Raycast(origin + _leftOffset, Vector3.down, distance, groundLayer);

        nearGround = leftRayHit.collider != null || middleRayHit.collider != null || rightRayHit.collider != null;
        onGround = nearGround && ((leftRayHit.collider != null && leftRayHit.distance < groundedDistance + deltaToBottom) ||
            (middleRayHit.collider != null && middleRayHit.distance < groundedDistance + deltaToBottom) ||
            (rightRayHit.collider != null && rightRayHit.distance < groundedDistance + deltaToBottom));

        if (justLanded & _audioSource != null && landingSound)
        {
            _audioSource.PlayOneShot(landingSound);
        }
    }

    public void OnDrawGizmosSelected()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _halfHeight = _collider2D.bounds.extents.y;
        _halfWidth = _collider2D.bounds.size.x;
        _leftOffset = Vector3.left * _halfWidth * 0.5f;
        _rightOffset = Vector3.right * _halfWidth * 0.5f;

        var offset = -_collider2D.offset.y + _halfHeight * 0.5f;
        var deltaToBottom = (_halfHeight - offset);
        var origin = transform.position + new Vector3(0, -offset, 0);
        var distance = deltaToBottom + (_halfHeight * 1.5f);

        var right = origin + _rightOffset;
        var middle = origin;
        var left = origin + _leftOffset;
        Debug.DrawLine(right, right + Vector3.down * distance);
        Debug.DrawLine(middle, middle + Vector3.down * distance);
        Debug.DrawLine(left, left + Vector3.down * distance);
    }
}
