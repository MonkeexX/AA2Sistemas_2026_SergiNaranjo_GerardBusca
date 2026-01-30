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

    void Start()
    {
        LoadReplays();
    }

    void LoadReplays()
    {
        string folder = Path.Combine(Application.persistentDataPath, replayFolder);
        if (!Directory.Exists(folder))
        {
            Debug.LogWarning("No existe la carpeta de replays: " + folder);
            return;
        }

        string[] files = Directory.GetFiles(folder, "*.json");

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);

            // Crear botón dinámicamente
            Button b = Instantiate(buttonPrefab, buttonContainer);
            b.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            b.onClick.AddListener(() => videoPlayer.PlayReplay(fileName));
        }
    }
}
