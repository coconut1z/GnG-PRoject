using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour {

    public GameObject instructionsMenu, mainMenu;
    public GameObject realtimeCamera;
    public GameObject pitchPanel, yawPanel, rollPanel, throttlePanel, checkpoint1, checkpoint2;
    public Button back, prev, next;
    public Image checkpoint;
    public Sprite good, bad;
    public GameObject ship;
    public Text page;
    private int count;
    private Vector3 zero, one, start;
    private IEnumerator coroutine;
    
    void Start() {
        coroutine = Switch(5f);
        StartCoroutine(coroutine);
        StartCoroutine(RotateShip(0f));
        realtimeCamera.SetActive(true);
        pitchPanel.SetActive(true);
        yawPanel.SetActive(false);
        rollPanel.SetActive(false);
        throttlePanel.SetActive(false);
        checkpoint1.SetActive(false);
        checkpoint2.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        prev.interactable = false;
        checkpoint.sprite = good;
        zero = new Vector3(0, 0, 0);
        one = new Vector3(1, 1, 1);
        start = ship.transform.rotation.eulerAngles;
        count = 1;

        instructionsMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);        
    }

    void Update() {
        page.text = count + " / 6";
        if (pitchPanel.activeSelf) {
            prev.interactable = false;
            next.interactable = true;
            realtimeCamera.SetActive(true);
            Pitch();
        }
        if (yawPanel.activeSelf) {
            prev.interactable = true;
            next.interactable = true;
            realtimeCamera.SetActive(true);
        }
        if (rollPanel.activeSelf) {
            prev.interactable = true;
            next.interactable = true;
            realtimeCamera.SetActive(true);
        }
        if (throttlePanel.activeSelf) {
            prev.interactable = true;
            next.interactable = true;
            realtimeCamera.SetActive(false);
        }
        if (checkpoint1.activeSelf) {
            prev.interactable = true;
            next.interactable = true;
            realtimeCamera.SetActive(false);          
        }
        if (checkpoint2.GetComponent<RectTransform>().localScale == one) {
            prev.interactable = true;
            next.interactable = false;
            realtimeCamera.SetActive(false);              
        }
    }

    public void Pitch() {
        float angle = Mathf.PingPong(Time.unscaledTime * 20f, 30f) - 15f;
        Vector3 temp = ship.transform.eulerAngles;
        temp.x = angle;
        ship.transform.eulerAngles = temp;
    }

    public void Yaw() {
        float angle = Mathf.PingPong(Time.unscaledTime * 10f, 30f) - 15f;
        Vector3 temp = ship.transform.eulerAngles;
        temp.y = angle;
        ship.transform.eulerAngles = temp;
    }

    public void Roll() {
        float angle = Mathf.PingPong(Time.unscaledTime * 20f, 30f) - 15f;
        Vector3 temp = ship.transform.eulerAngles;
        temp.z = angle;
        ship.transform.eulerAngles = temp;
    }

    public IEnumerator RotateShip(float waitTime) {
        while (true) {
            if (pitchPanel.activeSelf) {
                Pitch();
            }
            if (yawPanel.activeSelf) {
                Yaw();
            }
            if (rollPanel.activeSelf) {
                Roll();
            }
            yield return new WaitForSecondsRealtime(waitTime);
        }        
    }

    public IEnumerator Switch(float waitTime) {
        while (true) {
            if (checkpoint.sprite.name == "90") {
                checkpoint.sprite = bad;
            }
            else if (checkpoint.sprite.name == "60") {
                checkpoint.sprite = good;
            }
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    public void Back() {
        instructionsMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        mainMenu.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void Prev() {
        count--;
        ship.transform.localEulerAngles = start;
        if (yawPanel.activeSelf) {
            yawPanel.SetActive(false);
            pitchPanel.SetActive(true);
            return;
        }
        if (rollPanel.activeSelf) {
            rollPanel.SetActive(false);
            yawPanel.SetActive(true);
            return;
        }
        if (throttlePanel.activeSelf) {
            throttlePanel.SetActive(false);
            rollPanel.SetActive(true);
            return;
        }
        if (checkpoint1.activeSelf) {
            checkpoint1.SetActive(false);
            throttlePanel.SetActive(true);
            return;
        }
        if (checkpoint2.GetComponent<RectTransform>().localScale == one) {
            checkpoint2.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            checkpoint1.SetActive(true);
            return;
        }
    }

    public void Next() {
        count++;
        ship.transform.localEulerAngles = start;
        if (pitchPanel.activeSelf) {
            pitchPanel.SetActive(false);
            yawPanel.SetActive(true);
            return;
        }
        if (yawPanel.activeSelf) {
            yawPanel.SetActive(false);
            rollPanel.SetActive(true);
            return;
        }
        if (rollPanel.activeSelf) {
            rollPanel.SetActive(false);
            throttlePanel.SetActive(true);
            return;
        }
        if (throttlePanel.activeSelf) {
            throttlePanel.SetActive(false);
            checkpoint1.SetActive(true);
            return;
        }
        if (checkpoint1.activeSelf) {
            checkpoint1.SetActive(false);
            checkpoint2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            return;
        }
    }
}
