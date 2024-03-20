using Managers;
using UnityEngine;

namespace Interactables
{
    public class Reactor : Interactable
    {
        private void OnCollisionEnter(Collision other)
        {
            if (_isDead)
            {
                return;
            }
            
            if (_isGoingToBreak)
            {
                StartCoroutine(TransitionManager.Instance.TransitionScript.Defeat(transform));
                _isDead = true;
            }

            if (_isGoingToBreak == false && _isFalling)
            {
                SetInteractableOnTile();
                _isFalling = false;
            }
        }
    }
}