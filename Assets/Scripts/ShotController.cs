using UnityEngine;

public class ShotController : MonoBehaviour {

    public float movementSpeed = 5f;                            //the speed at which the shot moves

    private Rigidbody2D rb;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        rb.velocity = Vector2.up * movementSpeed;               //moves the shot upwards
    }

    private void OnCollisionEnter2D(Collision2D collision) {    //checks for collision

        Destroy(gameObject);                                    //destroys itself
    }
}
