using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public bool activated;
    public bool ignoreCheck;
    public Checkpoint previousCheckpoint;
    public Material activatedMat;
    public Material nextCheckpointMat;
    public int children;
    public ShipInput ship;

    void Start() {
        activated = false;
        children = transform.childCount;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (ignoreCheck) {
                activated = true;
                UpdateMaterial();
                ship.recentCheckpoint = gameObject;
            }
            else if (!ignoreCheck && previousCheckpoint.activated) {
                activated = true;
                UpdateMaterial();
                ship.recentCheckpoint = gameObject;
            }
        }
    }

    public void UpdateMaterial() {
        for (int i = 0; i < children; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<Renderer>().material = activatedMat;
        }
    }

    public void NextCheckpointMaterial() {
        for (int i = 0; i < children; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponent<Renderer>().material = nextCheckpointMat;
        }
    }
}
