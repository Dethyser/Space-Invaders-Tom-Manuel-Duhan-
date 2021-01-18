using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Spawner spawner;                                //the spawner that instantiated this enemy

    private bool isDestroyed = false;                       //check if the enemy is destroyed, so it doesnt send his death-message twice

    [SerializeField]private GameObject enemyShotPrefab;     //the gameobject thats being shot by this enemy

    public void SetSpawner(Spawner spawner) {               //sets the spawner

        this.spawner = spawner;
    }

    private void OnCollisionEnter2D(Collision2D collision) {            //detects collision
        if (collision.gameObject.CompareTag("Shot") && !isDestroyed) {  //checks if the collided object has the tag "Shot" and if this enemy isnt destroyed yet

            isDestroyed = true;             
            spawner.EnemyGotDestroyed();                                //sends death-message to spawner
            Destroy(gameObject);                                        //destroyes itself
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {                           //detects trigger

        if(GameObject.Find("Player") != null) {                                     //checks if a player exist
            GameObject.Find("Player").GetComponent<PlayerController>().EndGame();   //sends player endgame-message
        }
        GameObject.Find("Spawner").GetComponent<Spawner>().EndGame();               //sends spawner endgame-message
    }

    public void MoveLeft() {
        transform.position += new Vector3(-0.15f, 0, 0);                            //moves to the left by 0.15
    }

    public void MoveDown() {
        transform.position += new Vector3(0, -0.45f, 0);    //moves down by 0.45
    }

    public void MoveRight() {
        transform.position += new Vector3(0.15f, 0, 0);     //moves to the right by 0.15
    }

    public void Shoot() {


        Instantiate(enemyShotPrefab, transform.position - Vector3.up, Quaternion.identity); //spawns a shot slightly under the enemy
    }
}
