using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }

    [SerializeField] private Tile _currentTile;
    [SerializeField] private float _blockSize = 1f;

    private bool _isDragging = false;
    private Vector3 _initialMousePosition;
    private Vector3 _dragDirection;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject == PlayerController.gameObject)
                {
                    _isDragging = true;
                    _initialMousePosition = Input.mousePosition;
                }
            }
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            _dragDirection = (Input.mousePosition - _initialMousePosition).normalized;
            _dragDirection = Vector3.ProjectOnPlane(_dragDirection, PlayerController.transform.forward).normalized;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;

            Vector3 finalDirection = ConvertToFourDirections(_dragDirection);

            RaycastHit hit;
            
            if (Physics.Raycast(_currentTile.transform.position, finalDirection,out hit, _blockSize))
            {
                if (hit.collider.gameObject.TryGetComponent(out Tile tile))
                {
                    if (_currentTile.ContainsTile(tile))
                    {
                        PlayerController.Move(tile);
                        _currentTile = tile;
                    }
                }
            }
        }
    }

    Vector3 ConvertToFourDirections(Vector3 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        return new Vector3(absX > absY ? Mathf.Sign(direction.x) * _blockSize : 0, 0, absX > absY ? 0 : Mathf.Sign(direction.y) * _blockSize);
    }
}