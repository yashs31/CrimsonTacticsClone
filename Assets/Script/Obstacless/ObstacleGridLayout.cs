using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleGridLayout
{
    [System.Serializable]
    public struct rowData
    {
        public bool[] Y;
    }

    public rowData[] X= new rowData[10];

}
