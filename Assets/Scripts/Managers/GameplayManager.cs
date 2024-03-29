﻿using System;
using System.Collections;
using Interactables;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameplayManager : Singleton<GameplayManager>
    {
        [field: SerializeField] public FeedbackManager FeedbackManager { get; private set; }
        [field: SerializeField] public ScoreManager ScoreManager { get; private set; }
        
        public TileController CurrentTile { get; set; }
        public SpotController CurrentSpot { get; set; }
        public Vector3 LastDragPos { get; set; }
        public float BlockSize => _blockSize;

        [SerializeField] private float _blockSize;

        protected override void InternalAwake()
        {
        }

        private void Start()
        {
            FeedbackManager.ActivateCanvas(false);
            ScoreManager.ActivateStats(false);
            
            TransitionManager.Instance.TransitionScript.TransitionIn();
        }

        public void MoveSpot(Vector3 direction)
        {
            TileController newTile = CurrentTile.GetNeighbour(direction);

            if (newTile != null)
            {
                if (newTile.TileState == TileState.Free || newTile.TileState == TileState.HasInteractable)
                {
                    CurrentSpot.Move(newTile);

                    ScoreManager.UpdateScore(1);

                    CurrentTile.CurrentSpot = null;

                    CurrentTile = newTile;
                    newTile.CurrentSpot = CurrentSpot;

                    StartCoroutine(Interact(newTile, newTile.Interactable, direction, false));
                }
                else
                {
                    FeedbackManager.ShakeSpot(CurrentSpot);
                }
            }
            else
            {
                FeedbackManager.ShakeSpot(CurrentSpot);
            }
        }

        public IEnumerator Interact(TileController tile, Interactable interactable, Vector3 direction, bool IsARebound)
        {
            if (interactable == null && IsARebound == false)
            {
                yield break;
            }

            TileController newTile = CurrentSpot.PartnerSpot.CurrentTile.GetNeighbour(direction);
            LastDragPos = direction;

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

            yield return new WaitForSeconds(interactable.GetTweenDuration());

            interactable.SetInteractableOnTile();

            tile.RemoveInteractable();
        }

        private void DestroyInteractable(TileController tile, Vector3 direction)
        {
            Interactable interactable = tile.Interactable;

            Vector3 position = CurrentSpot.PartnerSpot.transform.position;
            interactable.Move(position, position + direction * BlockSize);

            tile.RemoveInteractable();
            StartCoroutine(interactable.Fall());
        }

        public void ReloadScene()
        {
            StartCoroutine(WaitToReloadScene());
        }

        private IEnumerator WaitToReloadScene()
        {
            yield return ExitScene();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void EndLevel(bool nextLevel)
        {
            StartCoroutine(WaitToEndLevel(nextLevel));
        }

        public IEnumerator WaitToEndLevel(bool nextLevel)
        {
            yield return ExitScene();

            if (nextLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene("LevelsMenu");
            }
        }

        private object ExitScene()
        {
            FeedbackManager.ActivateCanvas(false);
            ScoreManager.ActivateStats(false);

            Transitions transi = TransitionManager.Instance.TransitionScript;
            transi.TransitionOut();

            return new WaitForSeconds(transi.FadeTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
        }
    }
}