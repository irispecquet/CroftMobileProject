using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
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

                    StartCoroutine(Interact(tile));
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

    private IEnumerator Interact(TileController tile)
    {
        IInteractable interactable = tile.Interactable;
        
        if (interactable != null)
        {
            if (Physics.Raycast(_currentSpot.PartnerSpot.CurrentTile.transform.position, _dragDirection, out RaycastHit neighbourHit, _blockSize))
            {
                if (neighbourHit.collider.gameObject.TryGetComponent(out TileController newTile))
                {
                    if (_currentSpot.PartnerSpot.CurrentTile.ContainsTile(newTile))
                    {
                        interactable.Move(_currentSpot.PartnerSpot.transform.position, newTile.SpotPositionTransform.position);
                        newTile.SetInteractable(interactable);

                        tile.RemoveInteractable();

                        yield return new WaitForSeconds(interactable.GetTweenDuration());
                        
                        if (newTile.CurrentSpot != null)
                        {
                            _currentSpot = newTile.CurrentSpot;
                            StartCoroutine(Interact(newTile));
                        }
                    }
                }
            }
            else
            {
                DestroyInteractable(tile);
            }
        }
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