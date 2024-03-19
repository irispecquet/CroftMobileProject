using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _mainMenuSlider;
    [SerializeField] private Image _imageSlider;
    [SerializeField] private GameObject _levelSelection;
    [SerializeField] private GameObject _self;

    public void OpenLevelMenu()
    {
        float value = _mainMenuSlider.value;
        _imageSlider.DOFade(value, 0.1f);

        if (value <= 0.1f)
        {
            _levelSelection.SetActive(true);
            _self.SetActive(false);
            //SceneManager.LoadScene("FirstMenu"); 
        }
    }
}
