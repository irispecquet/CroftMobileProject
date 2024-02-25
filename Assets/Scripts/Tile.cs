using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerPosition { get; private set; }
    
    [SerializeField] private float _rayDistance;

    private List<Tile> _neighbours = new List<Tile>(4);
    private Vector3[] _directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private void Start()
    {
        RaycastHit hit;

        foreach (Vector3 direction in _directions)
        {
            if (Physics.Raycast(transform.position, direction, out hit, _rayDistance))
            {
                if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                {
                    _neighbours.Add(tile);
                }
            }
        }
    }

    public bool ContainsTile(Tile tile)
    {
        return _neighbours.Contains(tile);
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