using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private Rigidbody _rigidbody;

    private bool _isFalling;
    private bool _isOnTile;
    
    private void Start()
    {
        _rigidbody.isKinematic = true;

        SetReactorOnTile();
    }

    private void SetReactorOnTile()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    transform.position = tile.SpotPositionTransform.position;

                    tile.SetInteractable(this);
                    
                    _isOnTile = true;
                    _isFalling = false;
                }
            }
        }
    }

    private void Update()
    {
        if (_isFalling)
        {
            SetReactorOnTile();
        }
    }

    public void Execute()
    {
        
    }

    public void Move(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;
        transform.DOJump(endPos, _jumpForce, 1, _jumpDuration);
    }

    public IEnumerator Destroy()
    {
        _isOnTile = false;
        
        yield return new WaitForSeconds(_jumpDuration);

        _rigidbody.isKinematic = false;
        _isFalling = true;
        
        yield return new WaitForSeconds(5);

        if (_isOnTile == false)
        {
            Destroy(gameObject);
        }
    }

    public float GetTweenDuration()
    {
        return _jumpDuration;
    }
}