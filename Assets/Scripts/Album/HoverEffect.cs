using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
    }

    public void test()
    {
        Debug.Log("CLIIIICK");
    }
}
//IPointerClickHandler,