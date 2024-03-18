using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private int _dimensionIndex;
        [SerializeField] private Image[] _starsImages;
        [SerializeField] private Sprite _starFullSprite;
        [SerializeField] private TMP_Text _levelName;

        private Button _button;
        
        private void Start()
        {
            string scene = $"Dimension{_dimensionIndex}";
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => StartCoroutine(UIManager.Instance.GoToScene(scene)));

            _levelName.text = $"Dimension {_dimensionIndex}";
            
            int stars = PlayerPrefs.GetInt(scene);
            for (int i = 0; i < stars; i++)
            {
                _starsImages[i].sprite = _starFullSprite;
            }
        }
    }
}