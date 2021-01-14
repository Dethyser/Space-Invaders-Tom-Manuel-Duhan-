using UnityEngine;

public class PlayerController : MonoBehaviour {

    float input;
    public float movementSpeed = 5f;

    public string axisName;

    private Rigidbody2D rb;
    public GameObject shotPrefab;

    private float shootCooldown;

    private bool gameStart = false;
    private bool endGame = false;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(shootCooldown > 0.0f) {

            shootCooldown -= Time.deltaTime;
        }
        if (!gameStart && Input.GetButtonDown("Enter")) {

            gameStart = true;
        }
        if (gameStart && !endGame) {

            input = Input.GetAxisRaw(axisName);
            if (Input.GetButton("Jump") && shootCooldown <= 0.0f) {
                shootCooldown = 0.5f;
                SpawnShot();
            }
        }
    }

    private void FixedUpdate() {

        rb.velocity = Vector2.right * input * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Block")) {
            return;
        }

        GameObject.Find("Spawner").GetComponent<Spawner>().EndGame();
        Destroy(gameObject);
    }

    private void SpawnShot() {

        Instantiate(shotPrefab, transform.position + Vector3.up, Quaternion.identity);
    }

    public void EndGame() {

        endGame = true;
    }
}
