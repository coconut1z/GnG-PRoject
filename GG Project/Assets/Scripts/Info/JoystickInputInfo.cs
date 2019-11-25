using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickInputInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private Dropdown dropdown;
    public Image infoImage;
    public Text infoText;

    public void Start() {
        dropdown = gameObject.GetComponent<Dropdown>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //x-roll
        if (dropdown.value == 0) {
            infoText.text = "The horizontal or left-right movement of the joystick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/joystickX");
        }
        //y-pitch
        if (dropdown.value == 1) {
            infoText.text = "The vertical or forward-back movement of the joystick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/joystickY");
        }
        //z-yaw
        if (dropdown.value == 2) {
            infoText.text = "The twisting left and twisting right movement of the joystick.";
            infoImage.enabled = true;
            infoImage.sprite = Resources.Load<Sprite>("Sprites/joystickZ");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        infoText.text = "";
        infoImage.enabled = false;
    }
}
