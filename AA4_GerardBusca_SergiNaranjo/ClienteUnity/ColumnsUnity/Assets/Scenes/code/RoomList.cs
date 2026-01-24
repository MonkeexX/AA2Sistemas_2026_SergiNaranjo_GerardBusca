using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomList : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    private string[] servers = { "Servidor 1", "Servidor 2", "Servidor 3", "Servidor 4" };
    public float buttonSpacing = 60f;

    void Start()
    {
        PopulateServerList();
    }

    void PopulateServerList()
    {
        for (int i = 0; i < servers.Length; i++)
        {
            string serverName = servers[i];

            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
                buttonText.text = "Room " + serverName;

            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                string server = serverName;
                buttonComponent.onClick.AddListener(() => OnServerSelected(server));
            }

            RectTransform rt = newButton.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.5f, 1f);
                rt.anchorMax = new Vector2(0.5f, 1f);
                rt.pivot = new Vector2(0.5f, 1f);
                rt.anchoredPosition = new Vector2(0f, -i * buttonSpacing);
            }
        }
    }

    void OnServerSelected(string serverName)
    {
        Debug.Log("Servidor seleccionado: " + serverName);

        // Guardamos el nombre de la room
        RoomData.roomName = "Room " + serverName;

        // Cargamos la escena Room
        SceneManager.LoadScene("Room");
    }
}

