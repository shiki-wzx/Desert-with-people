using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasGroup blackimage;
    public bool toblack;
    public GameObject Ed1;
    public GameObject Ed2;
    public float Speed;
    void Start()
    {
        Speed = 0.5f;
        StartCoroutine(EndUI());
    }

    // Update is called once per frame
    void Update()
    {
        if(toblack)
        {
            Debug.Log("black");
            if(blackimage.alpha<1)
            blackimage.alpha+=Speed* Time.deltaTime ;
        }
        else
        {
            Debug.Log("white");
            if (blackimage.alpha > 0)
                blackimage.alpha -=Speed* Time.deltaTime;
        }
    }
    IEnumerator  EndUI()
    {
        toblack = false;
        Ed1.SetActive(true);
        yield return new WaitForSeconds(6f);
        Speed = 1;
        toblack = true;
        yield return new WaitForSeconds(1f);
        Ed1.SetActive(false);
        Ed2.SetActive(true);
        toblack = false;

    }
}
