using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScrollViewLevels : MonoBehaviour
    {
        [SerializeField] private int _starsMin;
        [SerializeField] private GameObject _padlockObject;
        [SerializeField] private TMP_Text _starsMinText;
        [SerializeField] private LevelButton[] _levels;

        private void Start()
        {
            if (_starsMin > 0)
            {
                int total = PlayerPrefs.GetInt("TotalStars");
                bool canBeSelected = total >= _starsMin;
                
                _starsMinText.text = $"{total} / {_starsMin}";

                Lock(!canBeSelected);
            }
            else
            {
                Lock(false);
            }
        }

        private void Lock(bool isLocked)
        {
            _padlockObject.gameObject.SetActive(isLocked);
            _starsMinText.gameObject.SetActive(isLocked);

            foreach (LevelButton level in _levels)
            {
                level.CanBeSelected = !isLocked;
            }
        }
    }
}