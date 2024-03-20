using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
//using UnityEngine.VFX;
//using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using Managers;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Transitions : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float _fadeTime = 1;
    [SerializeField] private float _beamTime = 3;

    [Header("Object")]
    [SerializeField] private GameObject _beamFx;
    [SerializeField] private Material _fader;

    [Space(10)]
    [SerializeField] private Volume _postPro;
    
    private PaniniProjection _paniniProj;
    private float _paniniDistance;
    private float _paniniCrop;
    
    private const float FadeMax = 0.55f;
    private const float FadeMin = -0.3f;

    public float FadeTime => _fadeTime;

    private void Awake()
    {
        _postPro.profile.TryGet(out _paniniProj);

        _paniniDistance = (float)_paniniProj.distance;
        _paniniCrop = (float)_paniniProj.cropToFit;
    }

    public void TransitionOut()
    {
        _fader.DOFloat(FadeMin, "_PowerLevel", FadeTime);
    }

    public void TransitionIn()
    {
        // _paniniProj.distance.Override(_paniniDistance);
        _fader.DOFloat(FadeMax, "_PowerLevel", FadeTime);
    }

    public IEnumerator Defeat(Transform reactor)
    {
        Instantiate(_beamFx, reactor);

        AudioManager.Instance.PlaySound("Beam");
        // _paniniProj.distance.Override(1);
        
        yield return new WaitForSeconds(_beamTime);

        TransitionOut();
        
        yield return new WaitForSeconds(FadeTime);
        
        GameplayManager.Instance.ReloadScene();
    }

}
