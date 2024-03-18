using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int _dimensionIndex;
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Sprite _starFullSprite;

        private Button _button;
        
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => StartCoroutine(UIManager.Instance.GoToScene($"Dimension{_dimensionIndex}")));

            int stars = PlayerPrefs.GetInt($"Dimension{_dimensionIndex}");

            for (int i = 0; i < stars; i++)
            {
                _starsImages[i].sprite = _starFullSprite;
            }
        }
    }
}