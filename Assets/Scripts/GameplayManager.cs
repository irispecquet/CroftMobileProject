using System.Collections;
using DG.Tweening;
using UnityEngine;


public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private float _blockSize;

    [Header("Shake")] [SerializeField] private float _effectDuration;
    [SerializeField] private float _shakeStrength;
    [SerializeField] private int _shakeVibrato;
    [SerializeField] private float _shakeRandomness;

    private TileController _currentTile;
    private bool _isDragging = false;
    private Vector3 _initialMousePosition;
    private Vector3 _dragDirection;
    private Vector3 _currentMousePosition;
    private Camera _mainCamera;
    private RaycastHit _initialHit;
    private SpotController _currentSpot;

    protected override void InternalAwake()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isDragging == false)
        {
            InitDrag();
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            _currentMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            EndDrag();
        }
    }

    private void EndDrag()
    {
        _isDragging = false;

        Ray rayCurrentMousePos = _mainCamera.ScreenPointToRay(_currentMousePosition);
        Physics.Raycast(rayCurrentMousePos, out RaycastHit currentHit);

        _dragDirection = (currentHit.point - _initialHit.point).normalized;

        MoveSpot();
    }

    private void InitDrag()
    {
        _initialMousePosition = Input.mousePosition;

        Ray initRay = _mainCamera.ScreenPointToRay(_initialMousePosition);
        Physics.Raycast(initRay, out _initialHit);

        if (_initialHit.collider.gameObject.TryGetComponent(out SpotController spot))
        {
            if (spot != null)
            {
                if (_currentSpot != spot)
                {
                    _currentSpot = spot;
                    _currentTile = spot.CurrentTile;
                }

                _isDragging = true;
            }
        }
    }

    private void MoveSpot()
    {
        if (Physics.Raycast(_currentTile.transform.position, _dragDirection, out RaycastHit hit, _blockSize))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (_currentTile.ContainsTile(tile) && tile.TileState == TileState.Free || tile.TileState == TileState.HasInteractable)
                {
                    _currentSpot.Move(tile);

                    _currentTile.CurrentSpot = null;

                    _currentTile = tile;
                    tile.CurrentSpot = _currentSpot;

                    StartCoroutine(Interact(tile, tile.Interactable, _dragDirection, false));
                }
                else
                {
                    ShakeSpot();
                }
            }
            else
            {
                ShakeSpot();
            }
        }
        else
        {
            ShakeSpot();
        }
    }

    private IEnumerator Interact(TileController tile, IInteractable interactable, Vector3 direction, bool IsARebound)
    {
        if (interactable == null && IsARebound == false)
        {
            yield break;
        }

        if (Physics.Raycast(_currentSpot.PartnerSpot.CurrentTile.transform.position, direction, out RaycastHit neighbourHit, _blockSize))
        {
            if (neighbourHit.collider.gameObject.TryGetComponent(out TileController newTile))
            {
                if (_currentSpot.PartnerSpot.CurrentTile.ContainsTile(newTile))
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

                    StartCoroutine(WaitToMoveInteractable(_currentSpot.PartnerSpot, tile, newTile, interactable, direction));
                }
            }
        }
        else
        {
            DestroyInteractable(tile);
        }
    }

    private bool IsAFakeRebound(TileController tile, IInteractable interactable, Vector3 direction, bool IsARebound, TileController newTile)
    {
        if (newTile.TileState == TileState.HasAWall && IsARebound)
        {
            if (Physics.Raycast(_currentSpot.CurrentTile.transform.position, -direction, out RaycastHit reboundHit, _blockSize))
            {
                if (reboundHit.collider.gameObject.TryGetComponent(out TileController fakeTile))
                {
                    if (_currentSpot.CurrentTile.ContainsTile(fakeTile))
                    {
                        StartCoroutine(WaitToMoveInteractable(_currentSpot, tile, fakeTile, interactable, -direction));
                    }
                }
                else
                {
                    DestroyInteractable(tile);
                }
            }
            else
            {
                DestroyInteractable(tile);
            }

            return true;
        }

        return false;
    }

    private IEnumerator WaitToMoveInteractable(SpotController spot, TileController tile, TileController newTile, IInteractable interactable, Vector3 direction)
    {
        interactable?.Move(spot.transform.position, newTile.SpotPositionTransform.position);
        newTile.SetInteractable(interactable);
        tile.RemoveInteractable();

        yield return new WaitForSeconds(interactable.GetTweenDuration());

        if (newTile.CurrentSpot != null)
        {
            _currentSpot = newTile.CurrentSpot;
            StartCoroutine(Interact(newTile, newTile.Interactable, direction, false));
        }

        _currentTile = tile;
    }

    private static void DestroyInteractable(TileController tile)
    {
        IInteractable interactable = tile.Interactable;
        tile.RemoveInteractable();
        interactable.Destroy();
    }

    private void ShakeSpot()
    {
        _currentSpot.transform.DOShakePosition(_effectDuration, new Vector3(_shakeStrength, 0, 0), _shakeVibrato, _shakeRandomness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}