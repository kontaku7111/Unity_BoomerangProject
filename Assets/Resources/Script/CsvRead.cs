///////////////////////////////////////////////////////
// (c) 2019 Darwin Boomerangs                         /
// Smart -Boomerang Project                           /
// In partnership with :                              /
//   - University of St Etienne, France               /
//   - Aoyama Gakuin University / Lopez lab, Japan    /
///////////////////////////////////////////////////////

///////////////////////////////////////////////////////
// Filename    CsvRead.cs                             /
// This file is intended to read csv files in csv     /
// folder.                                            /
//                                                    /
///////////////////////////////////////////////////////
// v1.0  Date July 23th, 2019     Author Takumi Kondo /
// Modifications from previous version...             /
// ...                                                /
///////////////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CsvRead : MonoBehaviour
{
    public List<string[]> csvDatas = new List<string[]>();
       
    public void csvRead(string fileName)
    {
        //read a csv file from Resources/csv/
        TextAsset csv = Resources.Load("csv/"+fileName) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // read line by line
            csvDatas.Add(line.Split(','));
        }
        reader.Close();

    }
    // clear datas in csvDatas
    public void clearData()
    {
        csvDatas.Clear();
        Debug.Log(csvDatas.Count);
    }
}
