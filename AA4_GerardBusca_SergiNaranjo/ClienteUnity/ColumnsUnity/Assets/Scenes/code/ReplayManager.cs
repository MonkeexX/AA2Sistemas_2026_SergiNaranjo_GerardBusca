using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ReplayManager : MonoBehaviour
{
    public ScreenRecorderUI videoPlayer;
    public Transform buttonContainer;
    public Button buttonPrefab;
    public string replayFolder = "";

    private int replayCounter = 0;

    void Start()
    {
        LoadReplays();
    }

    void LoadReplays()
    {
        string folder = Path.Combine(Application.persistentDataPath, replayFolder);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string[] files = Directory.GetFiles(folder, "*.json");

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);
            AddReplayButton(fileName);
        }
    }

    public void AddReplayButton(string fileName)
    {
        replayCounter++;
        string buttonText = "Replay " + replayCounter;

        Button b = Instantiate(buttonPrefab);
        b.transform.SetParent(buttonContainer, false);

        b.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        string capturedFileName = fileName;
        b.onClick.AddListener(() => videoPlayer.PlayReplay(capturedFileName));
    }
}
