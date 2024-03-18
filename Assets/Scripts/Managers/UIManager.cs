using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Slider Menu")]
        [SerializeField] private GameObject _menuObj;
        [SerializeField] private Slider _mainMenuSlider;
        [SerializeField] private Image _imageSlider;
        
        [Space(8)]
        [SerializeField] private GameObject _levelMenuObj;

        protected override void InternalAwake()
        {
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("FirstPlay") == 1)
            {
                _menuObj.SetActive(false);
                _levelMenuObj.SetActive(true);
            }
        }

        public void OpenLevelMenu()
        {
            float value = _mainMenuSlider.value;

            if (value <= 0.1f)
            {
                _menuObj.SetActive(false);
                _levelMenuObj.SetActive(true);
                
                PlayerPrefs.SetInt("FirstPlay", 1);
            }

            _imageSlider.DOFade(value, 0.1f);
        }

        public IEnumerator GoToScene(string nameScene)
        {
            // mets ta transi ici

            float seconds = 0; // mets le temps de ton anim
            yield return new WaitForSeconds(seconds);

            SceneManager.LoadScene(nameScene);
        }
    }
}