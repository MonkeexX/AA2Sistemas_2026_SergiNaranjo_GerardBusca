using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomList : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPrefab; // Tu botón plantilla
    public Transform buttonContainer; // Panel donde se van a instanciar los botones

    // Lista de servidores (hardcodeada por ahora)
    private string[] servers = { "Servidor 1", "Servidor 2", "Servidor 3", "Servidor 4" };

    // Espaciado entre botones
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

            // Instanciar un nuevo botón
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);

            // Cambiar el texto del botón
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = "Room " + serverName;
            }

            // Agregar listener al botón
            Button buttonComponent = newButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                string server = serverName; // Evitar problema de closure
                buttonComponent.onClick.AddListener(() => OnServerSelected(server));
            }

            // Posicionar el botón manualmente
            RectTransform rt = newButton.GetComponent<RectTransform>();
            if (rt != null)
            {
                // Anchors centrados
                rt.anchorMin = new Vector2(0.5f, 1f);
                rt.anchorMax = new Vector2(0.5f, 1f);
                rt.pivot = new Vector2(0.5f, 1f);

                // Posición relativa al panel
                rt.anchoredPosition = new Vector2(0f, -i * buttonSpacing);
            }
        }

    }

    void OnServerSelected(string serverName)
    {
        Debug.Log("Servidor seleccionado: " + serverName);
    }
}
