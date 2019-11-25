using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows throttle and speed of the player ship.
/// </summary>
public class SpeedUI : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        float speed = Ship.PlayerShip.Velocity.magnitude * 2f;
        if (text != null && Ship.PlayerShip != null) {
            text.text = string.Format("THR: {0}\nSPD: {1}", (Ship.PlayerShip.Throttle * 100.0f).ToString("000"), speed.ToString("000"));
        }
    }
}
