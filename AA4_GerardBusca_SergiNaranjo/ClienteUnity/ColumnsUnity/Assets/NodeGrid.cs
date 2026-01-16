using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

enum ScreenState { REPLAYSELECTOR,WAITINGFORREPLAY,WATCHING }

public class NodeGrid : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private float timeToNextUpdate = 1f;

    [Header("External")]
    public SocketIOController controller;
    public RecordingButtons buttons;

    private ScreenState screenState = ScreenState.REPLAYSELECTOR;

    private Grid grid;
    private int currentUpdateIndex;

    public List<GridUpdate> updates = new();
    public List<int> recordingIDs = new();

    private Dictionary<Vector2Int, SpriteRenderer> nodeRenderers = new();

    #region ===== DATA MODELS =====

    [Serializable]
    public class Node
    {
        public enum JewelType
        {
            NONE, RED, GREEN, BLUE, YELLOW, ORANGE, PURPLE, SHINY
        }

        public int x;
        public int y;
        public JewelType type;

        public Node(JewelType type, int x, int y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]
    public class Grid
    {
        public int width;
        public int height;

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    [Serializable]
    public class GridUpdate
    {
        public int playerId;
        public string playerName;
        public List<Node> updateNodes = new();
    }

    #endregion

    #region ===== UNITY LIFECYCLE =====

    private void Start()
    {
        BuildGrid(6, 12);
        ClearRecording();
    }

    private void Update()
    {
        if (screenState == ScreenState.REPLAYSELECTOR && recordingIDs.Count > 0)
        {
            buttons.GenerateButtons(recordingIDs);
            screenState = ScreenState.WAITINGFORREPLAY;
        }
    }

    #endregion

    #region ===== GRID SETUP =====

    private void BuildGrid(int sizeX, int sizeY)
    {
        grid = new Grid(sizeX, sizeY);

        for (int x = 0; x < sizeX; x++)
        {
            Transform column = new GameObject($"Column_{x}").transform;
            column.SetParent(transform);
            column.localPosition = new Vector3(-sizeX / 2f + x, sizeY / 2f, 0);
            
            for (int y = 0; y < sizeY; y++)
            {
                GameObject nodeGO = Instantiate(nodePrefab, column);
                nodeGO.name = $"Node_{x}_{y}";
                nodeGO.transform.localPosition = new Vector3(0, -y, 0);

                var renderer = nodeGO.GetComponent<SpriteRenderer>();
                nodeRenderers[new Vector2Int(x, y)] = renderer;
            }
        }
    }

    #endregion

    #region ===== GRID UPDATE =====

    private static readonly Dictionary<Node.JewelType, Color> jewelColors = new()
    {
        {Node.JewelType.NONE, Color.white },
        {Node.JewelType.RED, Color.red },
        {Node.JewelType.GREEN, Color.green },
        {Node.JewelType.BLUE, Color.blue },
        {Node.JewelType.YELLOW, Color.yellow },
        {Node.JewelType.ORANGE, new Color(1f, 0.5f, 0f) },
        {Node.JewelType.PURPLE, new Color(0.5f, 0f, 1f) },
        {Node.JewelType.SHINY, Color.cyan }
    };

    public void ApplyGridUpdate(GridUpdate update)
    {
        foreach (var node in update.updateNodes)
        {
            Vector2Int key = new(node.x, node.y);

            if (!nodeRenderers.TryGetValue(key, out var renderer))
                continue;

            renderer.color = jewelColors[node.type];
        }
    }

    #endregion

    #region ===== RECORDING =====

    public void GenerateRecording(List<GridUpdate> incomingUpdates)
    {
        updates.Clear();
        updates.AddRange(incomingUpdates);
        StartPlayback();
    }

    public void ClearRecording()
    {
        updates.Clear();
    }

    public void AddUpdate(GridUpdate update)
    {
        updates.Add(update);
    }

    public void StartPlayback()
    {
        currentUpdateIndex = 0;
        StopAllCoroutines();
        StartCoroutine(PlaybackRoutine());
    }

    private IEnumerator PlaybackRoutine()
    {
        while (currentUpdateIndex < updates.Count)
        {
            ApplyGridUpdate(updates[currentUpdateIndex]);
            currentUpdateIndex++;
            yield return new WaitForSeconds(timeToNextUpdate);
        }
    }

    #endregion
}