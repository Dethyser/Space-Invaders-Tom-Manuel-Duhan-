using UnityEngine;

public class ShotController : MonoBehaviour {

    public float movementSpeed = 5f;

    private Rigidbody2D rb;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        rb.velocity = Vector2.up * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

    }
}
