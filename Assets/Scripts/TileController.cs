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

    private void Start()
    {
        UpdateTile();
    }

    public void UpdateTile()
    {
        _neighbours.Clear();
        
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

    public void AddWallOnTile()
    {
        Instantiate(_tilePrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
        TileState = TileState.Occupied;
        UpdateTile();
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