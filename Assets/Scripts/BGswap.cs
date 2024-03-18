using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGswap : MonoBehaviour
{
    public List<Sprite> Images = new List<Sprite>();

    Image image;

    float timer;
    int listCnt = 0;

    public float Limit;


    private void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = Images[0];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > Limit) 
        {
            timer = 0;
            listCnt++;

            if (listCnt >= Images.Count)
            {
                listCnt = 0;
            }

            image.sprite = Images[listCnt];
        }
    }

}
