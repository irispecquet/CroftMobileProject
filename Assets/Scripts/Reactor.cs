using UnityEngine;

public class Reactor : Interactable
{
    private void OnCollisionEnter(Collision other)
    {
        if (_isGoingToBreak == false && _isFalling)
        {
            SetInteractableOnTile();
            _isFalling = false;
        }
    }
}