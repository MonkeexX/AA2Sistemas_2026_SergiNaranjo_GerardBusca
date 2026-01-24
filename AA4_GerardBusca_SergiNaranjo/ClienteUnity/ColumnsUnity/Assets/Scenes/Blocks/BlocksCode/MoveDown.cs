using UnityEngine;

public class MoveDown : MonoBehaviour
{
    // Velocidad de movimiento hacia abajo
    public float speed = 3f;

    void Update()
    {
        if (transform.position.y > -4.5f)
            transform.position += Vector3.down * speed * Time.deltaTime;
    }
}

