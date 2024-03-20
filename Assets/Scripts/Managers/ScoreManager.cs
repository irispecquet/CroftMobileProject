using System;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private int[] _stepsToStars;
        [SerializeField] private int _dimensionIndex;

        private int _starNb;

        private void Start()
        {
            Score = 0;
        }

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

            int stars = PlayerPrefs.GetInt($"Dimension{_dimensionIndex}Stars");

            if (_starNb > stars)
            {
                PlayerPrefs.SetInt($"Dimension{_dimensionIndex}Stars", _starNb);

                PlayerPrefs.SetInt($"TotalStars", PlayerPrefs.GetInt($"TotalStars") + (_starNb - stars));
            }

            Debug.Log($"You got {_starNb} stars, with {Score} moves.");
            Debug.Log($"You have {PlayerPrefs.GetInt("TotalStars")} stars in total.");
        }

#if UNITY_EDITOR
        [MenuItem("Custom Menu/ResetPlayerPrefs")]
        private static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }
}