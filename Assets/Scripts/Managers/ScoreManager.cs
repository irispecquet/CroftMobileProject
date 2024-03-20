using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int Score { get; private set; }

        [SerializeField] private int[] _stepsToStars;
        [SerializeField] private int _dimensionIndex;
        
        [Header("End game")]
        [SerializeField] private GameObject _endGameStats;
        [SerializeField] private Image[] _endGameStars;
        [SerializeField] private Sprite _starFullSprite;
        [SerializeField] private TMP_Text _planetText;

        private int _starNb;

        private void Start()
        {
            Score = 0;
        }

        public void UpdateScore(int value)
        {
            Score += value;
        }

        public void ActivateStats(bool activate)
        {
            _endGameStats.SetActive(activate);
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

            for (int i = 0; i < _starNb; i++)
            {
                _endGameStars[i].sprite = _starFullSprite;
                _planetText.text = $"PLANÈTE {_dimensionIndex + 1}";
            }

            int stars = PlayerPrefs.GetInt($"Dimension{_dimensionIndex}Stars");

            if (_starNb > stars)
            {
                PlayerPrefs.SetInt($"Dimension{_dimensionIndex}Stars", _starNb);

                PlayerPrefs.SetInt($"TotalStars", PlayerPrefs.GetInt($"TotalStars") + (_starNb - stars));
            }
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