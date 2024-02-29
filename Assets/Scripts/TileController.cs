using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [field: SerializeField] public Transform SpotPositionTransform { get; private set; }
    public SpotController CurrentSpot { get; set; }
    public IInteractable Interactable { get; private set; }
    public TileState TileState { get; set; }

    [SerializeField] private float _rayDistance;
    [SerializeField] private TileController _tilePrefab;

    private List<TileController> _neighbours = new List<TileController>(4);
    private Vector3[] _directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

    private TileController _wall;

    private void Start()
    {
        UpdateTile();
    }

    public void UpdateTile()
    {
        _neighbours.Clear();

        foreach (Vector3 direction in _directions)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _rayDistance))
            {
                if (hit.collider.gameObject.TryGetComponent(out TileController tile))
                {
                    _neighbours.Add(tile);
                }
            }
        }

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitUp, _rayDistance))
        {
            if (hitUp.collider.gameObject.TryGetComponent(out TileController tile))
            {
                _wall = tile;
            }
        }
    }

    public void AddWallOnTile()
    {
        if (TileState != TileState.Free || _wall != null)
        {
            return;
        }

        TileController wall = Instantiate(_tilePrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
        TileState = TileState.Occupied;
        _wall = wall;
        UpdateTile();
    }

    public void RemoveWall()
    {
        if (_wall != null)
        {
            DestroyImmediate(_wall.gameObject);
            UpdateTile();
        }
    }

    public bool ContainsTile(TileController tileController)
    {
        return _neighbours.Contains(tileController);
    }

    public void SetInteractable(IInteractable interactable)
    {
        Interactable = interactable;
        TileState = TileState.HasInteractable;
    }

    public void RemoveInteractable()
    {
        Interactable = null;
        TileState = TileState.Occupied;
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
    Occupied,
    HasInteractable
}