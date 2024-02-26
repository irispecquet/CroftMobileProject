using DG.Tweening;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    [field: SerializeField] public SpotController PartnerSpot { get; private set; }
    public TileController CurrentTile { get; private set; }

    [SerializeField] private float _speed;

    private TileController _startingTile;
    private Tween _moveTween;

    private void Start()
    {
        UpdateSpot();
    }

    public void UpdateSpot()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    _startingTile = tile;

                    CurrentTile = _startingTile;

                    transform.position = CurrentTile.PlayerPositionTransform.position;
                    CurrentTile.TileState = TileState.Occupied;
                }
                else
                {
                    Debug.LogError($"The tile {gameObject.name} has already a wall, you can't place your spot here.");
                }
            }
        }
        else
        {
            Debug.LogError("You have to put the spot above a tile.");
        }
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