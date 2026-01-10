using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLists : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject listLobby;

    public void OpenLobbyList()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (listLobby != null) listLobby.SetActive(true);
    }
}
