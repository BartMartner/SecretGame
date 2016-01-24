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
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        MainCamera.instance.SetLimits(_boxCollider2D.bounds, transition);
    }
}
