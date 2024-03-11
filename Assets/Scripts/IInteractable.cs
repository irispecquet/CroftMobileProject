
using System.Collections;
using UnityEngine;

public interface IInteractable
{
    void Execute();

    void Move(Vector3 startPos, Vector3 endPos);
    IEnumerator Destroy();
    float GetTweenDuration();
}