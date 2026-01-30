using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ReplayManager : MonoBehaviour
{
    public ScreenRecorderUI videoPlayer;
    public Transform buttonContainer;    // Panel donde se generarán los botones
    public Button buttonPrefab;          // Prefab de botón
    public string replayFolder = "";     // Carpeta dentro de persistentDataPath (puede estar vacía)

    private int replayCounter = 0; // Contador de replays

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
        // Incrementar el contador
        replayCounter++;
        string buttonText = "Replay " + replayCounter;

        // Instanciar el botón
        Button b = Instantiate(buttonPrefab);
        b.transform.SetParent(buttonContainer, false);

        // Configurar texto y función
        b.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        string capturedFileName = fileName; // Evitar closure
        b.onClick.AddListener(() => videoPlayer.PlayReplay(capturedFileName));
    }
}
