using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public string fileName; //dataset

    public bool isPlay = false;

    private CsvRead csvData;
    public GameObject Play_button;

    List<double> phi = new List<double>();
    List<double> theta = new List<double>();
    List<double> psi = new List<double>();

    private int frame;
    private const int EndData = -99;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        csvData = gameObject.AddComponent<CsvRead>();
        Play_button = GameObject.Find("PlayButton");
        Debug.Log(fileName);
        csvData.csvRead(fileName);
        for (int i = 0; i < csvData.csvDatas.Count; i++)
        {
            //Debug.Log("csv data[0]: " + csvData.csvDatas[i][0]);
            //Debug.Log("[1]: "+csvData.csvDatas[i][1]);
            //Debug.Log("[2]: " + csvData.csvDatas[i][2]);
            phi.Add(double.Parse(csvData.csvDatas[i][0]));
            theta.Add(double.Parse(csvData.csvDatas[i][1]));
            psi.Add(double.Parse(csvData.csvDatas[i][2]));
        }
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
                    //rotate using phi, theta and psi caalculated by Madgwick
                    transform.rotation = Quaternion.Euler((float)-theta[frame], (float)-phi[frame], (float)-psi[frame]);
                }
                catch
                {
                    frame = EndData;
                    isPlay = false;
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
}
