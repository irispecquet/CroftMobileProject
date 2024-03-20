using Audio;
using DG.Tweening;
using Managers;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    [field: SerializeField] public SpotType Type { get; private set; }
    [field: SerializeField] public SpotController PartnerSpot { get; private set; }
    public TileController CurrentTile { get; private set; }

    [SerializeField] private float _speed;

    private TileController _startingTile;
    private Tween _moveTween;

    private void Start()
    {
        UpdateSpot();
    }

    private void UpdateSpot()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GameplayManager.Instance.BlockSize / 2))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    _startingTile = tile;

                    CurrentTile = _startingTile;
                    CurrentTile.CurrentSpot = this;

                    transform.position = CurrentTile.SpotPositionTransform.position;
                    CurrentTile.TileState = TileState.HasASpot;
                }
            }
        }
        else
        {
            Debug.LogError($"You have to put the {gameObject.name} above a tile.");
        }
    }

    public void Move(TileController tile)
    {
        AudioManager.Instance.PlaySound("Move");
        
        _moveTween?.Kill();
        transform.DOMove(tile.SpotPositionTransform.position, _speed);

        CurrentTile.TileState = TileState.Free;

        CurrentTile = tile;
        tile.TileState = TileState.HasASpot;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + 0.5f * Vector3.down);
    }
}

public enum SpotType
{
    Normal,
    NextLevelTrigger
}