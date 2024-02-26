using System;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerPositionTransform { get; private set; }
    public TileState TileState { get; set; }
    
    [SerializeField] private float _rayDistance;

    private List<TileController> _neighbours = new List<TileController>(4);
    private Vector3[] _directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private void Start()
    {
        RaycastHit hit;

        foreach (Vector3 direction in _directions)
        {
            if (Physics.Raycast(transform.position, direction, out hit, _rayDistance))
            {
                if (hit.collider.gameObject.TryGetComponent(out TileController tile))
                {
                    _neighbours.Add(tile);
                }
            }
        }
    }

    public bool ContainsTile(TileController tileController)
    {
        return _neighbours.Contains(tileController);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        foreach (Vector3 direction in _directions)
        {
            Gizmos.DrawLine(transform.position, transform.position + _rayDistance * direction);
        }
    }
}

public enum TileState
{
    Free,
    Occupied
}