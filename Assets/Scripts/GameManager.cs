using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _blockSize = 1f;

    private TileController _currentTile;
    private bool _isDragging = false;
    private Vector3 _initialMousePosition;
    private Vector3 _dragDirection;
    private Vector3 _currentMousePosition;
    private Camera _mainCamera;
    private RaycastHit _initialHit;
    private SpotController _currentSpot;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isDragging == false)
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
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            _currentMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;

            Ray rayCurrentMousePos = _mainCamera.ScreenPointToRay(_currentMousePosition);
            Physics.Raycast(rayCurrentMousePos, out RaycastHit currentHit);

            _dragDirection = (currentHit.point - _initialHit.point).normalized;

            if (Physics.Raycast(_currentTile.transform.position, _dragDirection, out RaycastHit hit, _blockSize))
            {
                if (hit.collider.gameObject.TryGetComponent(out TileController tile))
                {
                    if (_currentTile.ContainsTile(tile) && tile.TileState == TileState.Free)
                    {
                        _currentSpot.Move(tile);
                        _currentTile = tile;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}