using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Recording : MonoBehaviour
{
    [Header("UI")]
    public Button stopButton;

    [Header("Grabación")]
    public int width = 320;
    public int height = 240;
    public float captureInterval = 0.1f; // Intervalo entre frames

    [Header("Manager")]
    public ReplayManager replayManager; // Referencia al ReplayManager

    private bool isRecording = false;
    private RecordingData recording = new RecordingData();
    private Coroutine recordingCoroutine;

    private static readonly Dictionary<string, Color> jewelColors = new()
    {
        {"NONE", Color.white },
        {"RED", Color.red },
        {"GREEN", Color.green },
        {"BLUE", Color.blue },
        {"YELLOW", Color.yellow },
        {"ORANGE", new Color(1f, 0.5f, 0f) },
        {"PURPLE", new Color(0.5f, 0f, 1f) },
        {"SHINY", Color.cyan }
    };

    void Start()
    {
        StartRecording();

        if (stopButton != null)
            stopButton.onClick.AddListener(StopRecording);
    }

    void StartRecording()
    {
        isRecording = true;
        recordingCoroutine = StartCoroutine(CaptureFramesCoroutine());
        Debug.Log("Grabación iniciada");
    }

    IEnumerator CaptureFramesCoroutine()
    {
        while (isRecording)
        {
            yield return new WaitForEndOfFrame();

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            Color32[] pixels = tex.GetPixels32();
            string[] pixelTypes = new string[pixels.Length];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixelTypes[i] = GetClosestJewelType(pixels[i]);
            }

            FrameData frameData = new FrameData { pixels = pixelTypes };
            recording.frames.Add(frameData);

            Destroy(tex);
            yield return new WaitForSeconds(captureInterval);
        }
    }

    string GetClosestJewelType(Color32 color)
    {
        string closest = "NONE";
        float minDistance = float.MaxValue;

        foreach (var kv in jewelColors)
        {
            float distance = ColorDistance(color, kv.Value);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = kv.Key;
            }
        }

        return closest;
    }

    float ColorDistance(Color32 c1, Color c2)
    {
        float dr = c1.r / 255f - c2.r;
        float dg = c1.g / 255f - c2.g;
        float db = c1.b / 255f - c2.b;
        return dr * dr + dg * dg + db * db;
    }

    void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        if (recordingCoroutine != null)
            StopCoroutine(recordingCoroutine);

        // Crear nombre único para el JSON usando timestamp
        string fileName = "Recording_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
        string folder = Path.Combine(Application.persistentDataPath, replayManager.replayFolder);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, fileName);
        string json = JsonUtility.ToJson(recording, true);
        File.WriteAllText(path, json);

        Debug.Log("Grabación detenida y guardada en: " + path);

        // Crear botón dinámico para reproducir esta grabación
        if (replayManager != null)
        {
            string capturedFileName = fileName; // Evitar closure
            replayManager.AddReplayButton(capturedFileName);
        }

        // Reiniciar la grabación para poder grabar otra vez
        recording = new RecordingData();
    }
}
