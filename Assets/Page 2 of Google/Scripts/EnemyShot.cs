using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour {

    public float movementSpeed = 10f;

    private Rigidbody2D rb;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        rb.velocity = -Vector2.up * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        Destroy(gameObject);
    }
}
