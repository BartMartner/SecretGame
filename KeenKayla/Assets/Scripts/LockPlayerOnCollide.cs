using UnityEngine;
using System.Collections;

public class LockPlayerOnCollide : MonoBehaviour
{
    private Vector3 _lastPosition;
    private bool _playerPresent;

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
            _playerPresent = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _playerPresent = false;
        }
    }

    
}
