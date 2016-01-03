using UnityEngine;
using System.Collections;

public class NoPushing : MonoBehaviour
{
    private Collider2D _collider2D;
    private Rigidbody2D _rigidBody2D;
    private Vector3 _playerDelta;
    private RigidbodyConstraints2D _defaultConstraints;
    private RigidbodyConstraints2D _noPushConstraints;
    private float _xDistance;
    private Collider2D _playerCollider;
    //private float _yOffset = 0.5f;
    //private float _playerYOffset = 0.5f;
    private int _framesCounter;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _playerCollider = Player.instance.collider2D;
        _defaultConstraints = _rigidBody2D.constraints;
        _noPushConstraints = _defaultConstraints | RigidbodyConstraints2D.FreezePositionX;
    }

    private void FixedUpdate()
    {
        var playerMidBottom = Player.instance.transform.position.y - _playerCollider.bounds.extents.y * 0.5f;
        var bottom = transform.position.y - _collider2D.bounds.extents.y;
        _playerDelta = Player.instance.transform.position - transform.position + Vector3.up;
        _xDistance = _collider2D.bounds.extents.x + _playerCollider.bounds.extents.x;
        bool inYRange = playerMidBottom < transform.position.y && playerMidBottom > bottom;
        if (Mathf.Abs(_playerDelta.x) < _xDistance && inYRange)
        {
            _framesCounter++;
            if (_framesCounter > 3 && _rigidBody2D.constraints != _noPushConstraints)
            {
                _rigidBody2D.constraints = _noPushConstraints;
            }
        }
        else if (_rigidBody2D.constraints != _defaultConstraints)
        {
            _framesCounter = 0;
            _rigidBody2D.constraints = _defaultConstraints;
        }
    }
}
