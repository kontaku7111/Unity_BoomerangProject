///////////////////////////////////////////////////////
// (c) 2019 Darwin Boomerangs                         /
// Smart -Boomerang Project                           /
// In partnership with :                              /
//   - University of St Etienne, France               /
//   - Aoyama Gakuin University / Lopez lab, Japan    /
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
// Filename    Display.cs                             /
// This file is intended to display boomerang detail  /
// status and rotate a boomerang object using csv     /
// files (accMPU, gyroMPU, mag)                       /
///////////////////////////////////////////////////////
// v1.0  Date July 23th, 2019     Author Takumi Kondo /
// Modifications from previous version...             /
// ...                                                /
///////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public bool isPlay = false;

    private CsvRead csvData;
    public GameObject Play_button;
    //////add//////
    public GameObject RotationSpeed;
    public GameObject ThrowingSpeed;
    public GameObject BoomerangStatus;
    public GameObject Inclination;
    //////////////

    List<double> phi = new List<double>();
    List<double> theta = new List<double>();
    List<double> psi = new List<double>();

    private int frame;
    private const int EndData = -99;
    //////add new codes//////
    private int update_frequency = 1;
    public double maxSpeed = 0;
    public float rotationSpeed = 0;
    public float norm = 0;
    // These are variables stored sensor data
    public List<double> xAcc = new List<double>();
    public List<double> yAcc = new List<double>();
    public List<double> zAcc = new List<double>();
    public List<double> xGyro = new List<double>();
    public List<double> yGyro = new List<double>();
    public List<double> zGyro = new List<double>();
    public List<double> xMag = new List<double>();
    public List<double> yMag = new List<double>();
    public List<double> zMag = new List<double>();
    
    //dataset
    //These dataset were resampled from original sampling rate to 60 Hz using MATLAB
    //Furthermore, Acc unit was converted into G (original unit: mG)
    //Mag unit was converted into G (original unit: μT, 100μT=1G) 
    public string acc_dataset; //dataset of accelerometer
    public string gyro_dataset; //dataset of gyroscope
    public string mag_dataset; //dataset of magnet
    public string euler_dataset; //dataset of euler

    //state determination
    public bool isFlight = false;

    // use for Madgwick filter
    List<double> roll = new List<double>();
    List<double> pitch = new List<double>();
    List<double> yaw = new List<double>();
    float[] euler = new float[3];

    static AHRS.MadgwickAHRS AHRS;

    ////// //////

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        csvData = gameObject.AddComponent<CsvRead>();
        Play_button = GameObject.Find("PlayButton");

        //////Add new codes//////
        RotationSpeed = GameObject.Find("RotationSpeed");
        BoomerangStatus = GameObject.Find("BoomerangStatus");
        Inclination = GameObject.Find("Inclination");
        ThrowingSpeed = GameObject.Find("ThrowingSpeed");

        frame = 0;
        //read sensor data from csv file
        csvData.csvRead(acc_dataset);
        //Accelerometer unit is not mG 
        for (int i = 0; i < csvData.csvDatas.Count; i++)
        {
            //Debug.Log("csv data[0]: " + csvData.csvDatas[i][0]);
            //Debug.Log("[1]: "+csvData.csvDatas[i][1]);
            //Debug.Log("[2]: " + csvData.csvDatas[i][2]);
            xAcc.Add(double.Parse(csvData.csvDatas[i][0]));
            yAcc.Add(double.Parse(csvData.csvDatas[i][1]));
            zAcc.Add(double.Parse(csvData.csvDatas[i][2]));
        }
        csvData.clearData();

        csvData.csvRead(gyro_dataset);
        for (int i = 0; i < csvData.csvDatas.Count; i++)
        {
            //Debug.Log("csv data[0]: " + csvData.csvDatas[i][0]);
            //Debug.Log("[1]: "+csvData.csvDatas[i][1]);
            //Debug.Log("[2]: " + csvData.csvDatas[i][2]);
            xGyro.Add(double.Parse(csvData.csvDatas[i][0]));
            yGyro.Add(double.Parse(csvData.csvDatas[i][1]));
            zGyro.Add(double.Parse(csvData.csvDatas[i][2]));
        }
        csvData.clearData();

        csvData.csvRead(mag_dataset);
        for (int i = 0; i < csvData.csvDatas.Count; i++)
        {
            //Debug.Log("csv data[0]: " + csvData.csvDatas[i][0]);
            //Debug.Log("[1]: "+csvData.csvDatas[i][1]);
            //Debug.Log("[2]: " + csvData.csvDatas[i][2]);
            xMag.Add(double.Parse(csvData.csvDatas[i][0]));
            yMag.Add(double.Parse(csvData.csvDatas[i][1]));
            zMag.Add(double.Parse(csvData.csvDatas[i][2]));
        }
        csvData.clearData();

        csvData.csvRead(euler_dataset);
        for (int i = 0; i < csvData.csvDatas.Count; i++)
        {
            //Debug.Log("csv data[0]: " + csvData.csvDatas[i][0]);
            //Debug.Log("[1]: "+csvData.csvDatas[i][1]);
            //Debug.Log("[2]: " + csvData.csvDatas[i][2]);
            phi.Add(double.Parse(csvData.csvDatas[i][0]));
            theta.Add(double.Parse(csvData.csvDatas[i][1]));
            psi.Add(double.Parse(csvData.csvDatas[i][2]));
        }

        AHRS = new AHRS.MadgwickAHRS(1f / 60f, 0.1f);

        ///////////////
    }

    // Update is called once per frame
    void Update()
    {
        if (true == isPlay)
        {
            if (EndData != frame)
            {
                //in Unity, ZXY euler, while ZYX euler in Matlab
                // in Unity 
                try
                {
                    //calculate euler angle
                    AHRS.Update((float)xGyro[frame] * (Mathf.PI / 180), (float)yGyro[frame] * (Mathf.PI / 180), (float)zGyro[frame] * (Mathf.PI / 180), (float)xAcc[frame], (float)yAcc[frame], (float)zAcc[frame], (float)xMag[frame], (float)yMag[frame], (float)zMag[frame]);
                    euler = quatern2euler(qConj(AHRS.Quaternion));
                    euler[0] *= (180 / Mathf.PI);
                    euler[1] *= (180 / Mathf.PI);
                    euler[2] *= (180 / Mathf.PI);
                    roll.Add(euler[0]);
                    pitch.Add(euler[1]);
                    yaw.Add(euler[2]);

                    //calculate norm of acceleromater
                    norm = (float)(xAcc[frame] * xAcc[frame] + yAcc[frame] * yAcc[frame] + zAcc[frame] * zAcc[frame]);
                    //Judge whether the boomerang flights
                    if (Mathf.Sqrt(norm) > 50)
                    {
                        Debug.Log("achieved!");
                        if (60 == update_frequency)
                        {
                            // Display rotation speed of boomerang
                            RotationSpeed.GetComponent<Text>().text = "Rotation speed: " + Mathf.Abs((float)zGyro[frame]) + "degree/s";
                            //update once in 60 frame
                            update_frequency = 1;
                        }

                        // Detect throwing speed
                        if (maxSpeed < Mathf.Abs((float)zGyro[frame]))
                        {
                            maxSpeed = Mathf.Abs((float)zGyro[frame]);
                        }
                        //else if (maxSpeed > Mathf.Abs((float)zGyro[frame]))
                        //{
                        //    ThrowingSpeed.GetComponent<Text>().text = "Throwing speed: " + Mathf.Abs((float)maxSpeed) + "degree/s";
                        //}

                        if (!isFlight)
                        {
                            isFlight = true;
                            // Display boomerang status;
                            BoomerangStatus.GetComponent<Text>().text = "Boomerang status: Flight";
                        }
                        update_frequency++;
                    }
                    else
                    {
                        // Holding boomerang
                        //rotate using phi, theta and psi caalculated by Madgwick
                        //transform.rotation = Quaternion.Euler((float)-phi[frame], (float)-theta[frame], (float)-psi[frame]);
                        transform.rotation = Quaternion.Euler((float)-roll[frame], (float)-pitch[frame], (float)-yaw[frame]);

                        // Display inclination to horizontality
                        //Inclination.GetComponent<Text>().text = "Inclination to horizontality: " + (int)Mathf.Abs((float)phi[frame]) + "°, " + (int)Mathf.Abs((float)theta[frame]);
                        Inclination.GetComponent<Text>().text = "Angle from wind: " + (int)Mathf.Abs((float)roll[frame]) + "°\nAngle from vertical: " + (int)Mathf.Abs((float)pitch[frame]) + "°";
                        if (isFlight)
                        {
                            isFlight = false;
                            // Display boomerang status
                            BoomerangStatus.GetComponent<Text>().text = "Boomerang status: Holding";
                            ThrowingSpeed.GetComponent<Text>().text = "Throwing speed: " + Mathf.Abs((float)maxSpeed) + "degree/s";
                        }
                    }
                }
                catch
                {
                    frame = EndData;
                    isPlay = false;
                    maxSpeed = 0;
                    Play_button.GetComponentInChildren<Text>().text = "start";
                    Debug.Log("range out!!");
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                if (EndData != frame)
                {
                    frame++;
                }
            }
        }
    }

    //perform if a user push a play button
    public void clicPlayButton()
    {
        if (false == isPlay)
        {
            if (EndData == frame)
            {
                frame = 0;
            }
            isPlay = true;
            Play_button.GetComponentInChildren<Text>().text = "stop";
        }
        else
        {
            isPlay = false;
            Play_button.GetComponentInChildren<Text>().text = "start";
        }
    }

    public float[] qConj(float[] q)
    {
        float[] qj = { q[0], -q[1], -q[2], -q[3] };
        return qj;
    }

    public float[] quatern2euler(float[] q)
    {
        float R11, R21, R31, R32, R33;
        R11 = 2 * q[0] * q[0] - 1 + 2 * q[1] * q[1];
        R21 = 2 * (q[1] * q[2] - q[0] * q[3]);
        R31 = 2 * (q[1] * q[3] + q[0] * q[2]);
        R32 = 2 * (q[2] * q[3] - q[0] * q[1]);
        R33 = 2 * q[0] * q[0] - 1 + 2 * q[3] * q[3];
        float phi = Mathf.Atan2(R32, R33);
        float theta = -Mathf.Atan(R31) / (Mathf.Sqrt(1 - R31 * R31));
        float psi = Mathf.Atan2(R21, R11);
        float[] euler = { phi, theta, psi };
        return euler;
    }

}
