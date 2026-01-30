using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FrameData
{
    public string[] pixels;
}

[System.Serializable]
public class RecordingData
{
    public List<FrameData> frames = new List<FrameData>();
}

public class ReplayData : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
