using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Video;

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

public class ScreenRecorderUI : MonoBehaviour
{
    public RawImage display;          // UI donde se mostrará el video
    public float frameRate = 10f;     // FPS de reproducción

    private RecordingData recording;
    private Texture2D videoTexture;
    private Coroutine playingCoroutine;

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

    public void PlayReplay(string jsonFileName)
    {
        StopReplay();

        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("No se encontró el JSON en: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        recording = JsonUtility.FromJson<RecordingData>(json);

        if (recording.frames.Count == 0)
        {
            Debug.LogWarning("JSON vacío: " + jsonFileName);
            return;
        }

        int pixelCount = recording.frames[0].pixels.Length;
        int size = Mathf.RoundToInt(Mathf.Sqrt(pixelCount)); // Asumimos cuadrado
        videoTexture = new Texture2D(size, size, TextureFormat.RGB24, false);
        display.texture = videoTexture;

        
        playingCoroutine = StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        foreach (FrameData frame in recording.frames)
        {
            Color[] colors = new Color[frame.pixels.Length];
            for (int i = 0; i < frame.pixels.Length; i++)
            {
                if (jewelColors.TryGetValue(frame.pixels[i], out Color c))
                    colors[i] = c;
                else
                    colors[i] = Color.black;
            }

            videoTexture.SetPixels(colors);
            videoTexture.Apply();

            yield return new WaitForSeconds(1f / frameRate);
        }

        // Aquí se termina la reproducción
        gameObject.SetActive(false); // Desactiva el GameObject
    }

    public void StopReplay()
    {
        if (playingCoroutine != null)
        {
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }
    }
}