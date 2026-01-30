using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public static MovementTest Instance { get; private set; }

    public Vector2 CurrentDirection { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            CurrentDirection = Vector2.down;

        else if (Input.GetKeyDown(KeyCode.A))
            CurrentDirection = Vector2.left;

        else if (Input.GetKeyDown(KeyCode.D))
            CurrentDirection = Vector2.right;
    }
}
