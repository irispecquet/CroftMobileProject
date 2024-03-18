using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeSpot : MonoBehaviour
{
    public GameObject Slider;
    private Image image;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var _value = Slider.GetComponent<Slider>().value;
        image.color = new Color(image.color.r, image.color.g, image.color.b, _value);

        if (_value <= 0.2f)
        {
            // Scene suivante
        }
    }
}
