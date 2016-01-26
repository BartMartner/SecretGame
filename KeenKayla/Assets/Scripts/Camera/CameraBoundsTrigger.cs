using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraBoundsTrigger : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    public bool transition = true;

    private void Awake()
    {
        gameObject.layer = LayerConstants.PlayerTrigger;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.isTrigger = true;
        Player.instance.onSpawn += OnPlayerSpawn;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var reallyTrasition = transition && Vector3.Distance(Player.instance.transform.position, MainCamera.instance.transform.position) < 20;
        MainCamera.instance.SetLimits(_boxCollider2D.bounds, reallyTrasition);        
    }

    public void OnPlayerSpawn()
    {
        if (_boxCollider2D.bounds.Contains(Player.instance.transform.position))
        {
            MainCamera.instance.SetLimits(_boxCollider2D.bounds, false);
            MainCamera.instance.requireUpdate = true;
        }
        Player.instance.onSpawn -= OnPlayerSpawn;
    }

    public void OnDrawGizmos()
    {
        if (!_boxCollider2D)
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        Vector3 bottomLeft = new Vector3(_boxCollider2D.bounds.min.x, _boxCollider2D.bounds.min.y, 0);
        Vector3 bottomRight = new Vector3(_boxCollider2D.bounds.max.x, _boxCollider2D.bounds.min.y, 0);
        Vector3 topRight = new Vector3(_boxCollider2D.bounds.max.x, _boxCollider2D.bounds.max.y, 0);
        Vector3 topLeft = new Vector3(_boxCollider2D.bounds.min.x, _boxCollider2D.bounds.max.y, 0);

        Debug.DrawLine(bottomLeft, bottomRight, Color.yellow);
        Debug.DrawLine(bottomRight, topRight, Color.yellow);
        Debug.DrawLine(topRight, topLeft, Color.yellow);
        Debug.DrawLine(topLeft, bottomLeft, Color.yellow);
    }
}
