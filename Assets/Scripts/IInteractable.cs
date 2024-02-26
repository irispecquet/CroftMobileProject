
using UnityEngine;

public interface IInteractable
{
    void Execute();
    void Move(TileController newTile);
    void Destroy();
}