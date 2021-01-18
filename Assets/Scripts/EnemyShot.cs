using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour {

    public float movementSpeed = 10f;       //how fast the shot travels

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {

        rb.velocity = -Vector2.up * movementSpeed;  //moves the shot downwards
    }

    private void OnCollisionEnter2D(Collision2D collision) {    //checks for collision

        Destroy(gameObject);                                    //destroys the shot if it collides with anything
    }
}
