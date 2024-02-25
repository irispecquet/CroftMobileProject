using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float _speed;

    private Tween _moveTween;

    public void Move(Tile tile)
    {
        _moveTween?.Kill();
        transform.DOMove(tile.PlayerPosition.position, _speed);
    }
}