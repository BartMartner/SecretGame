using UnityEngine;
using System.Collections;

public class GroundedCheck : MonoBehaviour
{
    public float groundedDistance = 0.1f;
    //public bool onOneWayPlatform;
    public bool onGround;
    public int groundFrames;
    public int groundTolerance = 12;
    public bool nearGround;
    public LayerMask groundLayer;

    public RaycastHit2D rightRayHit;
    public RaycastHit2D leftRayHit;
    public RaycastHit2D middleRayHit;

    private float _halfHeight, _halfWidth;
    private Vector3 _leftOffset, _rightOffset;
    private BoxCollider2D _collider2D;
    private AudioSource _audioSource;

    private bool _player;
    private Player _playerController;
    private AudioClip[] _landingSounds;

    private void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        _halfHeight = _collider2D.bounds.extents.y;
        _halfWidth = _collider2D.bounds.size.x;
        _leftOffset = Vector3.left * _halfWidth * 0.5f;
        _rightOffset = Vector3.right * _halfWidth * 0.5f;

        _audioSource = GetComponentInChildren<AudioSource>();

        _playerController = GetComponentInChildren<Player>();
        if(_playerController != null)
        {
            _player = true;
            _landingSounds = _playerController.landingSounds;
        }
    }

    public void UpdateRaycasts()
    {
        var offset = _halfHeight * 0.5f;
        var deltaToBottom = (_halfHeight - offset);
        var origin = transform.position + Vector3.down * offset;
        var distance = deltaToBottom + (_halfHeight * 1.5f);

        rightRayHit = Physics2D.Raycast(origin + _rightOffset, Vector3.down, distance, groundLayer);
        middleRayHit = Physics2D.Raycast(origin, Vector3.down, distance, groundLayer);
        leftRayHit = Physics2D.Raycast(origin + _leftOffset, Vector3.down, distance, groundLayer);

        nearGround = leftRayHit.collider != null || middleRayHit.collider != null || rightRayHit.collider != null;
        onGround = nearGround && ((leftRayHit.collider != null && leftRayHit.distance < groundedDistance + deltaToBottom) ||
            (middleRayHit.collider != null && middleRayHit.distance < groundedDistance + deltaToBottom) ||
            (rightRayHit.collider != null && rightRayHit.distance < groundedDistance + deltaToBottom));

        if (onGround && groundFrames < groundTolerance)
        {
            if(_player && !_playerController.jumping || !_player)
            {
                if (groundFrames <= 0 && _audioSource != null && _landingSounds != null && _landingSounds.Length > 0)
                {
                    _audioSource.PlayOneShot(_landingSounds[Random.Range(0, _landingSounds.Length)]);
                }

                groundFrames = groundTolerance;
            }
        }

        if (!onGround && groundTolerance > 0)
        {
            groundFrames--;
        }
    }
}
