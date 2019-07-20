using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script set FPS 

public class Manager : MonoBehaviour
{   
    void Start()
    {
        QualitySettings.vSyncCount = 0; // turn off vSync
        Application.targetFrameRate = 60; // set FPS 
    }
    void Update(){ }
}
