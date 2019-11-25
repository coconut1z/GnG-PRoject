using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour {
    public CheckpointController checkpointController;

    void Start() {
        
    }

    void Update() {
        transform.LookAt(checkpointController.currentCheckpoint.transform);
    }
}
