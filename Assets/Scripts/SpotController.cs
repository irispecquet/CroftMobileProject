using System;
using System.Net.Security;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SpotController : MonoBehaviour
{
    public TileController CurrentTile { get; private set; }

    [SerializeField] private SpotController _partnerSpot;
    [SerializeField] private float _speed;

    private TileController _startingTile;
    private Tween _moveTween;

    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                _startingTile = tile;
            }
        }
        else
        {
            Debug.LogError("You have to put the spot above a tile.");
            return;
        }

        transform.position = _startingTile.PlayerPositionTransform.position;

        CurrentTile = _startingTile;
        CurrentTile.TileState = TileState.Occupied;
    }

    public void Move(TileController tile)
    {
        _moveTween?.Kill();
        transform.DOMove(tile.PlayerPositionTransform.position, _speed);

        CurrentTile.TileState = TileState.Free;

        CurrentTile = tile;
        tile.TileState = TileState.Occupied;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + 0.5f * Vector3.down);
    }
}