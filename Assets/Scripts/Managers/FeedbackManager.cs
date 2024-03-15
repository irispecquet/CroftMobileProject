using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class FeedbackManager : MonoBehaviour
    {
        [Header("Shake")]
        [SerializeField] private float _effectDuration;
        [SerializeField] private float _shakeStrength;
        [SerializeField] private int _shakeVibrato;
        [SerializeField] private float _shakeRandomness;
    
        public void ShakeSpot(SpotController spot)
        {
            spot.transform.DOShakePosition(_effectDuration, new Vector3(_shakeStrength, 0, 0), _shakeVibrato, _shakeRandomness);
        }
    }
}