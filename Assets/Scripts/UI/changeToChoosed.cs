using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class changeToChoosed : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    public GameObject canActiveUI;
    public float smallScale;
    private void OnMouseEnter()
    {
        canActiveUI.SetActive(true);
    }
    private void OnMouseOver()
    {
        canActiveUI.SetActive(true);
    }
    private void OnMouseExit()
    {
        canActiveUI.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canActiveUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canActiveUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector3(smallScale,smallScale,smallScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
