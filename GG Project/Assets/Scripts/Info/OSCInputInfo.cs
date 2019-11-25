using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OSCInputInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Dropdown dropdown;
    public Image infoImage;
    public Text infoText;

    public void Start() {
        dropdown = gameObject.GetComponent<Dropdown>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //x-pitch
        if (dropdown.value == 0) {
            infoText.text = "The vertical or forward-back rotaiton of the phone.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/oscX");
        }
        //y-roll
        if (dropdown.value == 1) {
            infoText.text = "The horizontal or left-right rotaiton of the phone.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/oscY");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        infoText.text = "";
        infoImage.enabled = false;
    }
}
