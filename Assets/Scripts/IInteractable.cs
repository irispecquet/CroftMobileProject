
using UnityEngine;

public interface IInteractable
{
    void Execute();

    void Move(Vector3 startPos, Vector3 endPos);
    void Destroy();
    float GetTweenDuration();
}