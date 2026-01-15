using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RecordingButtons : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private SocketIOController socketIO;
    [SerializeField] private NodeGrid grid;

    private const float ButtonSpacing = 100f;

    #region ===== PUBLIC API =====

    public void GenerateButtons(IEnumerable<int> ids)
    {
        ClearButtons();

        int index = 0;
        foreach (int id in ids)
        {
            CreateButton(id, index);
            index++;
        }
    }

    #endregion

    #region ===== BUTTON CREATION =====

    private void CreateButton(int recordId, int index)
    {
        GameObject buttonGO = Instantiate(buttonPrefab, transform);
        buttonGO.transform.localPosition = CalculatePosition(index);

        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClicked(recordId));
    }

    private Vector2 CalculatePosition(int index)
    {
        return Vector2.down * ButtonSpacing * index;
    }

    #endregion

    #region ===== BUTTON ACTIONS =====

    private void OnButtonClicked(int recordId)
    {
        socketIO.GetSocket().Emit("UnityTest", $"CALL GetUpdatesByRoomId({recordId})");

        ClearButtons();
        StartCoroutine(WaitForUpdatesAndPlay());
    }

    #endregion

    #region ===== COROUTINES =====

    private IEnumerator WaitForUpdatesAndPlay()
    {
        yield return new WaitUntil(() => grid.updates.Count > 0);
        grid.StartRecording();
    }

    #endregion

    #region ===== CLEANUP =====

    private void ClearButtons()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    #endregion
}
