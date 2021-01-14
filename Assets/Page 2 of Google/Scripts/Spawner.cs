using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {

    [SerializeField] float xOffset = 1f;
    [SerializeField] float yOffset = 1f;

    [SerializeField] int row = 5;
    [SerializeField] int column = 11;

    private Enemy[,] enemies;
    [SerializeField] private Enemy[] enemyPrefabs;

    private int enemyCounter;

    private enum Gamestate {

        moveLeft,
        moveRight,
        moveDown,
    }
    
    private Gamestate gameState;
    private Gamestate previousGamestate;
    private int currentRow;
    private int currentColumn;
    private int stepCounter;
    private int clockwork = 0;
    private float shootCooldown;

    private bool gameStart = false;
    private bool endGame = false;

    private void Start() {

        enemies = new Enemy[row,column];
        enemyCounter = row * column;
        SpawnEnemies();
        gameState = Gamestate.moveRight;
        previousGamestate = Gamestate.moveRight;
        currentRow = 0;
        currentColumn = 0;
        stepCounter = 10;
}

    private void Update() {

        if (Input.GetButtonDown("Enter")) {

            gameStart = true;
        }

        if (endGame && Input.GetButtonDown("Enter")) {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (gameStart && !endGame) {
            if (shootCooldown > 0.0f) {

                shootCooldown -= Time.deltaTime;
            }
            else {

                RandomEnemyShot();
                shootCooldown = 1.0f;
            }

            if (clockwork < 5) {

                clockwork++;
            }
            else {
                if (currentRow == (row - 1) && currentColumn == (column - 1) && stepCounter >= 20) {

                    if (gameState != Gamestate.moveDown) {
                        gameState = Gamestate.moveDown;
                    }
                    else if (previousGamestate == Gamestate.moveLeft) {
                        gameState = Gamestate.moveRight;
                        stepCounter = 0;
                    }
                    else {
                        gameState = Gamestate.moveLeft;
                        stepCounter = 0;
                    }
                }
                else {

                    if (gameState != Gamestate.moveDown) {
                        previousGamestate = gameState;
                    }
                }

                switch (gameState) {

                    case Gamestate.moveLeft:
                        if (enemies[currentRow, currentColumn] != null) {

                            enemies[currentRow, currentColumn].MoveLeft();
                        }
                        break;
                    case Gamestate.moveRight:
                        if (enemies[currentRow, currentColumn] != null) {

                            enemies[currentRow, currentColumn].MoveRight();
                        }
                        break;
                    case Gamestate.moveDown:
                        if (enemies[currentRow, currentColumn] != null) {

                            enemies[currentRow, currentColumn].MoveDown();
                        }
                        break;
                    default:
                        break;
                }

                currentColumn++;
                if (currentColumn >= column) {

                    currentColumn = 0;
                    currentRow++;
                }
                if (currentRow >= row) {

                    currentRow = 0;
                    stepCounter++;
                }
                clockwork = 0;
            }
        }
    }

    private void SpawnEnemies() {


        Vector3 startPosition = transform.position - ((Vector3.right * xOffset * (column - 1)) + (Vector3.down * yOffset * (row - 1))) / 2f;

        Enemy newEnemy;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++) {
                newEnemy = Instantiate(
                    original: (y < enemyPrefabs.Length) ? enemyPrefabs[y] : enemyPrefabs[enemyPrefabs.Length - 1],
                    position: startPosition + (Vector3.right * xOffset * x) + (Vector3.down * yOffset * y),
                    rotation: Quaternion.identity
                    );

                enemies[y, x] = newEnemy;
                newEnemy.gameObject.name = $"Enemy({x}|{y})";

                newEnemy.SetSpawner(this);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.white;

        Vector3 startPosition = - ((Vector3.right * xOffset * (column - 1)) + (Vector3.down * yOffset * (row - 1))) / 2f;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++)
            {
                Gizmos.DrawWireCube(
                    center: startPosition + (Vector3.right * xOffset * x) + (Vector3.down * yOffset * y),
                    size: new Vector3(1f, 1f/12f*8f, 0)
                    );
            }
        }
    }

    public void EnemyGotDestroyed() {

        enemyCounter--;
        if(enemyCounter <= 0) {

            //Win
        }
    }

    private void RandomEnemyShot() {

        List<Enemy> aliveEnemies = new List<Enemy>(); 
        for (int y = 0; y < row; y++) {
            for (int x = 0; x < column; x++) {
                if(enemies[y,x] != null) {

                    aliveEnemies.Add(enemies[y, x]);
                }
            }
        }
        GetRandomLowestEnemy(aliveEnemies).Shoot();
    }

    private Enemy GetRandomLowestEnemy(List<Enemy> aliveEnemies) {
        int randomNumber = Random.Range(0, aliveEnemies.Count + 1);
        while (aliveEnemies[randomNumber] == null) {

            randomNumber = Random.Range(0, aliveEnemies.Count + 1);
        }

        return CheckLowestEnemy(aliveEnemies[randomNumber]);
    }

    private Enemy CheckLowestEnemy(Enemy randomEnemy) {
        int correctRow = 0;
        int correctColumn = 0;
        bool foundEnemy = false;
        for (int y = 0; y < row; y++) {
            for (int x = 0; x < column; x++) {
                if (enemies[y, x] != null && enemies[y, x] == randomEnemy) {

                    correctRow = y;
                    correctColumn = x;
                    foundEnemy = true;
                }
            }
        }
        if (!foundEnemy) {
            return randomEnemy;
        }
        for (int y = correctRow; y < row; y++) {
            if (enemies[y, correctColumn] != null) {

                correctRow = y;
            }
        }

        return enemies[correctRow, correctColumn];
    }

    public void EndGame() {

        endGame = true;
    }
}
