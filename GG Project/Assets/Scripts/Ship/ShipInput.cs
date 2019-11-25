using UnityEngine;

/// <summary>
/// Class specifically to deal with input.
/// </summary>
public class ShipInput : MonoBehaviour
{
    [Tooltip("When true, the mouse and mousewheel are used for ship input and A/D can be used for strafing like in many arcade space sims.\n\nOtherwise, WASD/Arrows/Joystick + R/T are used for flying, representing a more traditional style space sim.")]
    public bool useMouseInput = true;

    [Space]

    [Range(-1, 1)]
    public float pitch;
    [Range(-1, 1)]
    public float yaw;
    [Range(-1, 1)]
    public float roll;
    [Range(-1, 1)]
    public float strafe;
    public GameManager gameManager;
    public GameObject recentCheckpoint;
    public float Throttle { get; set; }
    public float OscThrottle { get; set; }
    public float Target { get; set; }
    public Vector3 DefaultPos { get; set; } //the phone's default pos which is flat
    public Vector3 startPos; //the player's preferred position/orientation of phone
    public float difference;
    public float YawL { get; set; } //unused
    public float YawR { get; set; } //unused
    public float OSCleftButton { get; set; } //the new version of YawL
    public float OSCrightButton { get; set; } //the new version of YawR
    public float oscX, oscY; //the osc values are first put into here, before being put into pitch/roll/yaw
    public float mouseP, mouseY, mouseR, conP, conY, conR, joyP, joyY, joyR, oscPitch, oscYaw, oscRoll; //these values are used for comparison
    public KeyCode MKpitchUp, MKpitchDown, MKyawLeft, MKyawRight, MKrollLeft, MKrollRight, MKaccel, MKdecel;
    public KeyCode controllerAccel, controllerDecel, joystickAccel, joystickDecel;
    public KeyCode controllerPitchUp, controllerPitchDown, controllerYawLeft, controllerYawRight, controllerRollLeft, controllerRollRight;

    // How quickly the throttle reacts to input.
    private const float THROTTLE_SPEED = 0.7f;

    // Keep a reference to the ship this is attached to just in case.
    private Ship ship;

    private void Awake() {
        ship = GetComponent<Ship>();
        mouseP = 0f;
        mouseY = 0f;
        mouseR = 0f;
    }

    private void Update() {
        SetToZero();
        SpawnAtRecentCheckpoint();    
        if (!gameManager.basicButton.interactable) {
            BasicControlSelector();
        }
        else {
            AdvancedControlSelector();
        }
    }

    public void SetToZero() {
        if (Time.timeScale == 0) {
            pitch = 0;
            yaw = 0;
            roll = 0;
        }
    }

    public void SpawnAtRecentCheckpoint() {
        if (recentCheckpoint != null) {
            if (Input.GetKey(KeyCode.Space)) {
                pitch = 0;
                yaw = 0;
                roll = 0;
                Target = 0;
                Throttle = 0;
                gameObject.transform.position = recentCheckpoint.transform.position;
            }
        }
    }

