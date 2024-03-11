using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPack
{
    public List<LineData> dataPack = new List<LineData>();
}

[System.Serializable]
public class LineData
{
    public List<LineDataModel> lineData = new List<LineDataModel>();
}

[System.Serializable]
public class LineDataModel
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public string color;
    public LinePathData linePathData;
}

[System.Serializable]
public class LinePathData
{
    public List<Vector3> linePath;

    public LinePathData()
    {
        linePath = new List<Vector3>();
    }
}