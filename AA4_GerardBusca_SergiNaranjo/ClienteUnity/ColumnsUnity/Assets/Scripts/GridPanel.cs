using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPanel : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject gridPanel;

    public void OpenGridPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (gridPanel != null) gridPanel.SetActive(true);
    }
}
