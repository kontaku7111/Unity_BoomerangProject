///////////////////////////////////////////////////////
// (c) 2019 Darwin Boomerangs                         /
// Smart -Boomerang Project                           /
// In partnership with :                              /
//   - University of St Etienne, France               /
//   - Aoyama Gakuin University / Lopez lab, Japan    /
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
// Filename    Manager.cs                             /
// This file is intended to fix FPS of Unity          /
//                                                    /
//                                                    /
///////////////////////////////////////////////////////
// v1.0  Date July 23th, 2019     Author Takumi Kondo /
// Modifications from previous version...             /
// ...                                                /
///////////////////////////////////////////////////////

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
