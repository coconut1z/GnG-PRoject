using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerInputInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Dropdown dropdown;
    public Image infoImage;
    public Text infoText;

    public void Start() {
        dropdown = gameObject.GetComponent<Dropdown>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //x
        if (dropdown.value == 0) {
            infoText.text = "The horizontal or left-right movement of the left analog stick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/controllerX");
        }
        //y
        if (dropdown.value == 1) {
            infoText.text = "The vertical or up-down movement of the left analog stick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/controllerY");
        }
        //z
        if (dropdown.value == 2) {
            infoText.text = "The horizontal or left-right movement of the right analog stick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/controllerZ");
        }
        //rz
        if (dropdown.value == 3) {
            infoText.text = "The vertical or up-down movement of the right analog stick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/controllerRZ");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        infoText.text = "";
        infoImage.enabled = false;
    }
}
