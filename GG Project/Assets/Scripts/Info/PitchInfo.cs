using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PitchInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Text tooltipT;

    public void OnPointerEnter(PointerEventData eventData) {
        tooltipT.text = "The elevation of the nose of the plane up or down";
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipT.text = "";
    }
}
