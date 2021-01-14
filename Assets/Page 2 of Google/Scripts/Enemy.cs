using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Spawner spawner;
    [SerializeField]private GameObject enemyShotPrefab;

    public void SetSpawner(Spawner spawner) {

        this.spawner = spawner;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        spawner.EnemyGotDestroyed();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if(GameObject.Find("Player") != null) {
            GameObject.Find("Player").GetComponent<PlayerController>().EndGame();
        }
        GameObject.Find("Spawner").GetComponent<Spawner>().EndGame();
    }

    public void MoveLeft() {
        transform.position += new Vector3(-0.1f, 0, 0);
    }

    public void MoveDown() {
        transform.position += new Vector3(0, -0.3f, 0);
    }

    public void MoveRight() {
        transform.position += new Vector3(0.1f, 0, 0);
    }

    public void Shoot() {


        Instantiate(enemyShotPrefab, transform.position - Vector3.up, Quaternion.identity);
    }
}
