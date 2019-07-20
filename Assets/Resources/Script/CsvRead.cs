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
