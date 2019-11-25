using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {
    public Checkpoint[] checkpointList;
    private Checkpoint currentCheckpoint;
    private int checkpointID;

    void Start() {
        checkpointID = 0;
        SetCurrentCheckpoint(checkpointList[checkpointID]);
    }

    void Update() {
        
    }

    private void SetCurrentCheckpoint(Checkpoint newCheckpoint) {
        currentCheckpoint = newCheckpoint;
        checkpointID++;
    }
}
