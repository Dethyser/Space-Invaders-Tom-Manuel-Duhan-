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

    private void OnMouseDown() {
        Debug.Log("Hi");
    }
    private void OnCollisionEnter2D(Collision2D collision) {

        Destroy(gameObject);
    }
}
