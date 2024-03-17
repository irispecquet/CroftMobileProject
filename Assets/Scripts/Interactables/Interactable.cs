using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        [field: SerializeField] public InteractableType Type { get; private set; }

        [Header("Jump")] [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpDuration;

        [Header("Fall")] [SerializeField] private Transform _rayStart;
        [SerializeField] private float _rayFallDistance;
        [SerializeField] private float _maxFallDistance;
        [SerializeField] private float _maxTimerBeforeDestroy;

        [Space(10), Header("References")] [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField] private LayerMask _tileLayer;

        protected bool _isGoingToBreak;
        protected bool _isFalling;

        private float _timer;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_rayStart.position, _rayStart.position + _rayFallDistance * Vector3.down);
        }

        private void Start()
        {
            _rigidbody.isKinematic = true;

            SetInteractableOnTile();
        }

        private void Update()
        {
            if (_isFalling)
            {
                _timer += Time.deltaTime;

                if (_timer > _maxTimerBeforeDestroy)
                {
                    Debug.Log("DEAD");
                    Destroy(gameObject);
                }
            }
            else
            {
                _timer = 0;
            }
        }

        public void SetInteractableOnTile()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GameplayManager.Instance.BlockSize / 2, _tileLayer))
            {
                if (hit.collider.gameObject.TryGetComponent(out TileController tile))
                {
                    HandleInteractOnFreeTile(tile);
                    HandleInteractOnSpot(tile);
                    HandleInteractOnInteractable(tile);
                }
            }
        }

        private void HandleInteractOnFreeTile(TileController tile)
        {
            if (tile.TileState == TileState.Free)
            {
                transform.position = tile.SpotPositionTransform.position;

                tile.SetInteractable(this);
            }
        }

        private void HandleInteractOnSpot(TileController tile)
        {
            if (tile.TileState == TileState.HasASpot)
            {
                if (tile.CurrentSpot.Type == SpotType.NextLevelTrigger)
                {
                    GameplayManager.Instance.GoToNextScene(); 
                    return;
                }
                
                GameplayManager.Instance.CurrentSpot = tile.CurrentSpot;

                tile.SetInteractable(this);

                StartCoroutine(GameplayManager.Instance.Interact(tile.CurrentSpot.CurrentTile, this, GameplayManager.Instance.LastDragPos, false));
            }
        }

        private void HandleInteractOnInteractable(TileController tile)
        {
            if (tile.TileState == TileState.HasInteractable)
            {
                if (tile.Interactable.Type == InteractableType.Pouffe)
                {
                    TileController newTile = tile.GetNeighbour(GameplayManager.Instance.LastDragPos);

                    if (newTile != null)
                    {
                        Move(transform.position, newTile.SpotPositionTransform.position);
                        newTile.SetInteractable(this);
                    }
                }
            }
        }

        private bool IsDetectingTile()
        {
            if (Physics.Raycast(_rayStart.position, Vector3.down, out RaycastHit hit, _rayFallDistance, _tileLayer))
            {
                if (hit.collider.gameObject.TryGetComponent(out TileController tile))
                {
                    return hit.distance < _maxFallDistance || tile.TileState == TileState.HasASpot || (tile.TileState == TileState.HasInteractable && tile.Interactable.Type == InteractableType.Pouffe);
                }
            }

            return false;
        }

        public void Move(Vector3 startPos, Vector3 endPos)
        {
            transform.position = startPos;
            transform.DOJump(endPos, _jumpForce, 1, _jumpDuration);
        }

        public IEnumerator Fall()
        {
            yield return new WaitForSeconds(_jumpDuration);

            _isGoingToBreak = !IsDetectingTile();
            _isFalling = true;
            _rigidbody.isKinematic = false;
        }

        public float GetTweenDuration()
        {
            return _jumpDuration;
        }
    }
}