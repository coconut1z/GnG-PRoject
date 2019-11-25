using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public ShipInput shipInput;
    public Image mainMenu, settingsMenu;
    public GameObject basicSettings, advSettings, creditsMenu, instructionsMenu;
    public Button basicButton, advancedButton;
    public bool started;
    public Text tooltip;
    public int[] pitch;
    public int[] yaw;
    public int[] roll;
    public int[] throttle;

    #region Basic Settings
    [Header("Basic Settings")]
    public Dropdown selector;
    public GameObject MK, controller, joystick, OSC;
    public Dropdown MKpitch, MKyaw, MKroll, controllerPitch, controllerYaw, controllerRoll,
                    joystickPitch, joystickYaw, joystickRoll, OSCpitch, OSCyaw, OSCroll;
    public InputField MKpitchUp, MKpitchDown, MKrollLeft, MKrollRight, MKyawLeft, MKyawRight, MKthrottleAccel, MKthrottleDecel;
    public Dropdown controllerAccel, controllerDecel, joystickAccel, joystickDecel;
    public Toggle MKpitchT, MKyawT, MKrollT, controllerPitchT, controllerYawT, controllerRollT,
                  joystickPitchT, joystickYawT, joystickRollT, OSCpitchT, OSCrollT;
    #endregion

    #region Advanced Settings
    [Header("Advanced Settings")]
    public int nothinglol;
    public Dropdown AdvMKpitch, AdvMKyaw, AdvMKroll, AdvConPitch, AdvConYaw, AdvConRoll,
                    AdvJoystickPitch, AdvJoystickYaw, AdvJoystickRoll, AdvOSCpitch, AdvOSCyaw, AdvOSCroll;
    public InputField AdvMKpitchUp, AdvMKpitchDown, AdvMKyawLeft, AdvMKyawRight, 
                      AdvMKrollLeft, AdvMKrollRight, AdvMKthrottleAccel, AdvMKthrottleDecel;
    public Dropdown AdvConPitchUp, AdvConPitchDown, AdvConYawLeft, AdvConYawRight, AdvConRollLeft,
                    AdvConRollRight, AdvConAccel, AdvConDecel, AdvJoystickAccel, AdvJoystickDecel, AdvOSCyawLeft,
                    AdvOSCyawRight, AdvOSCrollLeft, AdvOSCrollRight, AdvOscThrottle;
    public Toggle AdvMKpitchT, AdvMKyawT, AdvMKrollT, AdvConPitchT, AdvConYawT, AdvConRollT,
                  AdvJoystickPitchT, AdvJoystickYawT, AdvJoystickRollT, AdvOSCpitchT, AdvOSCyawT, AdvOSCrollT;
    public Text primaryP, primaryY, primaryR, primaryT, secondaryP, secondaryY, secondaryR, secondaryT;
    #endregion

    void Awake() {
        started = false;
        Time.timeScale = 0;        
        Cursor.visible = true;

        SetDefaults();
    }

    void Start() {
        SetDefaults();

        pitch = new int[2];
        yaw = new int[2];
        roll = new int[2];
        throttle = new int[2];

        basicSettings.SetActive(true); //basic settings are the first to show up
        advSettings.SetActive(false);
        creditsMenu.SetActive(false);

        MK.SetActive(true);
        controller.SetActive(false);    //controller and joystick menus are not active, mouse+keyboard is the default
        joystick.SetActive(false);
        advSettings.SetActive(false);
        settingsMenu.transform.gameObject.SetActive(false);
        instructionsMenu.SetActive(true);
        instructionsMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    void Update() {
        PauseAndUnpause();
        UpdateSelector();
        if (!basicButton.interactable) {
            ToggleAllBasicSettings();
            CheckBasicControllerThrottleInput();
            CheckBasicJoystickThrottleInput();
        }
        else {
            ToggleAllAdvSettings();
            AdvConThrottleButtonInput();
            AdvJoystickThrottleButtonInput();
            AdvConPitchButtonInput();
            AdvConYawButtonInput();
            AdvConRollButtonInput();
            SetControlBinds();
        }        
    }

    //called by start button
    public void StartGame() {
        started = true;
        Time.timeScale = 1;
        Cursor.visible = false;        
        mainMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);        
    }

    //called by pressing escape
    public void PauseAndUnpause() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0;
            mainMenu.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            started = false;
        }
        if (!started) {
            Cursor.visible = true;
        }
    }

    //called by restart button
    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void Credits() {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
    }

    public void Instructions() {
        instructionsMenu.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        mainMenu.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);        
    }

    //called by the settings button in main menu
    public void ShowSettings() {
        settingsMenu.transform.gameObject.SetActive(true);
    }

    //called by the back button in settings menu, also does checks to make sure the same axis is not bound to multiple movements
    public void HideSettings() {
        settingsMenu.transform.gameObject.SetActive(false);
        #region Might implement checks
        //M+K checks
        /*if (selector.value == 0) {
            if (MKpitch.value == MKyaw.value || MKpitch.value == MKroll.value || MKyaw.value == MKroll.value) {
                tooltip.text = "Cannot bind multiple movement axis to one mouse axis";
            }
            else if (MKpitchUp.interactable) {
                if (MKpitchUp.text == "" || MKpitchDown.text == "") {
                    tooltip.text = "Missing key bind for pitch";
                }
                else if (MKpitchUp.text == MKpitchDown.text || MKpitchUp.text == MKyawLeft.text || MKpitchUp.text == MKyawRight.text ||
                         MKpitchUp.text == MKrollLeft.text || MKpitchUp.text == MKrollRight.text) {
                    tooltip.text = "Cannot bind multiple movement axis to one key";
                }
            }
            else {
                settingsMenu.transform.gameObject.SetActive(false);
            }
        }
        //controller checks
        else if (selector.value == 1) {

        }
        //joystick checks
        else if (selector.value == 2) {

        }*/
        #endregion
    }

    //toggle between the basic and advanced settings page
    public void ToggleSettings() {
        basicSettings.SetActive(!basicSettings.activeSelf);
        advSettings.SetActive(!advSettings.activeSelf);
        basicButton.interactable = !basicButton.interactable;
        advancedButton.interactable = !advancedButton.interactable;
    }

    //auto capitalise letters
    public void Capitalise(InputField inputField) {
        string input = inputField.text;
        if (input != inputField.text.ToUpper()) {
            inputField.text = inputField.text.ToUpper();
        }
        InputFieldChecks();
    }

    //convert into keycode
    public void InputFieldChecks() {
        if (!basicButton.interactable) {
            //basic MK input fields
            if (MKpitchUp.text != null && MKpitchDown.text != null && MKpitchUp.text != "" && MKpitchDown.text != "") {
                shipInput.MKpitchUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKpitchUp.text);
                shipInput.MKpitchDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKpitchDown.text);
            }
            if (MKyawLeft.text != null && MKyawRight.text != null && MKyawLeft.text != "" && MKyawRight.text != "") {
                shipInput.MKyawLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKyawLeft.text);
                shipInput.MKyawRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKyawRight.text);
            }
            if (MKrollLeft.text != null && MKrollRight.text != null && MKrollLeft.text != "" && MKrollRight.text != "") {
                shipInput.MKrollLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKrollLeft.text);
                shipInput.MKrollRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKrollRight.text);
            }
            if (MKthrottleAccel.text != null && MKthrottleDecel.text != null && MKthrottleAccel.text != "" && MKthrottleDecel.text != "") {
                shipInput.MKaccel = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKthrottleAccel.text);
                shipInput.MKdecel = (KeyCode)System.Enum.Parse(typeof(KeyCode), MKthrottleDecel.text);
            }
        }
        else {
            //advanced MK input fields
            if (AdvMKpitchUp.text != null && AdvMKpitchDown.text != null && AdvMKpitchUp.text != "" && AdvMKpitchDown.text != "") {
                shipInput.MKpitchUp = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKpitchUp.text);
                shipInput.MKpitchDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKpitchDown.text);
            }
            if (AdvMKyawLeft.text != null && AdvMKyawRight.text != null && AdvMKyawLeft.text != "" && AdvMKyawRight.text != "") {
                shipInput.MKyawLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKyawLeft.text);
                shipInput.MKyawRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKyawRight.text);
            }
            if (AdvMKrollLeft.text != null && AdvMKrollRight.text != null && AdvMKrollLeft.text != "" && AdvMKrollRight.text != "") {
                shipInput.MKrollLeft = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKrollLeft.text);
                shipInput.MKrollRight = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKrollRight.text);
            }
            if (AdvMKthrottleAccel.text != null && AdvMKthrottleDecel.text != null && AdvMKthrottleAccel.text != "" && AdvMKthrottleDecel.text != "") {
                shipInput.MKaccel = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKthrottleAccel.text);
                shipInput.MKdecel = (KeyCode)System.Enum.Parse(typeof(KeyCode), AdvMKthrottleDecel.text);
            }
        }
    }

    //0 is square
    //1 is X
    //2 is circle
    //3 is triangle
    //4 is L1
    //5 is R1
    //6 is L2
    //7 is R2
    public void CheckBasicControllerThrottleInput() {
        if (controllerAccel.value == 0)
            shipInput.controllerAccel = KeyCode.JoystickButton0;
        if (controllerAccel.value == 1)
            shipInput.controllerAccel = KeyCode.JoystickButton1;
        if (controllerAccel.value == 2)
            shipInput.controllerAccel = KeyCode.JoystickButton2;
        if (controllerAccel.value == 3)
            shipInput.controllerAccel = KeyCode.JoystickButton3;
        if (controllerAccel.value == 4)
            shipInput.controllerAccel = KeyCode.JoystickButton4;
        if (controllerAccel.value == 5)
            shipInput.controllerAccel = KeyCode.JoystickButton5;
        if (controllerAccel.value == 6)
            shipInput.controllerAccel = KeyCode.JoystickButton6;
        if (controllerAccel.value == 7)
            shipInput.controllerAccel = KeyCode.JoystickButton7;

        if (controllerDecel.value == 0)
            shipInput.controllerDecel = KeyCode.JoystickButton0;
        if (controllerDecel.value == 1)
            shipInput.controllerDecel = KeyCode.JoystickButton1;
        if (controllerDecel.value == 2)
            shipInput.controllerDecel = KeyCode.JoystickButton2;
        if (controllerDecel.value == 3)
            shipInput.controllerDecel = KeyCode.JoystickButton3;
        if (controllerDecel.value == 4)
            shipInput.controllerDecel = KeyCode.JoystickButton4;
        if (controllerDecel.value == 5)
            shipInput.controllerDecel = KeyCode.JoystickButton5;
        if (controllerDecel.value == 6)
            shipInput.controllerDecel = KeyCode.JoystickButton6;
        if (controllerDecel.value == 7)
            shipInput.controllerDecel = KeyCode.JoystickButton7;
    }

    public void CheckBasicJoystickThrottleInput() {
        if (joystickAccel.value == 0)
            shipInput.joystickAccel = KeyCode.JoystickButton0;
        if (joystickAccel.value == 1)
            shipInput.joystickAccel = KeyCode.JoystickButton1;
        if (joystickAccel.value == 2)
            shipInput.joystickAccel = KeyCode.JoystickButton2;
        if (joystickAccel.value == 3)
            shipInput.joystickAccel = KeyCode.JoystickButton3;
        if (joystickAccel.value == 4)
            shipInput.joystickAccel = KeyCode.JoystickButton4;
        if (joystickAccel.value == 5)
            shipInput.joystickAccel = KeyCode.JoystickButton5;
        if (joystickAccel.value == 6)
            shipInput.joystickAccel = KeyCode.JoystickButton6;
        if (joystickAccel.value == 7)
            shipInput.joystickAccel = KeyCode.JoystickButton7;
        if (joystickAccel.value == 8)
            shipInput.joystickAccel = KeyCode.JoystickButton8;
        if (joystickAccel.value == 9)
            shipInput.joystickAccel = KeyCode.JoystickButton9;
        if (joystickAccel.value == 10)
            shipInput.joystickAccel = KeyCode.JoystickButton10;
        if (joystickAccel.value == 11)
            shipInput.joystickAccel = KeyCode.JoystickButton11;

        if (joystickDecel.value == 0)
            shipInput.joystickDecel = KeyCode.JoystickButton0;
        if (joystickDecel.value == 1)
            shipInput.joystickDecel = KeyCode.JoystickButton1;
        if (joystickDecel.value == 2)
            shipInput.joystickDecel = KeyCode.JoystickButton2;
        if (joystickDecel.value == 3)
            shipInput.joystickDecel = KeyCode.JoystickButton3;
        if (joystickDecel.value == 4)
            shipInput.joystickDecel = KeyCode.JoystickButton4;
        if (joystickDecel.value == 5)
            shipInput.joystickDecel = KeyCode.JoystickButton5;
        if (joystickDecel.value == 6)
            shipInput.joystickDecel = KeyCode.JoystickButton6;
        if (joystickDecel.value == 7)
            shipInput.joystickDecel = KeyCode.JoystickButton7;
        if (joystickDecel.value == 8)
            shipInput.joystickDecel = KeyCode.JoystickButton8;
        if (joystickDecel.value == 9)
            shipInput.joystickDecel = KeyCode.JoystickButton9;
        if (joystickDecel.value == 10)
            shipInput.joystickDecel = KeyCode.JoystickButton10;
        if (joystickDecel.value == 11)
            shipInput.joystickDecel = KeyCode.JoystickButton11;
    }

    #region Basic toggles
    //changes the options based on the dropdown that is selected, between the controllers
    public void UpdateSelector() {
        if (selector.value == 0) {
            MK.SetActive(true);
            controller.SetActive(false);
            joystick.SetActive(false);
            OSC.SetActive(false);
        }
        else if (selector.value == 1) {
            MK.SetActive(false);
            controller.SetActive(true);
            joystick.SetActive(false);
            OSC.SetActive(false);
        }
        else if (selector.value == 2) {
            MK.SetActive(false);
            controller.SetActive(false);
            joystick.SetActive(true);
            OSC.SetActive(false);
        }
        else if (selector.value == 3) {
            MK.SetActive(false);
            controller.SetActive(false);
            joystick.SetActive(false);
            OSC.SetActive(true);
        }
    }

    //makes the inputfields and toggle button interactable or not depending if you select mouse axis or keyboard
    public void ToggleBasicMKPitchInput() {
        if (MKpitch.value == 2) {
            MKpitchUp.interactable = true;
            MKpitchDown.interactable = true;
            MKpitchT.interactable = false;
        }
        else {
            MKpitchUp.interactable = false;
            MKpitchDown.interactable = false;
            MKpitchT.interactable = true;
        }
    }

    //makes the inputfield uninteractable when you select a mouse axis, and enables invert toggle
    public void ToggleBasicMKYawInput() {
        if (MKyaw.value == 2) {
            MKyawLeft.interactable = true;
            MKyawRight.interactable = true;
            MKyawT.interactable = false;
        }
        else {
            MKyawLeft.interactable = false;
            MKyawRight.interactable = false;
            MKyawT.interactable = true;
        }
    }

    public void ToggleBasicMKRollInput() {
        if (MKroll.value == 2) {
            MKrollLeft.interactable = true;
            MKrollRight.interactable = true;
            MKrollT.interactable = false;
        }
        else {
            MKrollLeft.interactable = false;
            MKrollRight.interactable = false;
            MKrollT.interactable = true;
        }
    }
    #endregion

    //is called in updated which calls all the toggles
    public void ToggleAllBasicSettings() {
        ToggleBasicMKPitchInput();
        ToggleBasicMKYawInput();
        ToggleBasicMKRollInput();
    }
    #region Advanced toggles
    public void ToggleAdvMK() {
        if (AdvMKpitch.value == 0) {
            AdvMKpitchUp.interactable = false;
            AdvMKpitchDown.interactable = false;
            AdvMKpitchT.interactable = false;
        }
        else if (AdvMKpitch.value == 3) {
            AdvMKpitchUp.interactable = true;
            AdvMKpitchDown.interactable = true;
            AdvMKpitchT.interactable = false;
        }
        else {
            AdvMKpitchUp.interactable = false;
            AdvMKpitchDown.interactable = false;
            AdvMKpitchT.interactable = true;
        }

        if (AdvMKyaw.value == 0) {
            AdvMKyawLeft.interactable = false;
            AdvMKyawRight.interactable = false;
            AdvMKyawT.interactable = false;
        }
        else if (AdvMKyaw.value == 3) {
            AdvMKyawLeft.interactable = true;
            AdvMKyawRight.interactable = true;
            AdvMKyawT.interactable = false;
        }
        else {
            AdvMKyawLeft.interactable = false;
            AdvMKyawRight.interactable = false;
            AdvMKyawT.interactable = true;
        }

        if (AdvMKroll.value == 0) {
            AdvMKrollLeft.interactable = false;
            AdvMKrollRight.interactable = false;
            AdvMKrollT.interactable = false;
        }
        else if (AdvMKroll.value == 3) {
            AdvMKrollLeft.interactable = true;
            AdvMKrollRight.interactable = true;
            AdvMKrollT.interactable = false;
        }
        else {
            AdvMKrollLeft.interactable = false;
            AdvMKrollRight.interactable = false;
            AdvMKrollT.interactable = true;
        }
    }

    public void ToggleAdvController() {
        if (AdvConPitch.value == 0) {
            AdvConPitchUp.interactable = false;
            AdvConPitchDown.interactable = false;
            AdvConPitchT.interactable = false;
        }
        else if (AdvConPitch.value == 7) {
            AdvConPitchUp.interactable = true;
            AdvConPitchDown.interactable = true;
            AdvConPitchT.interactable = false;
        }
        else {
            AdvConPitchUp.interactable = false;
            AdvConPitchDown.interactable = false;
            AdvConPitchT.interactable = true;
        }

        if (AdvConYaw.value == 0) {
            AdvConYawLeft.interactable = false;
            AdvConYawRight.interactable = false;
            AdvConYawT.interactable = false;
        }
        else if (AdvConYaw.value == 7) {
            AdvConYawLeft.interactable = true;
            AdvConYawRight.interactable = true;
            AdvConYawT.interactable = false;
        }
        else {
            AdvConYawLeft.interactable = false;
            AdvConYawRight.interactable = false;
            AdvConYawT.interactable = true;
        }

        if (AdvConRoll.value == 0) {
            AdvConRollLeft.interactable = false;
            AdvConRollRight.interactable = false;
            AdvConRollT.interactable = false;
        }
        else if (AdvConRoll.value == 7) {
            AdvConRollLeft.interactable = true;
            AdvConRollRight.interactable = true;
            AdvConRollT.interactable = false;
        }
        else {
            AdvConRollLeft.interactable = false;
            AdvConRollRight.interactable = false;
            AdvConRollT.interactable = true;
        }

        AdvConAccel.interactable = true;
        AdvConDecel.interactable = true;
    }

    public void ToggleAdvJoystick() {
        if (AdvJoystickPitch.value == 0)
            AdvJoystickPitchT.interactable = false;
        else
            AdvJoystickPitchT.interactable = true;

        if (AdvJoystickYaw.value == 0)
            AdvJoystickYawT.interactable = false;
        else
            AdvJoystickYawT.interactable = true;

        if (AdvJoystickRoll.value == 0)
            AdvJoystickRollT.interactable = false;
        else
            AdvJoystickRollT.interactable = true;
    }

    public void ToggleAdvMobile() {
        if (AdvOSCpitch.value == 0)
            AdvOSCpitchT.interactable = false;
        else
            AdvOSCpitchT.interactable = true;

        if (AdvOSCyaw.value == 0) {
            AdvOSCyawLeft.interactable = false;
            AdvOSCyawRight.interactable = false;
            AdvOSCyawLeft.value = 0;
            AdvOSCyawRight.value = 0;
            AdvOSCyawT.interactable = false;
        }
        else if (AdvOSCyaw.value == 3) {
            AdvOSCyawLeft.interactable = false;
            AdvOSCyawRight.interactable = false;
            AdvOSCyawLeft.value = 1;
            AdvOSCyawRight.value = 1;
            AdvOSCyawT.interactable = false;
        }
        else {
            AdvOSCyawLeft.interactable = false;
            AdvOSCyawRight.interactable = false;
            AdvOSCyawLeft.value = 0;
            AdvOSCyawRight.value = 0;
            AdvOSCyawT.interactable = true;
        }

        if (AdvOSCroll.value == 0) {
            AdvOSCrollLeft.interactable = false;
            AdvOSCrollRight.interactable = false;
            AdvOSCrollLeft.value = 0;
            AdvOSCrollRight.value = 0;
            AdvOSCrollT.interactable = false;
        }
        else if (AdvOSCroll.value == 3) {
            AdvOSCrollLeft.interactable = false;
            AdvOSCrollRight.interactable = false;
            AdvOSCrollLeft.value = 1;
            AdvOSCrollRight.value = 1;
            AdvOSCrollT.interactable = false;
        }
        else {
            AdvOSCrollLeft.interactable = false;
            AdvOSCrollRight.interactable = false;
            AdvOSCrollLeft.value = 0;
            AdvOSCrollRight.value = 0;
            AdvOSCrollT.interactable = true;
        }
    }
    #endregion

    public void AdvConThrottleButtonInput() {
        if (AdvConAccel.value == 1)
            shipInput.controllerAccel = KeyCode.JoystickButton0;
        if (AdvConAccel.value == 2)
            shipInput.controllerAccel = KeyCode.JoystickButton1;
        if (AdvConAccel.value == 3)
            shipInput.controllerAccel = KeyCode.JoystickButton2;
        if (AdvConAccel.value == 4)
            shipInput.controllerAccel = KeyCode.JoystickButton3;
        if (AdvConAccel.value == 5)
            shipInput.controllerAccel = KeyCode.JoystickButton4;
        if (AdvConAccel.value == 6)
            shipInput.controllerAccel = KeyCode.JoystickButton5;
        if (AdvConAccel.value == 7)
            shipInput.controllerAccel = KeyCode.JoystickButton6;
        if (AdvConAccel.value == 8)
            shipInput.controllerAccel = KeyCode.JoystickButton7;

        if (AdvConDecel.value == 1)
            shipInput.controllerDecel = KeyCode.JoystickButton0;
        if (AdvConDecel.value == 2)
            shipInput.controllerDecel = KeyCode.JoystickButton1;
        if (AdvConDecel.value == 3)
            shipInput.controllerDecel = KeyCode.JoystickButton2;
        if (AdvConDecel.value == 4)
            shipInput.controllerDecel = KeyCode.JoystickButton3;
        if (AdvConDecel.value == 5)
            shipInput.controllerDecel = KeyCode.JoystickButton4;
        if (AdvConDecel.value == 6)
            shipInput.controllerDecel = KeyCode.JoystickButton5;
        if (AdvConDecel.value == 7)
            shipInput.controllerDecel = KeyCode.JoystickButton6;
        if (AdvConDecel.value == 8)
            shipInput.controllerDecel = KeyCode.JoystickButton7;
    }

    public void AdvJoystickThrottleButtonInput() {
        if (AdvJoystickAccel.value == 1)
            shipInput.joystickAccel = KeyCode.JoystickButton0;
        if (AdvJoystickAccel.value == 2)
            shipInput.joystickAccel = KeyCode.JoystickButton1;
        if (AdvJoystickAccel.value == 3)
            shipInput.joystickAccel = KeyCode.JoystickButton2;
        if (AdvJoystickAccel.value == 4)
            shipInput.joystickAccel = KeyCode.JoystickButton3;
        if (AdvJoystickAccel.value == 5)
            shipInput.joystickAccel = KeyCode.JoystickButton4;
        if (AdvJoystickAccel.value == 6)
            shipInput.joystickAccel = KeyCode.JoystickButton5;
        if (AdvJoystickAccel.value == 7)
            shipInput.joystickAccel = KeyCode.JoystickButton6;
        if (AdvJoystickAccel.value == 8)
            shipInput.joystickAccel = KeyCode.JoystickButton7;
        if (AdvJoystickAccel.value == 9)
            shipInput.joystickAccel = KeyCode.JoystickButton8;
        if (AdvJoystickAccel.value == 10)
            shipInput.joystickAccel = KeyCode.JoystickButton9;
        if (AdvJoystickAccel.value == 11)
            shipInput.joystickAccel = KeyCode.JoystickButton10;
        if (AdvJoystickAccel.value == 12)
            shipInput.joystickAccel = KeyCode.JoystickButton11;

        if (AdvJoystickDecel.value == 1)
            shipInput.joystickDecel = KeyCode.JoystickButton0;
        if (AdvJoystickDecel.value == 2)
            shipInput.joystickDecel = KeyCode.JoystickButton1;
        if (AdvJoystickDecel.value == 3)
            shipInput.joystickDecel = KeyCode.JoystickButton2;
        if (AdvJoystickDecel.value == 4)
            shipInput.joystickDecel = KeyCode.JoystickButton3;
        if (AdvJoystickDecel.value == 5)
            shipInput.joystickDecel = KeyCode.JoystickButton4;
        if (AdvJoystickDecel.value == 6)
            shipInput.joystickDecel = KeyCode.JoystickButton5;
        if (AdvJoystickDecel.value == 7)
            shipInput.joystickDecel = KeyCode.JoystickButton6;
        if (AdvJoystickDecel.value == 8)
            shipInput.joystickDecel = KeyCode.JoystickButton7;
        if (AdvJoystickDecel.value == 9)
            shipInput.joystickDecel = KeyCode.JoystickButton8;
        if (AdvJoystickDecel.value == 10)
            shipInput.joystickDecel = KeyCode.JoystickButton9;
        if (AdvJoystickDecel.value == 11)
            shipInput.joystickDecel = KeyCode.JoystickButton10;
        if (AdvJoystickDecel.value == 12)
            shipInput.joystickDecel = KeyCode.JoystickButton11;
    }

    public void AdvConPitchButtonInput() {
        if (AdvConPitchUp.value == 1)
            shipInput.controllerPitchUp = KeyCode.JoystickButton0;
        if (AdvConPitchUp.value == 2)
            shipInput.controllerPitchUp = KeyCode.JoystickButton1;
        if (AdvConPitchUp.value == 3)
            shipInput.controllerPitchUp = KeyCode.JoystickButton2;
        if (AdvConPitchUp.value == 4)
            shipInput.controllerPitchUp = KeyCode.JoystickButton3;
        if (AdvConPitchUp.value == 5)
            shipInput.controllerPitchUp = KeyCode.JoystickButton4;
        if (AdvConPitchUp.value == 6)
            shipInput.controllerPitchUp = KeyCode.JoystickButton5;
        if (AdvConPitchUp.value == 7)
            shipInput.controllerPitchUp = KeyCode.JoystickButton6;
        if (AdvConPitchUp.value == 8)
            shipInput.controllerPitchUp = KeyCode.JoystickButton7;

        if (AdvConPitchDown.value == 1)
            shipInput.controllerPitchDown = KeyCode.JoystickButton0;
        if (AdvConPitchDown.value == 2)
            shipInput.controllerPitchDown = KeyCode.JoystickButton1;
        if (AdvConPitchDown.value == 3)
            shipInput.controllerPitchDown = KeyCode.JoystickButton2;
        if (AdvConPitchDown.value == 4)
            shipInput.controllerPitchDown = KeyCode.JoystickButton3;
        if (AdvConPitchDown.value == 5)
            shipInput.controllerPitchDown = KeyCode.JoystickButton4;
        if (AdvConPitchDown.value == 6)
            shipInput.controllerPitchDown = KeyCode.JoystickButton5;
        if (AdvConPitchDown.value == 7)
            shipInput.controllerPitchDown = KeyCode.JoystickButton6;
        if (AdvConPitchDown.value == 8)
            shipInput.controllerPitchDown = KeyCode.JoystickButton7;
    }

    public void AdvConYawButtonInput() {
        if (AdvConYawLeft.value == 1)
            shipInput.controllerYawLeft = KeyCode.JoystickButton0;
        if (AdvConYawLeft.value == 2)
            shipInput.controllerYawLeft = KeyCode.JoystickButton1;
        if (AdvConYawLeft.value == 3)
            shipInput.controllerYawLeft = KeyCode.JoystickButton2;
        if (AdvConYawLeft.value == 4)
            shipInput.controllerYawLeft = KeyCode.JoystickButton3;
        if (AdvConYawLeft.value == 5)
            shipInput.controllerYawLeft = KeyCode.JoystickButton4;
        if (AdvConYawLeft.value == 6)
            shipInput.controllerYawLeft = KeyCode.JoystickButton5;
        if (AdvConYawLeft.value == 7)
            shipInput.controllerYawLeft = KeyCode.JoystickButton6;
        if (AdvConYawLeft.value == 8)
            shipInput.controllerYawLeft = KeyCode.JoystickButton7;

        if (AdvConYawRight.value == 1)
            shipInput.controllerYawRight = KeyCode.JoystickButton0;
        if (AdvConYawRight.value == 2)
            shipInput.controllerYawRight = KeyCode.JoystickButton1;
        if (AdvConYawRight.value == 3)
            shipInput.controllerYawRight = KeyCode.JoystickButton2;
        if (AdvConYawRight.value == 4)
            shipInput.controllerYawRight = KeyCode.JoystickButton3;
        if (AdvConYawRight.value == 5)
            shipInput.controllerYawRight = KeyCode.JoystickButton4;
        if (AdvConYawRight.value == 6)
            shipInput.controllerYawRight = KeyCode.JoystickButton5;
        if (AdvConYawRight.value == 7)
            shipInput.controllerYawRight = KeyCode.JoystickButton6;
        if (AdvConYawRight.value == 8)
            shipInput.controllerYawRight = KeyCode.JoystickButton7;
    }

    public void AdvConRollButtonInput() {
        if (AdvConRollLeft.value == 1)
            shipInput.controllerRollLeft = KeyCode.JoystickButton0;
        if (AdvConRollLeft.value == 2)
            shipInput.controllerRollLeft = KeyCode.JoystickButton1;
        if (AdvConRollLeft.value == 3)
            shipInput.controllerRollLeft = KeyCode.JoystickButton2;
        if (AdvConRollLeft.value == 4)
            shipInput.controllerRollLeft = KeyCode.JoystickButton3;
        if (AdvConRollLeft.value == 5)
            shipInput.controllerRollLeft = KeyCode.JoystickButton4;
        if (AdvConRollLeft.value == 6)
            shipInput.controllerRollLeft = KeyCode.JoystickButton5;
        if (AdvConRollLeft.value == 7)
            shipInput.controllerRollLeft = KeyCode.JoystickButton6;
        if (AdvConRollLeft.value == 8)
            shipInput.controllerRollLeft = KeyCode.JoystickButton7;

        if (AdvConRollRight.value == 1)
            shipInput.controllerRollRight = KeyCode.JoystickButton0;
        if (AdvConRollRight.value == 2)
            shipInput.controllerRollRight = KeyCode.JoystickButton1;
        if (AdvConRollRight.value == 3)
            shipInput.controllerRollRight = KeyCode.JoystickButton2;
        if (AdvConRollRight.value == 4)
            shipInput.controllerRollRight = KeyCode.JoystickButton3;
        if (AdvConRollRight.value == 5)
            shipInput.controllerRollRight = KeyCode.JoystickButton4;
        if (AdvConRollRight.value == 6)
            shipInput.controllerRollRight = KeyCode.JoystickButton5;
        if (AdvConRollRight.value == 7)
            shipInput.controllerRollRight = KeyCode.JoystickButton6;
        if (AdvConRollRight.value == 8)
            shipInput.controllerRollRight = KeyCode.JoystickButton7;
    }

    //called in updated which calls all the advanced toggles
    public void ToggleAllAdvSettings() {
        ToggleAdvMK();
        ToggleAdvController();
        ToggleAdvJoystick();
        ToggleAdvMobile();
    }

    //M+K is 10s, controller 20s, joystick 30s, mobile 40s
    //pitch is 0, yaw is 1, roll is 2, throttle is 3
    //example controller roll is 23, joystick pitch is 30
    //this is gonna work by putting a number into the pitch array which is then gonna be checked from ship input to see which controls are in use
    public void SetControlBinds() {
        if (pitch[0] == 0) {
            if (AdvMKpitch.value != 0)
                pitch[0] = 10;
            else if (AdvConPitch.value != 0)
                pitch[0] = 20;
            else if (AdvJoystickPitch.value != 0)
                pitch[0] = 30;
            else if (AdvOSCpitch.value != 0)
                pitch[0] = 40;
        }
        if (pitch[0] != 0 && pitch[1] == 0) {
            if (pitch[0] == 10) {
                if (AdvConPitch.value != 0) {
                    pitch[1] = 20;
                }
                else if (AdvJoystickPitch.value != 0) {
                    pitch[1] = 30;
                }
                else if (AdvOSCpitch.value != 0) {
                    pitch[1] = 40;
                }
            }
            else if (pitch[0] == 20) {
                if (AdvMKpitch.value != 0) {
                    pitch[1] = 10;
                }
                else if (AdvJoystickPitch.value != 0) {
                    pitch[1] = 30;
                }
                else if (AdvOSCpitch.value != 0) {
                    pitch[1] = 40;
                }
            }
            else if (pitch[0] == 30) {
                if (AdvMKpitch.value != 0) {
                    pitch[1] = 10;
                }
                else if (AdvConPitch.value != 0) {
                    pitch[1] = 20;
                }
                else if (AdvOSCpitch.value != 0) {
                    pitch[1] = 40;
                }
            }
            else if (pitch[0] == 40) {
                if (AdvMKpitch.value != 0) {
                    pitch[1] = 10;
                }
                else if (AdvConPitch.value != 0) {
                    pitch[1] = 20;
                }
                else if (AdvJoystickPitch.value != 0) {
                    pitch[1] = 30;
                }
            }
        }

        if (yaw[0] == 0) {
            if (AdvMKyaw.value != 0)
                yaw[0] = 11;
            else if (AdvConYaw.value != 0)
                yaw[0] = 21;
            else if (AdvJoystickYaw.value != 0)
                yaw[0] = 31;
            else if (AdvOSCyaw.value != 0)
                yaw[0] = 41;
        }
        if (yaw[0] != 0 && yaw[1] == 0) {
            if (yaw[0] == 11) {
                if (AdvConYaw.value != 0) {
                    yaw[1] = 21;
                }
                else if (AdvJoystickYaw.value != 0) {
                    yaw[1] = 31;
                }
                else if (AdvOSCyaw.value != 0) {
                    yaw[1] = 41;
                }
            }
            else if (yaw[0] == 21) {
                if (AdvMKyaw.value != 0) {
                    yaw[1] = 11;
                }
                else if (AdvJoystickYaw.value != 0) {
                    yaw[1] = 31;
                }
                else if (AdvOSCyaw.value != 0) {
                    yaw[1] = 41;
                }
            }
            else if (yaw[0] == 31) {
                if (AdvMKyaw.value != 0) {
                    yaw[1] = 11;
                }
                else if (AdvConYaw.value != 0) {
                    yaw[1] = 21;
                }
                else if (AdvOSCyaw.value != 0) {
                    yaw[1] = 41;
                }
            }
            else if (yaw[0] == 41) {
                if (AdvMKyaw.value != 0) {
                    yaw[1] = 11;
                }
                else if (AdvConYaw.value != 0) {
                    yaw[1] = 21;
                }
                else if (AdvJoystickYaw.value != 0) {
                    yaw[1] = 31;
                }
            }
        }

        if (roll[0] == 0) {
            if (AdvMKroll.value != 0)
                roll[0] = 12;
            else if (AdvConRoll.value != 0)
                roll[0] = 22;
            else if (AdvJoystickRoll.value != 0)
                roll[0] = 32;
            else if (AdvOSCroll.value != 0)
                roll[0] = 42;
        }
        if (roll[0] != 0 && roll[1] == 0) {
            if (roll[0] == 12) {
                if (AdvConRoll.value != 0) {
                    roll[1] = 22;
                }
                else if (AdvJoystickRoll.value != 0) {
                    roll[1] = 32;
                }
                else if (AdvOSCroll.value != 0) {
                    roll[1] = 42;
                }
            }
            else if (roll[0] == 22) {
                if (AdvMKroll.value != 0) {
                    roll[1] = 12;
                }
                else if (AdvJoystickRoll.value != 0) {
                    roll[1] = 32;
                }
                else if (AdvOSCroll.value != 0) {
                    roll[1] = 42;
                }
            }
            else if (roll[0] == 32) {
                if (AdvMKroll.value != 0) {
                    roll[1] = 12;
                }
                else if (AdvConRoll.value != 0) {
                    roll[1] = 22;
                }
                else if (AdvOSCroll.value != 0) {
                    roll[1] = 42;
                }
            }
            else if (roll[0] == 42) {
                if (AdvMKroll.value != 0) {
                    roll[1] = 12;
                }
                else if (AdvConRoll.value != 0) {
                    roll[1] = 22;
                }
                else if (AdvJoystickRoll.value != 0) {
                    roll[1] = 32;
                }
            }
        }

        if (throttle[0] == 0) {
            if (AdvMKthrottleAccel.text != "" && AdvMKthrottleDecel.text != "") {
                throttle[0] = 13;
            }
            else if (AdvConAccel.value != 0 && AdvConDecel.value != 0) {
                throttle[0] = 23;
            }
            else if (AdvJoystickAccel.value != 0 && AdvJoystickDecel.value != 0) {
                throttle[0] = 33;
            }
            else if (AdvOscThrottle.value != 0) {
                throttle[0] = 43;
            }
        }
        if (throttle[0] != 0 && throttle[1] == 0) {
            if (throttle[0] == 13) {
                if (AdvConAccel.value != 0 && AdvConDecel.value != 0) {
                    throttle[1] = 23;
                }
                else if (AdvJoystickAccel.value != 0 && AdvJoystickDecel.value != 0) {
                    throttle[1] = 33;
                }
                else if (AdvOscThrottle.value != 0) {
                    throttle[1] = 43;
                }
            }
            else if (throttle[0] == 23) {
                if (AdvMKthrottleAccel.text != "" && AdvMKthrottleDecel.text != "") {
                    throttle[1] = 13;
                }
                else if (AdvJoystickAccel.value != 0 && AdvJoystickDecel.value != 0) {
                    throttle[1] = 33;
                }
                else if (AdvOscThrottle.value != 0) {
                    throttle[1] = 43;
                }
            }
            else if (throttle[0] == 33) {
                if (AdvMKthrottleAccel.text != "" && AdvMKthrottleDecel.text != "") {
                    throttle[1] = 13;
                }
                else if (AdvConAccel.value != 0 && AdvConDecel.value != 0) {
                    throttle[1] = 23;
                }
                else if (AdvOscThrottle.value != 0) {
                    throttle[1] = 43;
                }
            }
            else if (throttle[0] == 43) {
                if (AdvMKthrottleAccel.text != "" && AdvMKthrottleDecel.text != "") {
                    throttle[1] = 13;
                }
                else if (AdvConAccel.value != 0 && AdvConDecel.value != 0) {
                    throttle[1] = 23;
                }
                else if (AdvJoystickAccel.value != 0 && AdvJoystickDecel.value != 0) {
                    throttle[1] = 33;
                }
            }
        }

        if (pitch[0] != 0) {
            if (pitch[0] == 10) {
                if (AdvMKpitch.value == 1)
                    primaryP.text = "Mouse\nX-Axis";
                if (AdvMKpitch.value == 2)
                    primaryP.text = "Mouse\nY-Axis";
                if (AdvMKpitch.value == 3)
                    primaryP.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKpitchUp.text, AdvMKpitchDown.text);
            }
            if (pitch[0] == 20) {
                if (AdvConPitch.value == 1)
                    primaryP.text = "Controller\nX-Axis";
                if (AdvConPitch.value == 2)
                    primaryP.text = "Controller\nY-Axis";
                if (AdvConPitch.value == 3)
                    primaryP.text = "Controller\nZ-Axis";
                if (AdvConPitch.value == 4)
                    primaryP.text = "Controller\nRZ-Axis";
                if (AdvConPitch.value == 5)
                    primaryP.text = "Controller\nDPAD X-Axis";
                if (AdvConPitch.value == 6)
                    primaryP.text = "Controller\nDPAD Y-Axis";
                if (AdvConPitch.value == 7)
                    primaryP.text = string.Format("Controller buttons\n{0} - {1}", AdvConPitchUp.options[AdvConPitchUp.value].text, AdvConPitchDown.options[AdvConPitchDown.value].text);
            }
            if (pitch[0] == 30) {
                if (AdvJoystickPitch.value == 1)
                    primaryP.text = "Joystick\nX-Axis";
                if (AdvJoystickPitch.value == 2)
                    primaryP.text = "Joystick\nY-Axis";
                if (AdvJoystickPitch.value == 3)
                    primaryP.text = "Joystick\nZ-Axis";
            }
            if (pitch[0] == 40) {
                if (AdvOSCpitch.value == 1)
                    primaryP.text = "Mobile gyro\nX-Axis";
                if (AdvOSCpitch.value == 2)
                    primaryP.text = "Mobile gyro\nY-Axis";
            }
        }
        if (pitch[1] != 0) {
            if (pitch[1] == 10) {
                if (AdvMKpitch.value == 1)
                    secondaryP.text = "Mouse\nX-Axis";
                if (AdvMKpitch.value == 2)
                    secondaryP.text = "Mouse\nY-Axis";
                if (AdvMKpitch.value == 3)
                    secondaryP.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKpitchUp.text, AdvMKpitchDown.text);
            }
            if (pitch[1] == 20) {
                if (AdvConPitch.value == 1)
                    secondaryP.text = "Controller\nX-Axis";
                if (AdvConPitch.value == 2)
                    secondaryP.text = "Controller\nY-Axis";
                if (AdvConPitch.value == 3)
                    secondaryP.text = "Controller\nZ-Axis";
                if (AdvConPitch.value == 4)
                    secondaryP.text = "Controller\nRZ-Axis";
                if (AdvConPitch.value == 5)
                    secondaryP.text = "Controller\nDPAD X-Axis";
                if (AdvConPitch.value == 6)
                    secondaryP.text = "Controller\nDPAD Y-Axis";
                if (AdvConPitch.value == 7)
                    secondaryP.text = string.Format("Controller buttons\n{0} - {1}", AdvConPitchUp.options[AdvConPitchUp.value].text, AdvConPitchDown.options[AdvConPitchDown.value].text);
            }
            if (pitch[1] == 30) {
                if (AdvJoystickPitch.value == 1)
                    secondaryP.text = "Joystick\nX-Axis";
                if (AdvJoystickPitch.value == 2)
                    secondaryP.text = "Joystick\nY-Axis";
                if (AdvJoystickPitch.value == 3)
                    secondaryP.text = "Joystick\nZ-Axis";
            }
            if (pitch[1] == 40) {
                if (AdvOSCpitch.value == 1)
                    secondaryP.text = "Mobile gyro\nX-Axis";
                if (AdvOSCpitch.value == 2)
                    secondaryP.text = "Mobile gyro\nY-Axis";
            }
        }

        if (yaw[0] != 0) {
            if (yaw[0] == 11) {
                if (AdvMKyaw.value == 1)
                    primaryY.text = "Mouse\nX-Axis";
                if (AdvMKyaw.value == 2)
                    primaryY.text = "Mouse\nY-Axis";
                if (AdvMKyaw.value == 3)
                    primaryY.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKyawLeft.text, AdvMKyawRight.text);
            }
            if (yaw[0] == 21) {
                if (AdvConYaw.value == 1)
                    primaryY.text = "Controller\nX-Axis";
                if (AdvConYaw.value == 2)
                    primaryY.text = "Controller\nY-Axis";
                if (AdvConYaw.value == 3)
                    primaryY.text = "Controller\nZ-Axis";
                if (AdvConYaw.value == 4)
                    primaryY.text = "Controller\nRZ-Axis";
                if (AdvConYaw.value == 5)
                    primaryY.text = "Controller\nDPAD X-Axis";
                if (AdvConYaw.value == 6)
                    primaryY.text = "Controller\nDPAD Y-Axis";
                if (AdvConYaw.value == 7)
                    primaryY.text = string.Format("Controller buttons\n{0} - {1}", AdvConYawLeft.options[AdvConYawLeft.value].text, AdvConYawRight.options[AdvConYawRight.value].text);
            }
            if (yaw[0] == 31) {
                if (AdvJoystickYaw.value == 1)
                    primaryY.text = "Joystick\nX-Axis";
                if (AdvJoystickYaw.value == 2)
                    primaryY.text = "Joystick\nY-Axis";
                if (AdvJoystickYaw.value == 3)
                    primaryY.text = "Joystick\nZ-Axis";
            }
            if (yaw[0] == 41) {
                if (AdvOSCyaw.value == 1)
                    primaryY.text = "Mobile gyro\nX-Axis";
                if (AdvOSCyaw.value == 2)
                    primaryY.text = "Mobile gyro\nY-Axis";
                if (AdvOSCyaw.value == 3)
                    primaryY.text = "Mobile buttons\nLeft - Right";
            }
        }
        if (yaw[1] != 0) {
            if (yaw[1] == 11) {
                if (AdvMKyaw.value == 1)
                    secondaryY.text = "Mouse\nX-Axis";
                if (AdvMKyaw.value == 2)
                    secondaryY.text = "Mouse\nY-Axis";
                if (AdvMKyaw.value == 3)
                    secondaryY.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKyawLeft.text, AdvMKyawRight.text);
            }
            if (yaw[1] == 21) {
                if (AdvConYaw.value == 1)
                    secondaryY.text = "Controller\nX-Axis";
                if (AdvConYaw.value == 2)
                    secondaryY.text = "Controller\nY-Axis";
                if (AdvConYaw.value == 3)
                    secondaryY.text = "Controller\nZ-Axis";
                if (AdvConYaw.value == 4)
                    secondaryY.text = "Controller\nRZ-Axis";
                if (AdvConYaw.value == 5)
                    secondaryY.text = "Controller\nDPAD X-Axis";
                if (AdvConYaw.value == 6)
                    secondaryY.text = "Controller\nDPAD Y-Axis";
                if (AdvConYaw.value == 7)
                    secondaryY.text = string.Format("Controller buttons\n{0} - {1}", AdvConYawLeft.options[AdvConYawLeft.value].text, AdvConYawRight.options[AdvConYawRight.value].text);
            }
            if (yaw[1] == 31) {
                if (AdvJoystickYaw.value == 1)
                    secondaryY.text = "Joystick\nX-Axis";
                if (AdvJoystickYaw.value == 2)
                    secondaryY.text = "Joystick\nY-Axis";
                if (AdvJoystickYaw.value == 3)
                    secondaryY.text = "Joystick\nZ-Axis";
            }
            if (yaw[1] == 41) {
                if (AdvOSCyaw.value == 1)
                    secondaryY.text = "Mobile gyro\nX-Axis";
                if (AdvOSCyaw.value == 2)
                    secondaryY.text = "Mobile gyro\nY-Axis";
                if (AdvOSCyaw.value == 3)
                    secondaryY.text = "Mobile buttons\nLeft - Right";
            }
        }

        if (roll[0] != 0) {
            if (roll[0] == 12) {
                if (AdvMKroll.value == 1)
                    primaryR.text = "Mouse\nX-Axis";
                if (AdvMKroll.value == 2)
                    primaryR.text = "Mouse\nY-Axis";
                if (AdvMKroll.value == 3)
                    primaryR.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKrollLeft.text, AdvMKrollRight.text);
            }
            if (roll[0] == 22) {
                if (AdvConRoll.value == 1)
                    primaryR.text = "Controller\nX-Axis";
                if (AdvConRoll.value == 2)
                    primaryR.text = "Controller\nY-Axis";
                if (AdvConRoll.value == 3)
                    primaryR.text = "Controller\nZ-Axis";
                if (AdvConRoll.value == 4)
                    primaryR.text = "Controller\nRZ-Axis";
                if (AdvConRoll.value == 5)
                    primaryR.text = "Controller\nDPAD X-Axis";
                if (AdvConRoll.value == 6)
                    primaryR.text = "Controller\nDPAD Y-Axis";
                if (AdvConRoll.value == 7)
                    primaryR.text = string.Format("Controller buttons\n{0} - {1}", AdvConRollLeft.options[AdvConRollLeft.value].text, AdvConRollRight.options[AdvConRollRight.value].text);
            }
            if (roll[0] == 32) {
                if (AdvJoystickRoll.value == 1)
                    primaryR.text = "Joystick\nX-Axis";
                if (AdvJoystickRoll.value == 2)
                    primaryR.text = "Joystick\nY-Axis";
                if (AdvJoystickRoll.value == 3)
                    primaryR.text = "Joystick\nZ-Axis";
            }
            if (roll[0] == 42) {
                if (AdvOSCroll.value == 1)
                    primaryR.text = "Mobile gyro\nX-Axis";
                if (AdvOSCroll.value == 2)
                    primaryR.text = "Mobile gyro\nY-Axis";
                if (AdvOSCroll.value == 3)
                    primaryR.text = "Mobile buttons\nLeft - Right";
            }
        }
        if (roll[1] != 0) {
            if (roll[1] == 12) {
                if (AdvMKroll.value == 1)
                    secondaryR.text = "Mouse\nX-Axis";
                if (AdvMKroll.value == 2)
                    secondaryR.text = "Mouse\nY-Axis";
                if (AdvMKroll.value == 3)
                    secondaryR.text = string.Format("Keyboard\nKeys: {0} - {1}", AdvMKrollLeft.text, AdvMKrollRight.text);
            }
            if (roll[1] == 22) {
                if (AdvConRoll.value == 1)
                    secondaryR.text = "Controller\nX-Axis";
                if (AdvConRoll.value == 2)
                    secondaryR.text = "Controller\nY-Axis";
                if (AdvConRoll.value == 3)
                    secondaryR.text = "Controller\nZ-Axis";
                if (AdvConRoll.value == 4)
                    secondaryR.text = "Controller\nRZ-Axis";
                if (AdvConRoll.value == 5)
                    secondaryR.text = "Controller\nDPAD X-Axis";
                if (AdvConRoll.value == 6)
                    secondaryR.text = "Controller\nDPAD Y-Axis";
                if (AdvConRoll.value == 7)
                    secondaryR.text = string.Format("Controller buttons\n{0} - {1}", AdvConRollLeft.options[AdvConRollLeft.value].text, AdvConRollRight.options[AdvConRollRight.value].text);
            }
            if (roll[1] == 32) {
                if (AdvJoystickRoll.value == 1)
                    secondaryR.text = "Joystick\nX-Axis";
                if (AdvJoystickRoll.value == 2)
                    secondaryR.text = "Joystick\nY-Axis";
                if (AdvJoystickRoll.value == 3)
                    secondaryR.text = "Joystick\nZ-Axis";
            }
            if (roll[1] == 42) {
                if (AdvOSCroll.value == 1)
                    secondaryR.text = "Mobile gyro\nX-Axis";
                if (AdvOSCroll.value == 2)
                    secondaryR.text = "Mobile gyro\nY-Axis";
                if (AdvOSCroll.value == 3)
                    secondaryR.text = "Mobile buttons\nLeft - Right";
            }
        }

        if (throttle[0] != 0) {
            if (throttle[0] == 13) {
                primaryT.text = string.Format("Keyboard\n{0} - {1}", AdvMKthrottleAccel.text, AdvMKthrottleDecel.text);
            }
            if (throttle[0] == 23) {
                primaryT.text = string.Format("Controller buttons\n{0} - {1}", AdvConAccel.options[AdvConAccel.value].text, AdvConDecel.options[AdvConDecel.value].text);
            }
            if (throttle[0] == 33) {
                primaryT.text = string.Format("Joystick buttons\n{0} - {1}", AdvJoystickAccel.options[AdvJoystickAccel.value].text, AdvJoystickDecel.options[AdvJoystickDecel.value].text);
            }
            if (throttle[0] == 43) {
                primaryT.text = string.Format("Mobile\nSlider");
            }
        }
        if (throttle[1] != 0) {
            if (throttle[1] == 13) {
                secondaryT.text = string.Format("Keyboard\n{0} - {1}", AdvMKthrottleAccel.text, AdvMKthrottleDecel.text);
            }
            if (throttle[1] == 23) {
                secondaryT.text = string.Format("Controller buttons\n{0} - {1}", AdvConAccel.options[AdvConAccel.value].text, AdvConDecel.options[AdvConDecel.value].text);
            }
            if (throttle[1] == 33) {
                secondaryT.text = string.Format("Joystick buttons\n{0} - {1}", AdvJoystickAccel.options[AdvJoystickAccel.value].text, AdvJoystickDecel.options[AdvJoystickDecel.value].text);
            }
            if (throttle[1] == 43) {
                secondaryT.text = string.Format("Mobile\nSlider");
            }
        }
    }

    //sets everything back to their default values which is gonna be called by a button
    public void SetDefaults() {
        //defaults for basic settings
        if (!basicButton.interactable) {
            MKpitch.value = 1;
            MKpitchT.isOn = false;
            MKpitchT.interactable = true;
            MKpitchUp.interactable = false;
            MKpitchDown.interactable = false;
            MKyaw.value = 2;
            MKyawLeft.text = "A";
            MKyawRight.text = "D";
            MKyawT.interactable = false;
            MKroll.value = 0;
            MKrollT.interactable = true;
            MKrollLeft.interactable = false;
            MKrollRight.interactable = false;
            MKthrottleAccel.text = "W";
            MKthrottleDecel.text = "S";

            controllerPitch.value = 1;
            controllerPitchT.isOn = false;
            controllerPitchT.interactable = true;
            controllerYaw.value = 2;
            controllerYawT.isOn = false;
            controllerYawT.interactable = true;
            controllerRoll.value = 0;
            controllerRollT.isOn = false;
            controllerRollT.interactable = true;
            controllerAccel.value = 7;
            controllerDecel.value = 6;

            joystickPitch.value = 1;
            joystickPitchT.isOn = false;
            joystickPitchT.interactable = true;
            joystickYaw.value = 2;
            joystickYawT.isOn = false;
            joystickYawT.interactable = true;
            joystickRoll.value = 0;
            joystickRollT.isOn = false;
            joystickRollT.interactable = true;
            joystickAccel.value = 0;
            joystickDecel.value = 1;

            OSCpitch.value = 0;
            OSCpitchT.isOn = false;
            OSCpitchT.interactable = true;
            OSCyaw.value = 1;
            OSCroll.value = 1;
            OSCrollT.isOn = false;
            OSCrollT.interactable = true;
        }
        //defaults for advanced settings
        if (basicButton.interactable) {
            pitch[0] = 0;
            pitch[1] = 0;
            yaw[0] = 0;
            yaw[1] = 0;
            roll[0] = 0;
            roll[1] = 0;
            throttle[0] = 0;
            throttle[1] = 0;

            primaryP.text = "Unassigned";
            primaryY.text = "Unassigned";
            primaryR.text = "Unassigned";
            primaryT.text = "Unassigned";

            secondaryP.text = "Unassigned";
            secondaryY.text = "Unassigned";
            secondaryR.text = "Unassigned";
            secondaryT.text = "Unassigned";

            AdvMKpitch.value = 0;
            AdvMKpitchUp.interactable = false;
            AdvMKpitchDown.interactable = false;
            AdvMKpitchUp.text = "";
            AdvMKpitchDown.text = "";
            AdvMKpitchT.isOn = false;
            AdvMKpitchT.interactable = false;
            AdvMKyaw.value = 0;
            AdvMKyawLeft.interactable = false;
            AdvMKyawRight.interactable = false;
            AdvMKyawLeft.text = "";
            AdvMKyawRight.text = "";
            AdvMKyawT.isOn = false;
            AdvMKyawT.interactable = false;
            AdvMKroll.value = 0;
            AdvMKrollLeft.interactable = false;
            AdvMKrollRight.interactable = false;
            AdvMKrollLeft.text = "";
            AdvMKrollRight.text = "";
            AdvMKrollT.isOn = false;
            AdvMKrollT.interactable = false;
            AdvMKthrottleAccel.text = "";
            AdvMKthrottleDecel.text = "";

            AdvConPitch.value = 0;
            AdvConPitchUp.value = 0;
            AdvConPitchDown.value = 0;
            AdvConPitchUp.interactable = false;
            AdvConPitchDown.interactable = false;
            AdvConYaw.value = 0;
            AdvConYawLeft.value = 0;
            AdvConYawRight.value = 0;
            AdvConYawLeft.interactable = false;
            AdvConYawRight.interactable = false;
            AdvConRoll.value = 0;
            AdvConRollLeft.value = 0;
            AdvConRollRight.value = 0;
            AdvConRollLeft.interactable = false;
            AdvConRollRight.interactable = false;
            AdvConAccel.value = 0;
            AdvConDecel.value = 0;
            AdvConAccel.interactable = true;
            AdvConDecel.interactable = true;
            AdvConPitchT.isOn = false;
            AdvConPitchT.interactable = false;
            AdvConYawT.isOn = false;
            AdvConYawT.interactable = false;
            AdvConRollT.isOn = false;
            AdvConRollT.interactable = false;

            AdvJoystickPitch.value = 0;
            AdvJoystickYaw.value = 0;
            AdvJoystickRoll.value = 0;
            AdvJoystickAccel.value = 0;
            AdvJoystickDecel.value = 0;
            AdvJoystickPitchT.isOn = false;
            AdvJoystickPitchT.interactable = false;
            AdvJoystickYawT.isOn = false;
            AdvJoystickYawT.interactable = false;
            AdvJoystickRollT.isOn = false;
            AdvJoystickRollT.interactable = false;

            AdvOSCpitch.value = 0;
            AdvOSCyaw.value = 0;
            AdvOSCyawLeft.value = 0;
            AdvOSCyawRight.value = 0;
            AdvOSCyawLeft.interactable = false;
            AdvOSCyawRight.interactable = false;
            AdvOSCroll.value = 0;
            AdvOSCrollLeft.value = 0;
            AdvOSCrollRight.value = 0;
            AdvOSCrollLeft.interactable = false;
            AdvOSCrollRight.interactable = false;
            AdvOSCpitchT.isOn = false;
            AdvOSCpitchT.interactable = false;
            AdvOSCyawT.isOn = false;
            AdvOSCyawT.interactable = false;
            AdvOSCrollT.isOn = false;
            AdvOSCrollT.interactable = false;
            AdvOscThrottle.value = 0;
        }
    }
}
