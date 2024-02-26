using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class SpotController : MonoBehaviour
{
    public TileController CurrentTile { get; private set; }
    
    [SerializeField] private TileController _startingTile;
    [SerializeField] private float _speed;

    private Tween _moveTween;

    private void Start()
    {
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


}