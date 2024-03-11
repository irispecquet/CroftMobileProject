using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [field: SerializeField] public Transform SpotPositionTransform { get; private set; }
    public SpotController CurrentSpot { get; set; }
    public Interactable Interactable { get; private set; }
    public TileState TileState { get; set; }
    public TileController Wall { get; private set; }

    [SerializeField] private float _rayDistance;
    [SerializeField] private TileController _tilePrefab;

    private Dictionary<Vector3, TileController> _neighbours = new Dictionary<Vector3, TileController>(4);
    private Vector3[] _directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

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
                    _neighbours.Add(direction, tile);
                }
            }
        }

        if (Wall != null)
        {
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitUp, _rayDistance))
        {
            if (hitUp.collider.gameObject.TryGetComponent(out TileController tile))
            {
                Wall = tile;
                TileState = TileState.HasAWall;
            }
        }
    }

    public void AddWallOnTile()
    {
        if (TileState != TileState.Free || Wall != null)
        {
            return;
        }

        TileController wall = Instantiate(_tilePrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);

        Wall = wall;
        TileState = TileState.HasAWall;

        wall.UpdateTile();
        UpdateTile();
    }

    public void RemoveWall()
    {
        if (Wall != null)
        {
            DestroyImmediate(Wall.gameObject);
            Wall = null;
            TileState = TileState.Free;
            UpdateTile();
        }
    }

    public TileController GetNeighbour(Vector3 direction)
    {
        return _neighbours.ContainsKey(direction) == false ? null : _neighbours[direction];
    }

    public void SetInteractable(Interactable interactable)
    {
        Interactable = interactable;
        TileState = TileState.HasInteractable;
    }

    public void RemoveInteractable()
    {
        Interactable = null;
        TileState = TileState.HasASpot;
    }

    public TileController GetHighestWall()
    {
        if (Wall != null)
        {
            Debug.Log($"Tile {gameObject.name} has a wall named {Wall.gameObject.name}");
            return Wall.GetHighestWall();
        }

        return this;
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
    HasASpot,
    HasInteractable,
    HasAWall
}