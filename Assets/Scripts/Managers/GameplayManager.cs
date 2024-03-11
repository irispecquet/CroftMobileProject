using System.Collections;
using DG.Tweening;
using UnityEngine;


public class GameplayManager : Singleton<GameplayManager>
{
    public TileController CurrentTile { get; set; }
    public SpotController CurrentSpot { get; set; }
    public float BlockSize => _blockSize;

    [SerializeField] private FeedbackManager _feedbackManager;
    [SerializeField] private float _blockSize;

    protected override void InternalAwake()
    {
    }
    
    public void MoveSpot(Vector3 direction)
    {
        TileController newTile = CurrentTile.GetNeighbour(direction);

        if (newTile != null)
        {
            if (newTile.TileState == TileState.Free || newTile.TileState == TileState.HasInteractable)
            {
                CurrentSpot.Move(newTile);

                CurrentTile.CurrentSpot = null;

                CurrentTile = newTile;
                newTile.CurrentSpot = CurrentSpot;

                StartCoroutine(Interact(newTile, newTile.Interactable, direction, false));
            }
            else
            {
                _feedbackManager.ShakeSpot(CurrentSpot);
            }
        }
        else
        {
            _feedbackManager.ShakeSpot(CurrentSpot);
        }
    }

    private IEnumerator Interact(TileController tile, Interactable interactable, Vector3 direction, bool IsARebound)
    {
        if (interactable == null && IsARebound == false)
        {
            yield break;
        }
        
        TileController newTile = CurrentSpot.PartnerSpot.CurrentTile.GetNeighbour(direction);

        if (newTile != null)
        {
            if (newTile.TileState == TileState.HasAWall && IsARebound == false)
            {
                StartCoroutine(Interact(tile, tile.Interactable, -direction, true));
                yield break;
            }

            if (IsAFakeRebound(tile, interactable, direction, IsARebound, newTile))
            {
                yield break;
            }

            StartCoroutine(WaitToMoveInteractable(CurrentSpot.PartnerSpot, tile, newTile, interactable, direction));
        }
        else
        {
            DestroyInteractable(tile, direction);
        }
    }

    private bool IsAFakeRebound(TileController tile, Interactable interactable, Vector3 direction, bool IsARebound, TileController newTile)
    {
        if (newTile.TileState == TileState.HasAWall && IsARebound)
        {
            TileController fakeTile = CurrentSpot.CurrentTile.GetNeighbour(-direction);

            if (fakeTile != null)
            {
                StartCoroutine(WaitToMoveInteractable(CurrentSpot, tile, fakeTile, interactable, -direction));
            }
            else
            {
                DestroyInteractable(tile, direction);
            }

            return true;
        }

        return false;
    }

    private IEnumerator WaitToMoveInteractable(SpotController spot, TileController tile, TileController newTile, Interactable interactable, Vector3 direction)
    {
        interactable?.Move(spot.transform.position, newTile.SpotPositionTransform.position);
        newTile.SetInteractable(interactable);
        tile.RemoveInteractable();

        yield return new WaitForSeconds(interactable.GetTweenDuration());

        if (newTile.CurrentSpot != null)
        {
            CurrentSpot = newTile.CurrentSpot;
            StartCoroutine(Interact(newTile, newTile.Interactable, direction, false));
        }

        CurrentTile = tile;
    }

    private void DestroyInteractable(TileController tile, Vector3 direction)
    {
        Interactable interactable = tile.Interactable;

        Vector3 position = CurrentSpot.PartnerSpot.transform.position;
        interactable.Move(position, position + direction * BlockSize);

        tile.RemoveInteractable();
        StartCoroutine(interactable.Destroy());
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}