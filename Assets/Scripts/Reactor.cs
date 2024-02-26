using System;
using UnityEngine;

public class Reactor : MonoBehaviour, IInteractable
{
    private TileController _tile;
    
    private void Start()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.TryGetComponent(out TileController tile))
            {
                if (tile.TileState == TileState.Free)
                {
                    transform.position = tile.PlayerPositionTransform.position;
                    
                    tile.SetInteractable(this);
                    
                    _tile = tile;
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

    public void Move(TileController newTile)
    {
        transform.position = newTile.PlayerPositionTransform.position;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}