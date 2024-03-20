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
        public bool CanBeSelected
        {
            get => _canBeSelected;
            set
            {
                _canBeSelected = value;
                _dimensionImage.sprite = _canBeSelected ? _unlockedSprite : _lockedSprite;
            }
        }

        private bool _canBeSelected;
        
        [Header("References")]
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Image _dimensionImage;
        [SerializeField] private TMP_Text _levelName;
        [Space(10), Header("Values")]
        [SerializeField] private int _dimensionIndex;
        [SerializeField] private Sprite _lockedSprite;
        [SerializeField] private Sprite _unlockedSprite;
        [SerializeField] private Sprite _starFullSprite;

        private Button _button;
        private RectTransform _rectTransform;
        
        private void Start()
        {
            string scene = $"Dimension{_dimensionIndex}";
            
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
            
            _button.onClick.AddListener(ClickOnButton);

            _levelName.text = scene;
            
            int stars = PlayerPrefs.GetInt($"Dimension{_dimensionIndex}Stars");
            for (int i = 0; i < stars; i++)
            {
                _starsImages[i].sprite = _starFullSprite;
            }
        }

        private void ClickOnButton()
        {
            if (CanBeSelected == false)
            {
                return;
            }
            
            _rectTransform.DOPunchScale(LevelMenuManager.Instance.PunchForce, LevelMenuManager.Instance.PunchDuration).OnComplete(() => StartCoroutine(LevelMenuManager.Instance.GoToScene($"Dimension{_dimensionIndex}")));
        }
    }
}