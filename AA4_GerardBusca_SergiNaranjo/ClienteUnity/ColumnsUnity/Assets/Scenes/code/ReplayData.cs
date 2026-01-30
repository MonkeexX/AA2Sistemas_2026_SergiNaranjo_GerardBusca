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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
