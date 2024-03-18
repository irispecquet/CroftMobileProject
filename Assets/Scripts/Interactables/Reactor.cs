using Managers;
using UnityEngine;

namespace Interactables
{
    public class Reactor : Interactable
    {
        private void OnCollisionEnter(Collision other)
        {
            if (_isGoingToBreak)
            {
                // fonction explosion
                
                GameplayManager.Instance.ReloadScene();
            }
        
            if (_isGoingToBreak == false && _isFalling)
            {
                SetInteractableOnTile();
                _isFalling = false;
            }
        }
    }
}