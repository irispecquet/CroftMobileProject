using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _testgizmo;
    [SerializeField] private Transform _rayStart;
    [SerializeField] private Rigidbody _rigidbody;

    protected bool _isGoingToBreak;
    protected bool _isFalling;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_rayStart.position, _rayStart.position + _testgizmo * Vector3.down);
    }

    private void Start()
    {
        _rigidbody.isKinematic = true;

        SetInteractableOnTile();
    }

    protected void SetInteractableOnTile()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GameplayManager.Instance.BlockSize / 2))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    transform.position = tile.SpotPositionTransform.position;

                    tile.SetInteractable(this);
                }
            }
        }
    }

    private bool IsDetectingTile()
    {
        if (Physics.Raycast(_rayStart.position, Vector3.down, out RaycastHit hit, _testgizmo))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                return true;
            }
        }

        return false;
    }

    public void Move(Vector3 startPos, Vector3 endPos)
    {
        transform.position = startPos;
        transform.DOJump(endPos, _jumpForce, 1, _jumpDuration);
    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_jumpDuration);

        _isGoingToBreak = !IsDetectingTile();
        _isFalling = true;
        _rigidbody.isKinematic = false;

        if (_isFalling)
        {
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }

    public float GetTweenDuration()
    {
        return _jumpDuration;
    }
}