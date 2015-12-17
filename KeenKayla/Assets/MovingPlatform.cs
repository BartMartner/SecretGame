using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 _lastPosition;
    private bool _playerPresent;

    public void LateUpdate()
    {
        if(_playerPresent)
        {
            PlayerController.instance.transform.position += transform.position - _lastPosition;
        }

        _lastPosition = transform.position;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        _playerPresent = true;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        _playerPresent = false;
    }
}
