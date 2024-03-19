using System;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int _dimensionIndex;
        [SerializeField] private int _starsMin;
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Sprite _starFullSprite;
        [SerializeField] private TMP_Text _levelName;

        private Button _button;
        private RectTransform _rectTransform;
        
        private void Start()
        {
            string scene = $"Dimension{_dimensionIndex}";
            
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            
            _button.onClick.AddListener(ClickOnButton);

            _levelName.text = $"Dimension {_dimensionIndex}";
            
            int stars = PlayerPrefs.GetInt(scene);
            for (int i = 0; i < stars; i++)
            {
                _starsImages[i].sprite = _starFullSprite;
            }
        }

        private void ClickOnButton()
        {
            _rectTransform.DOPunchScale(LevelMenuManager.Instance.PunchForce, LevelMenuManager.Instance.PunchDuration).OnComplete(() => StartCoroutine(LevelMenuManager.Instance.GoToScene($"Dimension{_dimensionIndex}")));
        }
    }
}