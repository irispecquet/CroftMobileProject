using System;
using DG.Tweening;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody.isKinematic = true;
        
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    transform.position = tile.SpotPositionTransform.position;
                    
                    tile.SetInteractable(this);
                }
                else
                {
                    Debug.LogError($"The tile {gameObject.name} has already a wall, you can't place your object here.");
                }
            }
        }
        else
        {
            Debug.LogError("You have to put the object above a tile.");
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

    public void Destroy()
    {
        _rigidbody.isKinematic = false;
        Destroy(gameObject, 5);
    }

    public float GetTweenDuration()
    {
        return _jumpDuration;
    }
}