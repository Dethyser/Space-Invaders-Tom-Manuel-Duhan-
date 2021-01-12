using UnityEngine;

public class PlayerController : MonoBehaviour {

    float input;
    public float movementSpeed = 5f;

    public string axisName;

    private Rigidbody2D rb;

    public GameObject shotPrefab;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
            input = Input.GetAxisRaw(axisName);
        if (Input.GetButtonDown("Jump")) {

            SpawnShot();
        }
    }

    private void FixedUpdate() {

        rb.velocity = Vector2.right * input * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
    
    }

    private void SpawnShot() {

        Instantiate(shotPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
