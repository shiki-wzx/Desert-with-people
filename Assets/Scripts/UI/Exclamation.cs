using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exclamation : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canblinking=false;
    private Image blinkImage;
    private bool downAlpha=true;
    void Start()
    {
        blinkImage = GetComponent<Image>();
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
           if()
        }
    }
}