    #region Basic Controls
    #region MK Stuff
    public void BasicMKPitch() {
        if (gameManager.MKpitch.value == 0) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.MKpitchT.isOn) {
                mouseP = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                pitch = mouseP;
            }
            else {
                mouseP = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                pitch = mouseP;
            }
            pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);
        }
        else if (gameManager.MKpitch.value == 1) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.MKpitchT.isOn) {
                mouseP = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);//inverted
                pitch = mouseP;
            }
            else {
                mouseP = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);//normal
                pitch = mouseP;
            }
            pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);
        }
        else if (gameManager.MKpitch.value == 2) {
            if (Input.GetKey(MKpitchUp)) {
                pitch = -1;
            }
            else if (Input.GetKey(MKpitchDown)) {
                pitch = 1;
            }
            else {
                pitch = 0;
            }
        }
        
    }

    public void BasicMKYaw() {
        if (gameManager.MKyaw.value == 0) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.MKyawT.isOn) {
                yaw = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
            }
            else {
                yaw = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
            }
        }
        else if (gameManager.MKyaw.value == 1) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.MKyawT.isOn) {
                yaw = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
            }
            else {
                yaw = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
            }
        }
        else if (gameManager.MKyaw.value == 2) {
            if (Input.GetKey(MKyawLeft)) {
                yaw = -1;
            }
            else if (Input.GetKey(MKyawRight)) {
                yaw = 1;
            }
            else {
                yaw = 0;
            }
        }
    }

    public void BasicMKRoll() {
        Vector3 mousePos = Input.mousePosition;
        if (gameManager.MKroll.value == 0) {
            if (gameManager.MKrollT.isOn) {
                roll = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);//inverted
            }
            else {
                roll = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);//normal
            }
            roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        }
        else if (gameManager.MKroll.value == 1) {
            if (gameManager.MKrollT.isOn) {
                roll = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
            }
            else {
                roll = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
            }
            roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        }
        else if (gameManager.MKroll.value == 2) {
            if (Input.GetKey(MKrollLeft)) {
                roll = -1;
            }
            else if (Input.GetKey(MKrollRight)) {
                roll = 1;
            }
            else {
                roll = 0;
            }
        }
    }
    #endregion
    
    #region Controller Stuff
    //0 is x-axis (left stick horizontal)
    //1 is y-axis (left stick vertical)
    //2 is z-axis (right stick horizontal)
    //3 is rz-axis (right stick vertical)
    public void BasicControllerPitch() {
        if (gameManager.controllerPitch.value == 0) {
            if (gameManager.controllerPitchT.isOn)
                pitch = -Input.GetAxis("HorizontalConLeft");
            else
                pitch = Input.GetAxis("HorizontalConLeft");
        }
        else if (gameManager.controllerPitch.value == 1) {
            if (gameManager.controllerPitchT.isOn) {
                conP = Input.GetAxis("VerticalConLeft"); //inverted
                pitch = conP;
            }
            else {
                conP = -Input.GetAxis("VerticalConLeft");
                pitch = conP;
            }               
        }
        else if (gameManager.controllerPitch.value == 2) {
            if (gameManager.controllerPitchT.isOn)
                pitch = -Input.GetAxis("HorizontalConRight");
            else
                pitch = Input.GetAxis("HorizontalConRight");
        }
        else if (gameManager.controllerPitch.value == 3) {
            if (gameManager.controllerPitchT.isOn)
                pitch = -Input.GetAxis("VerticalConRight");
            else
                pitch = Input.GetAxis("VerticalConRight");
        }
    }

    public void BasicControllerYaw() {
        if (gameManager.controllerYaw.value == 0) {
            if (gameManager.controllerYawT.isOn)
                yaw = -Input.GetAxis("HorizontalConLeft");
            else
                yaw = Input.GetAxis("HorizontalConLeft");
        }
        else if (gameManager.controllerYaw.value == 1) {
            if (gameManager.controllerYawT.isOn)
                yaw = -Input.GetAxis("VerticalConLeft");
            else
                yaw = Input.GetAxis("VerticalConLeft");
        }
        else if (gameManager.controllerYaw.value == 2) {
            if (gameManager.controllerYawT.isOn)
                yaw = -Input.GetAxis("HorizontalConRight"); //inverted
            else
                yaw = Input.GetAxis("HorizontalConRight"); //normal
        }
        else if (gameManager.controllerYaw.value == 3) {
            if (gameManager.controllerYawT.isOn)
                yaw = -Input.GetAxis("VerticalConRight");
            else
                yaw = Input.GetAxis("VerticalConRight");
        }
    }

    public void BasicControllerRoll() {
        if (gameManager.controllerRoll.value == 0) {
            if (gameManager.controllerRollT.isOn)
                roll = Input.GetAxis("HorizontalConLeft"); //inverted
            else
                roll = -Input.GetAxis("HorizontalConLeft"); //normal
        }
        else if (gameManager.controllerRoll.value == 1) {
            if (gameManager.controllerRollT.isOn)
                roll = Input.GetAxis("VerticalConLeft");
            else
                roll = -Input.GetAxis("VerticalConLeft");
        }
        else if (gameManager.controllerRoll.value == 2) {
            if (gameManager.controllerRollT.isOn)
                roll = Input.GetAxis("HorizontalConRight");
            else
                roll = -Input.GetAxis("HorizontalConRight");
        }
        else if (gameManager.controllerRoll.value == 3) {
            if (gameManager.controllerRollT.isOn)
                roll = Input.GetAxis("VerticalConRight");
            else
                roll = -Input.GetAxis("VerticalConRight");
        }
    }
    #endregion

    #region Joystick Stuff
    public void BasicJoystickPitch() {
        if (gameManager.joystickPitch.value == 0) {
            if (gameManager.joystickPitchT.isOn) {
                pitch = -Input.GetAxis("JoystickX");
                pitch = joyP;
            }
            else {
                joyP = Input.GetAxis("JoystickX");
                pitch = joyP;
            }
        }
        else if (gameManager.joystickPitch.value == 1) {
            if (gameManager.joystickPitchT.isOn) {
                joyP = -Input.GetAxis("JoystickY"); //inverted
                pitch = joyP;
            }
            else {
                joyP = Input.GetAxis("JoystickY"); //normal
                pitch = joyP;
            }
        }
        else if (gameManager.joystickPitch.value == 2) {
            if (gameManager.joystickPitchT.isOn) {
                joyP = -Input.GetAxis("JoystickZ");
                pitch = joyP;
            }
            else {
                joyP = Input.GetAxis("JoystickZ");
                pitch = joyP;
            }
        }
    }

    public void BasicJoystickYaw() {
        if (gameManager.joystickYaw.value == 0) {
            if (gameManager.joystickYawT.isOn)
                yaw = -Input.GetAxis("JoystickX");
            else
                yaw = Input.GetAxis("JoystickX");
        }
        else if (gameManager.joystickYaw.value == 1) {
            if (gameManager.joystickYawT.isOn)
                yaw = -Input.GetAxis("JoystickY");
            else
                yaw = Input.GetAxis("JoystickY");
        }
        else if (gameManager.joystickYaw.value == 2) {
            if (gameManager.joystickYawT.isOn)
                yaw = -Input.GetAxis("JoystickZ"); //inverted
            else
                yaw = Input.GetAxis("JoystickZ"); //normal
        }
    }

    public void BasicJoystickRoll() {
        if (gameManager.joystickRoll.value == 0) {
            if (gameManager.joystickRollT.isOn)
                roll = Input.GetAxis("JoystickX"); //inverted
            else
                roll = -Input.GetAxis("JoystickX"); //normal
        }
        else if (gameManager.joystickRoll.value == 1) {
            if (gameManager.joystickRollT.isOn)
                roll = Input.GetAxis("JoystickY");
            else
                roll = -Input.GetAxis("JoystickY");
        }
        else if (gameManager.joystickRoll.value == 2) {
            if (gameManager.joystickRollT.isOn)
                roll = Input.GetAxis("JoystickZ");
            else
                roll = -Input.GetAxis("JoystickZ");
        }
    }
    #endregion

    #region OSC Stuff
    public void BasicOSCPitch() {
        if (gameManager.OSCpitchT.isOn) {
            if (startPos.x == 0) {//inverted
                startPos = DefaultPos;
            }
            difference = (DefaultPos.x - startPos.x);
            oscX = -(startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
            pitch = oscX;
            pitch = Mathf.Clamp(pitch, -1f, 1f);
        }
        else {//normal
            if (startPos.x == 0) {
                startPos = DefaultPos;
            }
            difference = (DefaultPos.x - startPos.x);
            oscX = (startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
            pitch = oscX;
            pitch = Mathf.Clamp(pitch, -1f, 1f);
        }
        
    }

    public void BasicOSCYaw() {
        if (YawL < 0) {
            yaw = YawL;
        }
        else {
            yaw = YawR;
        }
        yaw = Mathf.Clamp(yaw, -1f, 1f);  
    }

    public void BasicOSCRoll() {
        if (gameManager.OSCrollT.isOn) {
            oscY = -(DefaultPos.y / 10);
            oscRoll = roll = oscY;
            roll = -Mathf.Clamp(roll, -1.0f, 1.0f);//inverted
        }
        else {
            oscY = -(DefaultPos.y / 10);
            oscRoll = roll = oscY;
            roll = Mathf.Clamp(roll, -1.0f, 1.0f);//normal
        }
    }
    #endregion
    #endregion

    #region Advanced Controls
    #region AdvMK stuff
    public void AdvancedMKPitch() {
        if (gameManager.AdvMKpitch.value == 1) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKpitchT.isOn) {
                mouseP = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                pitch = mouseP;
            }

            else {
                mouseP = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                pitch = mouseP;
            }
            pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKpitch.value == 2) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKpitchT.isOn) {
                mouseP = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);//inverted
                pitch = mouseP;
            }
            else {
                mouseP = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);//normal
                pitch = mouseP;
            }
            pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKpitch.value == 3) {
            Vector3 mousePos = Input.mousePosition;
            if (Input.GetKey(MKpitchUp)) {
                mouseP = -1;
                pitch = mouseP;
            }
            else if (Input.GetKey(MKpitchDown)) {
                mouseP = 1;
                pitch = mouseP;
            }
            else {
                mouseP = 0;
                pitch = mouseP;
            }
        }
    }

    public void AdvancedMKYaw() {
        if (gameManager.AdvMKyaw.value == 1) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKyawT.isOn) {
                mouseY = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                yaw = mouseY;
            }
            else {
                mouseY = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
                yaw = mouseY;
            }
            yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKyaw.value == 2) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKyawT.isOn) {
                mouseY = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
                yaw = mouseY;
            }

            else {
                mouseY = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
                yaw = mouseY;
            }
            yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKyaw.value == 3) {
            if (Input.GetKey(MKyawLeft)) {
                mouseY = -1;
                yaw = mouseY;
            }
            else if (Input.GetKey(MKyawRight)) {
                mouseY = 1;
                yaw = mouseY;
            }
            else {
                mouseY = 0;
                yaw = mouseY;
            }
        }
    }

    public void AdvancedMKRoll() {
        if (gameManager.AdvMKroll.value == 1) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKrollT.isOn) {
                mouseR = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);//inverted
                roll = mouseR;
            }

            else {
                mouseR = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);//normal
                roll = mouseR;
            }                
            roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKroll.value == 2) {
            Vector3 mousePos = Input.mousePosition;
            if (gameManager.AdvMKrollT.isOn) {
                mouseR = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
                roll = mouseR;
            }

            else {
                mouseR = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
                roll = mouseR;
            }                
            roll = Mathf.Clamp(roll, -1.0f, 1.0f);
        }
        else if (gameManager.AdvMKroll.value == 3) {
            if (Input.GetKey(MKrollLeft)) {
                mouseR = -1;
                roll = mouseR;
            }
            else if (Input.GetKey(MKrollRight)) {
                mouseR = 1;
                roll = mouseR;
            }
            else {
                mouseR = 0;
                roll = mouseR;
            }
        }
    }
    #endregion

    #region AdvController Stuff
    public void AdvancedControllerPitch() {
        //x
        if (gameManager.AdvConPitch.value == 1) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = -Input.GetAxis("HorizontalConLeft");
                pitch = conP;
            }
            else {
                conP = Input.GetAxis("HorizontalConLeft");
                pitch = conP;
            }                
        }
        //y
        else if (gameManager.AdvConPitch.value == 2) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = Input.GetAxis("VerticalConLeft"); //inverted
                pitch = conP;
            }
            else {
                conP = -Input.GetAxis("VerticalConLeft"); //normal
                pitch = conP;
            }                
        }
        //z
        else if (gameManager.AdvConPitch.value == 3) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = -Input.GetAxis("HorizontalConRight");
                pitch = conP;
            }
            else {
                conP = Input.GetAxis("HorizontalConRight");
                pitch = conP;
            }                
        }
        //rz
        else if (gameManager.AdvConPitch.value == 4) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = -Input.GetAxis("VerticalConRight");
                pitch = conP;
            }
            else {
                conP = Input.GetAxis("VerticalConRight");
                pitch = conP;
            }                
        }
        //dpad x
        else if (gameManager.AdvConPitch.value == 5) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = -Input.GetAxis("DpadX");
                pitch = conP;
            }
            else {
                conP = Input.GetAxis("DpadX");
                pitch = conP;
            }
                
        }
        //dpad y
        else if (gameManager.AdvConPitch.value == 6) {
            if (gameManager.AdvConPitchT.isOn) {
                conP = -Input.GetAxis("DpadY");
                pitch = conP;
            }                
            else {
                conP = Input.GetAxis("DpadY");
                pitch = conP;
            }                
        }
        //buttons
        else if (gameManager.AdvConPitch.value == 7) {
            if (Input.GetKey(controllerPitchUp)) {
                conP = -1;
                pitch = conP;
            }
            else if (Input.GetKey(controllerPitchDown)) {
                conP = 1;
                pitch = conP;
            }
            else {
                conP = 0;
                pitch = conP;
            }
        }
    }

    public void AdvancedControllerYaw() {
        if (gameManager.AdvConYaw.value == 1) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("HorizontalConLeft");
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("HorizontalConLeft");
                yaw = conY;
            }
                
        }
        else if (gameManager.AdvConYaw.value == 2) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("VerticalConLeft");
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("VerticalConLeft");
                yaw = conY;
            }                
        }
        else if (gameManager.AdvConYaw.value == 3) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("HorizontalConRight"); //inverted
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("HorizontalConRight"); //normal
                yaw = conY;
            }                
        }
        else if (gameManager.AdvConYaw.value == 4) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("VerticalConRight");
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("VerticalConRight");
                yaw = conY;
            }
        }
        else if (gameManager.AdvConYaw.value == 5) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("DpadX");
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("DpadX");
                yaw = conY;
            }                
        }
        else if (gameManager.AdvConYaw.value == 6) {
            if (gameManager.AdvConYawT.isOn) {
                conY = -Input.GetAxis("DpadY");
                yaw = conY;
            }
            else {
                conY = Input.GetAxis("DpadY");
                yaw = conY;
            }
                
        }
        else if (gameManager.AdvConYaw.value == 7) {
            if (Input.GetKey(controllerYawLeft)) {
                conY = -1;
                yaw = conY;
            }
            else if (Input.GetKey(controllerYawRight)) {
                conY = 1;
                yaw = conY;
            }
            else {
                conY = 0;
                yaw = conY;
            }
        }
    }

    public void AdvancedControllerRoll() {
        if (gameManager.AdvConRoll.value == 1) {
            if (gameManager.AdvConRollT.isOn) {
                conR = Input.GetAxis("HorizontalConLeft"); //inverted
                roll = conR;
            }
            else {
                conR = -Input.GetAxis("HorizontalConLeft"); //normal
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 2) {
            if (gameManager.AdvConRollT.isOn) {
                conR = Input.GetAxis("VerticalConLeft");
                roll = conR;
            }                
            else {
                conR = -Input.GetAxis("VerticalConLeft");
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 3) {
            if (gameManager.AdvConRollT.isOn) {
                conR = Input.GetAxis("HorizontalConRight");
                roll = conR;
            }                
            else {
                conR = -Input.GetAxis("HorizontalConRight");
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 4) {
            if (gameManager.AdvConRollT.isOn) {
                conR = Input.GetAxis("VerticalConRight");
                roll = conR;
            }                
            else {
                conR = -Input.GetAxis("VerticalConRight");
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 5) {
            if (gameManager.AdvConRollT.isOn) {
                conR = -Input.GetAxis("DpadX");
                roll = conR;
            }                
            else {
                conR = Input.GetAxis("DpadX");
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 6) {
            if (gameManager.AdvConRollT.isOn) {
                conR = -Input.GetAxis("DpadY");
                roll = conR;
            }                
            else {
                conR = Input.GetAxis("DpadY");
                roll = conR;
            }                
        }
        else if (gameManager.AdvConRoll.value == 7) {
            if (Input.GetKey(controllerRollLeft)) {
                conR = -1;
                roll = conR;
            }
            else if (Input.GetKey(controllerRollRight)) {
                conR = 1;
                roll = conR;
            }
            else {
                conR = 0;
                roll = conR;
            }
        }
    }
    #endregion

    #region AdvJoystick Stuff
    public void AdvancedJoystickPitch() {
        if (gameManager.AdvJoystickPitch.value == 1) {
            if (gameManager.AdvJoystickPitchT.isOn) {
                joyP = -Input.GetAxis("JoystickX");
                pitch = joyP;
            }                
            else {
                joyP = Input.GetAxis("JoystickX");
                pitch = joyP;
            }                
        }
        else if (gameManager.AdvJoystickPitch.value == 2) {
            if (gameManager.AdvJoystickPitchT.isOn) {
                joyP = -Input.GetAxis("JoystickY"); //inverted
                pitch = joyP;
            }                
            else {
                joyP = Input.GetAxis("JoystickY"); //normal
                pitch = joyP;
            }                
        }
        else if (gameManager.AdvJoystickPitch.value == 3) {
            if (gameManager.AdvJoystickPitchT.isOn) {
                joyP = -Input.GetAxis("JoystickZ");
                pitch = joyP;
            }                
            else {
                joyP = Input.GetAxis("JoystickZ");
                pitch = joyP;
            }                
        }
    }

    public void AdvancedJoystickYaw() {
        if (gameManager.AdvJoystickYaw.value == 1) {
            if (gameManager.AdvJoystickYawT.isOn) {
                joyY = -Input.GetAxis("JoystickX");
                yaw = joyY;
            }                
            else {
                joyY = Input.GetAxis("JoystickX");
                yaw = joyY;
            }                
        }
        else if (gameManager.AdvJoystickYaw.value == 2) {
            if (gameManager.AdvJoystickYawT.isOn) {
                joyY = -Input.GetAxis("JoystickY");
                yaw = joyY;
            }                
            else {
                joyY = Input.GetAxis("JoystickY");
                yaw = joyY;
            }                
        }
        else if (gameManager.AdvJoystickYaw.value == 3) {
            if (gameManager.AdvJoystickYawT.isOn) {
                joyY = -Input.GetAxis("JoystickZ"); //inverted
                yaw = joyY;
            }                
            else {
                joyY = Input.GetAxis("JoystickZ"); //normal
                yaw = joyY;
            }                
        }
    }

    public void AdvancedJoystickRoll() {
        if (gameManager.AdvJoystickRoll.value == 1) {
            if (gameManager.AdvJoystickRollT.isOn) {
                joyR = Input.GetAxis("JoystickX"); //inverted
                roll = joyR;
            }                
            else {
                joyR = -Input.GetAxis("JoystickX"); //normal
                roll = joyR;
            }                
        }
        else if (gameManager.AdvJoystickRoll.value == 2) {
            if (gameManager.AdvJoystickRollT.isOn) {
                joyR = Input.GetAxis("JoystickY");
                roll = joyR;
            }                
            else {
                joyR = -Input.GetAxis("JoystickY");
                roll = joyR;
            }                
        }
        else if (gameManager.AdvJoystickRoll.value == 3) {
            if (gameManager.AdvJoystickRollT.isOn) {
                joyR = Input.GetAxis("JoystickZ");
                roll = joyR;
            }                
            else {
                joyR = -Input.GetAxis("JoystickZ");
                roll = joyR;
            }                
        }
    }
    #endregion

    #region OSC Stuff
    public void AdvancedOSCPitch() {
        if (gameManager.AdvOSCpitch.value == 1) {
            if (gameManager.AdvOSCpitchT.isOn) {
                if (startPos.x == 0) {//inverted
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = -(startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscPitch = pitch = oscX;
                pitch = Mathf.Clamp(pitch, -1f, 1f);
            }
            else {
                if (startPos.x == 0) {
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = (startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscPitch = pitch = oscX;
                pitch = Mathf.Clamp(pitch, -1f, 1f);
            }
        }
        else if (gameManager.AdvOSCpitch.value == 2) {
            if (gameManager.AdvOSCpitchT.isOn) {
                oscY = -(DefaultPos.y / 10);
                oscPitch = pitch = oscY;
                pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);//inverted
            }
            else {
                oscY = -(DefaultPos.y / 10);
                oscPitch = pitch = oscY;
                pitch = Mathf.Clamp(pitch, -1.0f, 1.0f);//normal
            }
        }
    }

    public void AdvancedOSCYaw() {
        if (gameManager.AdvOSCyaw.value == 1) {
            if (gameManager.AdvOSCyawT.isOn) {
                if (startPos.x == 0) {//inverted
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = -(startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscYaw = yaw = oscX;
                yaw = Mathf.Clamp(yaw, -1f, 1f);
            }
            else {
                if (startPos.x == 0) {
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = (startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscYaw = yaw = oscX;
                yaw = Mathf.Clamp(yaw, -1f, 1f);
            }
        }
        else if (gameManager.AdvOSCyaw.value == 2) {
            if (gameManager.AdvOSCyawT.isOn) {
                oscY = -(DefaultPos.y / 10);
                oscYaw = yaw = oscY;
                yaw = -Mathf.Clamp(yaw, -1.0f, 1.0f);//inverted
            }
            else {
                oscY = -(DefaultPos.y / 10);
                oscYaw = yaw = oscY;
                yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);//normal
            }
        }
        else if (gameManager.AdvOSCyaw.value == 3) {
            if (OSCleftButton < 0) {
                oscYaw = yaw = OSCleftButton;
            }
            else {
                oscYaw = yaw = OSCrightButton;
            }
            yaw = Mathf.Clamp(yaw, -1f, 1f);
        }
    }

    public void AdvancedOSCRoll() {
        if (gameManager.AdvOSCroll.value == 1) {
            if (gameManager.AdvOSCrollT.isOn) {
                if (startPos.x == 0) {//inverted
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = -(startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscRoll = roll = oscX;
                roll = Mathf.Clamp(roll, -1f, 1f);
            }
            else {
                if (startPos.x == 0) {
                    startPos = DefaultPos;
                }
                difference = (DefaultPos.x - startPos.x);
                oscX = (startPos.x - DefaultPos.x) / (DefaultPos.x - difference) * 1.8f;
                oscRoll = roll = oscX;
                roll = Mathf.Clamp(roll, -1f, 1f);
            }
        }
        else if (gameManager.AdvOSCroll.value == 2) {
            if (gameManager.AdvOSCrollT.isOn) {
                oscY = -(DefaultPos.y / 10);
                oscRoll = roll = oscY;
                roll = -Mathf.Clamp(roll, -1.0f, 1.0f);//inverted
            }
            else {
                oscY = -(DefaultPos.y / 10);
                oscRoll = roll = oscY;
                roll = Mathf.Clamp(roll, -1.0f, 1.0f);//normal
            }
        }
        else if (gameManager.AdvOSCroll.value == 3) {
            if (OSCleftButton < 0) {
                oscRoll = roll = OSCleftButton;
            }
            else {
                oscRoll = roll = OSCrightButton;
            }
            roll = Mathf.Clamp(roll, -1f, 1f);
        }
    }
    #endregion
    #endregion

    //called by the OSC property sender
    public void UpdateOSCThrottle() {
        Target = Throttle;
        Throttle = Mathf.Clamp(Throttle, 0f, 1f);
    }

    public void UpdateKeyboardThrottle(KeyCode increaseKey, KeyCode decreaseKey) {
        Target = Throttle;

        if (Input.GetKey(increaseKey))
            Target = 1.0f;
        else if (Input.GetKey(decreaseKey))
            Target = 0.0f;

        Throttle = Mathf.MoveTowards(Throttle, Target, Time.deltaTime * THROTTLE_SPEED);
    }

    public void BasicControlSelector() {
        //mouse controls
        if (gameManager.selector.value == 0) {
            useMouseInput = true;
            /*if (Input.GetAxis("VerticalConLeft") != 0) { //I need to do something like this I think to have primary and secondary input
                pitch = Input.GetAxis("VerticalConLeft");
            }
            else {
                MousePitch();
            }*/
            BasicMKPitch();
            BasicMKYaw();
            BasicMKRoll();
            UpdateKeyboardThrottle(MKaccel, MKdecel);
        }
        //controller controls
        else if (gameManager.selector.value == 1) {
            useMouseInput = false;
            BasicControllerPitch();
            BasicControllerYaw();
            BasicControllerRoll();
            UpdateKeyboardThrottle(controllerAccel, controllerDecel); //R2 to accel, L2 to decel is the default
        }
        //joystick controls
        else if (gameManager.selector.value == 2) {
            useMouseInput = false;
            BasicJoystickPitch();
            BasicJoystickYaw();
            BasicJoystickRoll();
            UpdateKeyboardThrottle(joystickAccel, joystickDecel); //Trigger to accel, thumb to decel is the default
        }
        //mobile osc controls
        else if (gameManager.selector.value == 3) {
            useMouseInput = false;
            BasicOSCPitch();
            BasicOSCYaw();
            BasicOSCRoll();
            UpdateOSCThrottle();
        }
    }

    //10s are MK, 20s are controller, 30s are joystick, 40s are mobile
    //0s are pitch, 1s are yaw, 2s are roll, 3s are throttle
    public void AdvancedControlSelector() {
        #region Pitch Controls
        if (gameManager.pitch[0] == 10) {
            if (gameManager.pitch[1] == 20) {
                if (Mathf.Abs(conP) > Mathf.Abs(mouseP)) {
                    AdvancedControllerPitch();
                }                    
                else {
                    AdvancedControllerPitch();
                    AdvancedMKPitch();
                }                    
            }
            else if (gameManager.pitch[1] == 30) {
                if (Mathf.Abs(joyP) > Mathf.Abs(mouseP))
                    AdvancedJoystickPitch();
                else {
                    AdvancedJoystickPitch();
                    AdvancedMKPitch();
                }
            }
            else if (gameManager.pitch[1] == 40) {
                if (Mathf.Abs(oscPitch) > Mathf.Abs(mouseP)) {
                    AdvancedMKPitch();
                    AdvancedOSCPitch();
                }                    
                else {
                    AdvancedOSCPitch();
                    AdvancedMKPitch();
                }
            }
        }
        else if (gameManager.pitch[0] == 20) {
            if (gameManager.pitch[1] == 10) {
                if (Mathf.Abs(mouseP) > Mathf.Abs(conP))
                    AdvancedMKPitch();
                else {
                    AdvancedMKPitch();
                    AdvancedControllerPitch();
                }
            }
            else if (gameManager.pitch[1] == 30) {
                if (Mathf.Abs(joyP) > Mathf.Abs(conP))
                    AdvancedJoystickPitch();
                else {
                    AdvancedJoystickPitch();
                    AdvancedControllerPitch();
                }
            }
            else if (gameManager.pitch[1] == 40) {
                if (Mathf.Abs(oscPitch) > Mathf.Abs(conP)) {
                    AdvancedControllerPitch();
                    AdvancedOSCPitch();
                }
                else {
                    AdvancedOSCPitch();
                    AdvancedControllerPitch();
                }
            }
        }
        else if (gameManager.pitch[0] == 30) {
            if (gameManager.pitch[1] == 10) {
                if (Mathf.Abs(mouseP) > Mathf.Abs(joyP))
                    AdvancedMKPitch();
                else {
                    AdvancedMKPitch();
                    AdvancedJoystickPitch();
                }
            }
            else if (gameManager.pitch[1] == 20) {
                if (Mathf.Abs(conP) > Mathf.Abs(joyP))
                    AdvancedControllerPitch();
                else {
                    AdvancedControllerPitch();
                    AdvancedJoystickPitch();
                }
            }
            else if (gameManager.pitch[1] == 40) {
                if (Mathf.Abs(oscPitch) > Mathf.Abs(joyP)) {
                    AdvancedJoystickPitch();
                    AdvancedOSCPitch();
                }
                else {
                    AdvancedOSCPitch();
                    AdvancedJoystickPitch();
                }
            }
        }
        else if (gameManager.pitch[0] == 40) {
            if (gameManager.pitch[1] == 10) {
                if (Mathf.Abs(mouseP) > Mathf.Abs(oscPitch)) {
                    AdvancedOSCPitch();
                    AdvancedMKPitch();
                }
                else {
                    AdvancedMKPitch();
                    AdvancedOSCPitch();
                }
            }
            else if (gameManager.pitch[1] == 20) {
                if (Mathf.Abs(conP) > Mathf.Abs(oscPitch)) {
                    AdvancedOSCPitch();
                    AdvancedControllerPitch();
                }
                else {
                    AdvancedControllerPitch();
                    AdvancedOSCPitch();
                }
            }
            else if (gameManager.pitch[1] == 30) {
                if (Mathf.Abs(joyP) > Mathf.Abs(oscPitch)) {
                    AdvancedOSCPitch();
                    AdvancedJoystickPitch();
                }
                else {
                    AdvancedJoystickPitch();
                    AdvancedOSCPitch();
                }
            }
        }
        #endregion

        #region Yaw Controls
        if (gameManager.yaw[0] == 11) {
            if (gameManager.yaw[1] == 21) {
                if (Mathf.Abs(conY) > Mathf.Abs(mouseY))
                    AdvancedControllerYaw();
                else {
                    AdvancedControllerYaw();
                    AdvancedMKYaw();
                }
            }
            else if (gameManager.yaw[1] == 31) {
                if (Mathf.Abs(joyY) > Mathf.Abs(mouseY))
                    AdvancedJoystickYaw();
                else {
                    AdvancedJoystickYaw();
                    AdvancedMKYaw();
                }
            }
            else if (gameManager.yaw[1] == 41) {
                if (Mathf.Abs(oscYaw) > Mathf.Abs(mouseY)) {
                    AdvancedMKYaw();
                    AdvancedOSCYaw();
                }
                else {
                    AdvancedOSCYaw();
                    AdvancedMKYaw();
                }
            }
        }
        else if (gameManager.yaw[0] == 21) {
            if (gameManager.yaw[1] == 11) {
                if (Mathf.Abs(mouseY) > Mathf.Abs(conY))
                    AdvancedMKYaw();
                else {
                    AdvancedMKYaw();
                    AdvancedControllerYaw();
                }
            }
            else if (gameManager.yaw[1] == 31) {
                if (Mathf.Abs(joyY) > Mathf.Abs(conY))
                    AdvancedJoystickYaw();
                else {
                    AdvancedJoystickYaw();
                    AdvancedControllerYaw();
                }
            }
            else if (gameManager.yaw[1] == 41) {
                if (Mathf.Abs(oscYaw) > Mathf.Abs(conY)) {
                    AdvancedControllerYaw();
                    AdvancedOSCYaw();
                }
                else {
                    AdvancedOSCYaw();
                    AdvancedControllerYaw();
                }
            }
        }
        else if (gameManager.yaw[0] == 31) {
            if (gameManager.yaw[1] == 11) {
                if (Mathf.Abs(mouseY) > Mathf.Abs(joyY))
                    AdvancedMKYaw();
                else {
                    AdvancedMKYaw();
                    AdvancedJoystickYaw();
                }
            }
            else if (gameManager.yaw[1] == 21) {
                if (Mathf.Abs(conY) > Mathf.Abs(joyY))
                    AdvancedControllerYaw();
                else {
                    AdvancedControllerYaw();
                    AdvancedJoystickYaw();
                }
            }
            else if (gameManager.yaw[1] == 41) {
                if (Mathf.Abs(oscYaw) > Mathf.Abs(joyY)) {
                    AdvancedJoystickYaw();
                    AdvancedOSCYaw();
                }
                else {
                    AdvancedOSCYaw();
                    AdvancedJoystickYaw();
                }
            }
        }
        else if (gameManager.yaw[0] == 41) {
            if (gameManager.yaw[1] == 11) {
                if (Mathf.Abs(mouseY) > Mathf.Abs(oscYaw)) {
                    AdvancedOSCYaw();
                    AdvancedMKYaw();
                }
                else {
                    AdvancedMKYaw();
                    AdvancedOSCYaw();
                }
            }
            else if (gameManager.yaw[1] == 21) {
                if (Mathf.Abs(conY) > Mathf.Abs(oscYaw)) {
                    AdvancedOSCYaw();
                    AdvancedControllerYaw();
                }
                else {
                    AdvancedControllerYaw();
                    AdvancedOSCYaw();
                }
            }
            else if (gameManager.yaw[1] == 31) {
                if (Mathf.Abs(joyY) > Mathf.Abs(oscYaw)) {
                    AdvancedOSCYaw();
                    AdvancedJoystickYaw();
                }
                else {
                    AdvancedJoystickYaw();
                    AdvancedOSCYaw();
                }
            }
        }
        #endregion

        #region Roll Controls
        if (gameManager.roll[0] == 12) {
            if (gameManager.roll[1] == 22) {
                if (Mathf.Abs(conR) > Mathf.Abs(mouseR))
                    AdvancedControllerRoll();
                else {
                    AdvancedControllerRoll();
                    AdvancedMKRoll();
                }
            }
            else if (gameManager.roll[1] == 32) {
                if (Mathf.Abs(joyR) > Mathf.Abs(mouseR))
                    AdvancedJoystickRoll();
                else {
                    AdvancedJoystickRoll();
                    AdvancedMKRoll();
                }
            }
            else if (gameManager.roll[1] == 42) {
                if (Mathf.Abs(oscRoll) > Mathf.Abs(mouseR)) {
                    AdvancedMKRoll();
                    AdvancedOSCRoll();
                }
                else {
                    AdvancedOSCRoll();
                    AdvancedMKRoll();
                }
            }
        }
        else if (gameManager.roll[0] == 22) {
            if (gameManager.roll[1] == 12) {
                if (Mathf.Abs(mouseR) > Mathf.Abs(conR))
                    AdvancedMKRoll();
                else {
                    AdvancedMKRoll();
                    AdvancedControllerRoll();
                }
            }
            else if (gameManager.roll[1] == 32) {
                if (Mathf.Abs(joyR) > Mathf.Abs(conR))
                    AdvancedJoystickRoll();
                else {
                    AdvancedJoystickRoll();
                    AdvancedControllerRoll();
                }
            }
            else if (gameManager.roll[1] == 42) {
                if (Mathf.Abs(oscRoll) > Mathf.Abs(conR)) {
                    AdvancedControllerRoll();
                    AdvancedOSCRoll();
                }
                else {
                    AdvancedOSCRoll();
                    AdvancedControllerRoll();
                }
            }
        }
        else if (gameManager.roll[0] == 32) {
            if (gameManager.roll[1] == 12) {
                if (Mathf.Abs(mouseR) > Mathf.Abs(joyR))
                    AdvancedMKRoll();
                else {
                    AdvancedMKRoll();
                    AdvancedJoystickRoll();
                }
            }
            else if (gameManager.roll[1] == 22) {
                if (Mathf.Abs(conR) > Mathf.Abs(joyR))
                    AdvancedControllerRoll();
                else {
                    AdvancedControllerRoll();
                    AdvancedJoystickRoll();
                }
            }
            else if (gameManager.roll[1] == 42) {
                if (Mathf.Abs(oscRoll) > Mathf.Abs(joyR)) {
                    AdvancedJoystickRoll();
                    AdvancedOSCRoll();
                }
                else {
                    AdvancedOSCRoll();
                    AdvancedJoystickRoll();
                }
            }
        }
        else if (gameManager.roll[0] == 42) {
            if (gameManager.roll[1] == 12) {
                if (Mathf.Abs(mouseR) > Mathf.Abs(oscRoll)) {
                    AdvancedOSCRoll();
                    AdvancedMKRoll();
                }
                else {
                    AdvancedMKRoll();
                    AdvancedOSCRoll();
                }
            }
            else if (gameManager.roll[1] == 22) {
                if (Mathf.Abs(conR) > Mathf.Abs(oscRoll)) {
                    AdvancedOSCRoll();
                    AdvancedControllerRoll();
                }
                else {
                    AdvancedControllerRoll();
                    AdvancedOSCRoll();
                }
            }
            else if (gameManager.roll[1] == 32) {
                if (Mathf.Abs(joyR) > Mathf.Abs(oscRoll)) {
                    AdvancedOSCRoll();
                    AdvancedJoystickRoll();
                }
                else {
                    AdvancedJoystickRoll();
                    AdvancedOSCRoll();
                }
            }
        }
        #endregion

        #region Throttle Controls
        if (gameManager.throttle[0] == 13) {
            if (gameManager.throttle[1] == 23) {
                UpdateKeyboardThrottle(MKaccel, MKdecel);
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
            }
            else if (gameManager.throttle[1] == 33) {
                UpdateKeyboardThrottle(MKaccel, MKdecel);
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
            }
            else if (gameManager.throttle[1] == 43) {
                UpdateKeyboardThrottle(MKaccel, MKdecel);
                UpdateOSCThrottle();
            }
        }
        else if (gameManager.throttle[0] == 23) {
            if (gameManager.throttle[1] == 13) {
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
                UpdateKeyboardThrottle(MKaccel, MKdecel);
            }
            else if (gameManager.throttle[1] == 33) {
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
            }
            else if (gameManager.throttle[1] == 43) {
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
                UpdateOSCThrottle();
            }
        }
        else if (gameManager.throttle[0] == 33) {
            if (gameManager.throttle[1] == 13) {
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
                UpdateKeyboardThrottle(MKaccel, MKdecel);
            }
            else if (gameManager.throttle[1] == 23) {
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
            }
            else if (gameManager.throttle[1] == 43) {
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
                UpdateOSCThrottle();
            }
        }
        else if (gameManager.throttle[0] == 43) {
            if (gameManager.throttle[1] == 13) {
                UpdateOSCThrottle();
                UpdateKeyboardThrottle(MKaccel, MKdecel);
            }
            else if (gameManager.throttle[1] == 23) {
                UpdateOSCThrottle();
                UpdateKeyboardThrottle(controllerAccel, controllerDecel);
            }
            else if (gameManager.throttle[1] == 33) {
                UpdateOSCThrottle();
                UpdateKeyboardThrottle(joystickAccel, joystickDecel);
            }
        }
        #endregion
    }

    #region Stuff that doesn't work or is old
    /*public void UpdateYawKeyCodes(KeyCode leftInput, KeyCode rightInput) {
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), gameManager.MKyawLeft.text);
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), gameManager.MKyawRight.text);
    }*/

    //takes two variables, left+right input, then gives out the keycode left and right - doesn't actually work but it looks nice
    /*public void YawInputToKeyCodeInput(string leftInput, string rightInput, out KeyCode left, out KeyCode right) {
        char[] l = leftInput.ToCharArray();
        char[] r = rightInput.ToCharArray();
        left = (KeyCode)l[0];              // It just works - Todd Howard
        right = (KeyCode)r[0];
    }*/

    //don't think this actually works ):
    /*public void ThrottleInputToKeyCode(string accelInput, string decelInput) {
        char[] accelKey = accelInput.ToCharArray();
        char[] decelKey = decelInput.ToCharArray();
        KeyCode accel = (KeyCode)accelKey[0];              // It just works - Todd Howard
        KeyCode decel = (KeyCode)decelKey[0];
        UpdateKeyboardThrottle(accel, decel);
    }*/

    /*public void SetStickCommandsUsingMouse() {
        Vector3 mousePos = Input.mousePosition;

        // Figure out mouse position relative to center of screen.
        // (0, 0) is center, (-1, -1) is bottom left, (1, 1) is top right.      
        pitch = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height* 0.5f);        //negative makes it inverted
        roll = -(mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);


        // Make sure the values don't exceed limits.
        pitch = -Mathf.Clamp(pitch, -1.0f, 1.0f);
        yaw = Mathf.Clamp(yaw, -1.0f, 1.0f);
        roll = Mathf.Clamp(roll, -1.0f, 1.0f);
    }*/
    #endregion
}