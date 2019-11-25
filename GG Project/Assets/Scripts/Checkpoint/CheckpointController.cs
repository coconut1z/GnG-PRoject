using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine;


public class CheckpointController : MonoBehaviour {
    public int checkpointID;
    public Checkpoint[] checkpointList;    
    public Checkpoint currentCheckpoint;
    public Checkpoint nextCheckpoint;

    public float timer, startTime, endTime;
    public bool timerStarted;
    public Text timerText;

    void Start() {
        checkpointID = 0;
        currentCheckpoint = checkpointList[checkpointID];   //the first checkpoint in the list
        nextCheckpoint = checkpointList[checkpointID + 1];  //the checkpoint after the current checkpoint, which is the second
        currentCheckpoint.NextCheckpointMaterial();
        timerStarted = false;
        timerText.text = string.Format("Checkpoint: {0}/{1}\nTime: {2}", checkpointID, checkpointList.Length,timer.ToString("0.00"));
    }

    void Update() {
        UpdateTimer();
        UpdateText();

        if (currentCheckpoint.activated) {
            if (!timerStarted) {
                StartTimer();
            }            
            if (checkpointID < checkpointList.Length) {
                checkpointID++; //It just works - Todd Howard
            }            
            if (checkpointID >= checkpointList.Length) {
                StopTimer();
            }
            else if (checkpointID < checkpointList.Length) {                
                SetNextCheckpoint(checkpointList[checkpointID]);
                if (checkpointID < checkpointList.Length - 1) {
                    SetNextNextCheckpoint(checkpointList[checkpointID + 1]);
                }                
            }         
        }
    }

    public void SetNextCheckpoint(Checkpoint newCheckpoint) {
        currentCheckpoint = newCheckpoint;  //once the current checkpoint has been triggered, set the next one as the new current one
        currentCheckpoint.NextCheckpointMaterial();
    }

    public void SetNextNextCheckpoint(Checkpoint newCheckpoint) {
        nextCheckpoint = newCheckpoint;     //once the current checkpoint has been triggered, set the next next checkpoint
    }

    public void StartTimer() {
        timerStarted = true;
        startTime = Time.time;        
    }

    public void StopTimer() {
        timerStarted = false;
        endTime = timer;
    }

    public void UpdateTimer() {
        if (timerStarted) {
            timer = (float)Math.Round((Time.time - startTime), 2);
        }        
    }

    public void UpdateText() {
        timerText.text = string.Format("Checkpoint: {0}/{1}\nTime: {2}", checkpointID, checkpointList.Length, timer.ToString("0.00"));
    }
}
