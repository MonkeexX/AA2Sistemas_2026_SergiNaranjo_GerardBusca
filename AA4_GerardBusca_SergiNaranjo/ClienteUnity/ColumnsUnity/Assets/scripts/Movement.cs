using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public bool useLocalInput = true;

    private bool canMove = true;

    void Update()
    {
        if (!canMove) return;

        Vector2 dir = Vector2.zero;

        if (useLocalInput && MovementTest.Instance != null)
        {
            dir = MovementTest.Instance.CurrentDirection;
        }
        else if (!useLocalInput && SocketIOController.Instance != null)
        {
            dir = SocketIOController.Instance.CurrentDirection;
        }

        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            Debug.Log("Colisión con bloqueador. Movimiento bloqueado.");
            canMove = false;
        }
    }
}
