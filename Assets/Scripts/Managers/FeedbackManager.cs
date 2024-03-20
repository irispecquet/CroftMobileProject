using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class FeedbackManager : MonoBehaviour
    {
        [Header("Shake")]
        [SerializeField] private float _effectDuration;
        [SerializeField] private float _shakeStrength;
        [SerializeField] private int _shakeVibrato;
        [SerializeField] private float _shakeRandomness;
        [Header("UI")]
        [SerializeField] private Image[] _uiImage;

        public void ShakeSpot(SpotController spot)
        {
            spot.transform.DOShakePosition(_effectDuration, new Vector3(_shakeStrength, 0, 0), _shakeVibrato, _shakeRandomness);
        }

        public void ActivateCanvas(bool activate)
        {
            foreach (Image canva in _uiImage)
            {
                if (activate == false)
                {
                    canva.DOFade(0, 0.3f);
                    canva.gameObject.SetActive(false);
                }
                else
                {
                    canva.gameObject.SetActive(true);
                    canva.DOFade(1, 0.3f);
                }
            }
        }
    }
}