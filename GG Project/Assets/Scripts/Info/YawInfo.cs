using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YawInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text tooltipT;

    public void OnPointerEnter(PointerEventData eventData) {
        tooltipT.text = "The turning of the nose of the plane to the left or right";
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipT.text = "";
    }
}
