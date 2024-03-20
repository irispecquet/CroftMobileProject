using System;
using System.Collections;
using DG.Tweening;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _mainMenuSlider;
    [SerializeField] private Image _imageSlider;
    
    public void OpenLevelMenu()
    {
        StartCoroutine(WaitToOpenLevelMenu());
    }

    private IEnumerator WaitToOpenLevelMenu()
    {
        float value = _mainMenuSlider.value;
        // _imageSlider.DOFade(value, 0.1f);

        if (value <= 0.1f)
        {
            Transitions transi = TransitionManager.Instance.TransitionScript;
            
            transi.TransitionOut();

            yield return new WaitForSeconds(transi.FadeTime);
            
            SceneManager.LoadScene("LevelsMenu");
        }
    }
}
