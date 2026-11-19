using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [Header("Einstellungen")]
    public float speed = 5f;
    Rigidbody2D body;
    Vector2 direction;

    void Awake() { body = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        float x = 0, y = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x--;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x++;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) y--;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) y++;
        direction = new Vector2(x, y).normalized;
    }

    void FixedUpdate() { body.linearVelocity = direction * speed; }
    void OnDisable() { if (body) body.linearVelocity = Vector2.zero; }
}
