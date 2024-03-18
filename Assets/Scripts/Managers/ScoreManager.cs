using System;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private int[] _stepsToStars;
        [SerializeField] private int _dimensionIndex;

        private int _starNb;

        public void UpdateScore(int value)
        {
            Score += value;
        }

        public void SetStars()
        {
            _starNb = _stepsToStars.Length;

            for (int i = _stepsToStars.Length - 1; i >= 0; i--)
            {
                if (Score > _stepsToStars[i])
                {
                    _starNb--;
                }
            }

            int stars = PlayerPrefs.GetInt($"Dimension{_dimensionIndex}");

            if (_starNb > stars)
            {
                PlayerPrefs.SetInt($"Dimension{_dimensionIndex}", _starNb);
            }
            
            Debug.Log($"You got {_starNb} stars, with {Score} moves.");
        }
    }
}