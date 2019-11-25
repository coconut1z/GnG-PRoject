using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MKInputInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Dropdown dropdown;
    public Image infoImage;
    public Text infoText;

    public void Start() {
        dropdown = gameObject.GetComponent<Dropdown>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (dropdown.value == 0) {
            infoText.text = "The horizontal or left-right movement of the mouse.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/mouseHorizontal");
        }
        if (dropdown.value == 1) {
            infoText.text = "The vertical or up-down movement of the mouse.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/mouseVertical");
        }
        if (dropdown.value == 2) {
            infoText.text = "An alphanumberic or A-Z and 0-9 character on the keyboard.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/keyboard");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        infoText.text = "";
        infoImage.enabled = false;
    }
}
