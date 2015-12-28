using UnityEngine;
using System.Collections;

public class LockPlayerOnCollide : MonoBehaviour
{
    private Vector3 _lastPosition;
    private bool _playerPresent;

    public void Awake()
    {
        var effector = GetComponent<PlatformEffector2D>();
        if(effector && effector.useColliderMask)
        {
            Debug.LogWarning(gameObject.name + " has an effector using a collider mask which may break the LockPlayerOnCollide script attached to it");
        }
    }

    public void LateUpdate()
    {
        if (_playerPresent)
        {
            PlayerController.instance.transform.position += transform.position - _lastPosition;
        }

        _lastPosition = transform.position;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player Present");
            _playerPresent = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player Exit");
            _playerPresent = false;
        }
    }
}
