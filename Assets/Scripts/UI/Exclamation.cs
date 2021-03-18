using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exclamation : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canblinking;
    private Image blinkImage;
    private bool downAlpha=true;
    public float t;
    void Start()
    {
        blinkImage = GetComponent<Image>();
        canblinking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canblinking)
        {
            if(255-blinkImage.color.a<3)
            {
                downAlpha = true;
            }
            if(blinkImage.color.a-100<3)
            {
                downAlpha = false;
            }
           if(downAlpha)
            {
                blinkImage.color = new Color(255, 255, 255, Mathf.Lerp(blinkImage.color.a, 100, t*Time.deltaTime));
                //Debug.Log(downAlpha);
            }
            else
            {
                blinkImage.color = new Color(255, 255, 255, Mathf.Lerp(blinkImage.color.a, 255, t * Time.deltaTime));
                //Debug.Log(downAlpha);
            }
        }
    }
}
