using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayLists : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject listReplays;

    public void OpenReplayList()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (listReplays != null) listReplays.SetActive(true);
    }
}
