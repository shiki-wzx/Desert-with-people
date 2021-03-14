using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExclamationCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canblinking;
    private CanvasGroup blinkImage;
    private bool downAlpha = true;
    public float t;
    public float finalAlpha;
    void Start()
    {
        blinkImage = GetComponent<CanvasGroup>();
        canblinking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canblinking)
        {
            if (1 - blinkImage.alpha < 0.05)
            {
                downAlpha = true;
            }
            if (blinkImage.alpha - 0.3 < 0.05)
            {
                downAlpha = false;
            }
            if (downAlpha)
            {
                blinkImage.alpha =  Mathf.Lerp(blinkImage.alpha,0.3f, t * Time.deltaTime);
            }
            else
            {
                blinkImage.alpha =  Mathf.Lerp(blinkImage.alpha, 1, t * Time.deltaTime);
            }
        }
    }
}
