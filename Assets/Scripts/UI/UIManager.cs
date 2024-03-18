using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private GameObject _menuObj;
        [SerializeField] private GameObject _levelMenuObj;
        [SerializeField] private Slider _mainMenuSlider;

        protected override void InternalAwake()
        {
        }
        
        public void OpenLevelMenu()
        {
            if (_mainMenuSlider.value >= 0.9f)
            {
                _menuObj.SetActive(false);
                _levelMenuObj.SetActive(true);
            }
        }

        public void GoToScene(string nameScene)
        {
            SceneManager.LoadScene(nameScene);
        }

    }
}