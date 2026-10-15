using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Einstellungen")]
    public float speed = 5f;

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        // X-Achse (Links / Rechts)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }

        // Y-Achse (Oben / Unten)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }

        // Bewegung berechnen und anwenden
        Vector3 direction = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}