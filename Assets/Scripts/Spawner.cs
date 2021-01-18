using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    [SerializeField] float xOffset = 1f;                //space between columns
    [SerializeField] float yOffset = 1f;                //space between rows

    [SerializeField] int row = 5;                       //amount of rows
    [SerializeField] int column = 11;                   //amount of column

    private Enemy[,] enemies;                           //2D-array of enemies that are spawned
    [SerializeField] private Enemy[] enemyPrefabs;      //array of all enemy-types

    [SerializeField] private Text scoreText;
    [SerializeField] private Text endText;

    private int score = 0;

    private int enemyCounter;                           //amount of entities in enemies[,] in line 15

    private enum Enemystate {                           //enum for different states for the movement of the enemies 

        moveLeft,
        moveRight,
        moveDown,
    }
    
    private Enemystate gameState;                       //current state
    private Enemystate previousGamestate;               //last state
    private int currentRow;
    private int currentColumn;
    private int stepCounter;                            //counter for how many steps the enemies took to the left/right
    private int clockwork = 0;                          //clockwork counter
    private float shootCooldown;                        //amount of time between 2 different shot from enemyside

    private bool gameStart = false;                     //checks if game has started
    private bool endGame = false;                       //checks if game had ended

    private void Start() {

        enemies = new Enemy[row,column];                //sets the 2D array with height = row and width = column
        enemyCounter = row * column;                    //calculates the amount of enemies
        SpawnEnemies();                                 //spawns enemies
        gameState = Enemystate.moveRight;               //sets starting state to moveRight
        previousGamestate = Enemystate.moveRight;       //sets previous state to moveRight
        currentRow = 0;
        currentColumn = 0;
        stepCounter = 10;

        scoreText.text = "0000";
}

    private void Update() {

        if (Input.GetButtonDown("Enter")) {                                     //checks if "enter" got pressed

            gameStart = true;                                                   //starts game
        }

        if (endGame && Input.GetButtonDown("Enter")) {                          //checks if the game has ended and wether "enter" got pressed

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   //loads the scene from anew 
        }
        if (gameStart && !endGame) {                                            //checks if the game has started and wether the game hasnt ended yet
            if (shootCooldown > 0.0f) {                                         //checks if shootcooldown is still above 0

                shootCooldown -= Time.deltaTime;                                //substracts the time since last frame from shootcooldown
            }
            else {

                RandomEnemyShot();                                              //lets random enemy shoot
                shootCooldown = 1.0f;                                           //sets cooldown to 1.0
            }

            if (clockwork < 5) {                                                //checks if clockwork counter is less than 5

                clockwork++;                                                    //increases clockwork by 1
            }
            else {
                MoveEnemy();                                                    //moves enemy
            }
        }
    }

        private void SpawnEnemies() {           //spawns an enemy at every position that is draw in the scene before starting playmode

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

    private void OnDrawGizmosSelected() {           //draws every position where an enemy will spawn
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
        enemyCounter--;                            //reduces enemy amount by 1


        score += 10;                               //increases score by 10
        if(score > 0 && score < 100)               //checks how many 0 have to be added to make the score at least 4 digits big
        {

            scoreText.text = "00" + score;
        }
        else if (score >= 100 && score < 1000) 
        {

            scoreText.text = "0" + score;
        }
        else 
        {

            scoreText.text = "" + score;
        }


        if (enemyCounter <= 0) {                //checks if enemycounter is less or equal to 0

            endText.text = "WINNER";            //set endtext to winner
            endGame = true;
        }
    }

    private void RandomEnemyShot() {

        List<Enemy> aliveEnemies = new List<Enemy>(); //makes a list
        for (int y = 0; y < row; y++) {               //adds every enemy that is still alive to the list
            for (int x = 0; x < column; x++) {
                if(enemies[y,x] != null) {

                    aliveEnemies.Add(enemies[y, x]);
                }
            }
        }
        if(aliveEnemies.Count > 0) {                   //checks if the list has atleast 1 enemy             

            GetRandomLowestEnemy(aliveEnemies).Shoot(); //chooses a random enemy from the list and makes it shoot
        }
    }

    private Enemy GetRandomLowestEnemy(List<Enemy> aliveEnemies) {
        int randomNumber = Random.Range(0, aliveEnemies.Count + 1);         //chooses a random number from 0 to the amount of enemies in the list
        while (aliveEnemies[randomNumber] == null) {                        //checks if the enemy died before it could shott

            randomNumber = Random.Range(0, aliveEnemies.Count + 1);         //chooses a new random number
        }

        return CheckLowestEnemy(aliveEnemies[randomNumber]);                //checks if there is an enemy below the choosen one so the enemies dont shoot each other
    }

    private Enemy CheckLowestEnemy(Enemy randomEnemy) {
        int correctRow = 0;
        int correctColumn = 0;
        bool foundEnemy = false;
        for (int y = 0; y < row; y++) {                                     //searches for the choosen enemy in the array and saves its position in that array
            for (int x = 0; x < column; x++) {
                if (enemies[y, x] != null && enemies[y, x] == randomEnemy) {

                    correctRow = y;
                    correctColumn = x;
                    foundEnemy = true;
                }
            }
        }
        if (!foundEnemy) {                                                  //checks if the choosen enemy was found
            return randomEnemy;                                             //makes the choosen enemy shoot
        }
        for (int y = correctRow; y < row; y++) {                            //searches for enemies below the choosen enemy
            if (enemies[y, correctColumn] != null) {

                correctRow = y;
            }
        }

        return enemies[correctRow, correctColumn];                           //makes the lowest enemy in that column shoot
    }

    public void EndGame() {

        endGame = true;
        endText.text = "LOSER";
    }

    private void MoveEnemy() {

        if (currentRow == (row - 1) && currentColumn == (column - 1) && stepCounter >= 20) {    //checks if the all enemies moved by the same amount and took all steps

            if (gameState != Enemystate.moveDown) {                                             //checks if current state is moveDown
                gameState = Enemystate.moveDown;                                                //sets current state to moveDown
            }
            else if (previousGamestate == Enemystate.moveLeft) {                                //checks if previous state is moveLeft
                gameState = Enemystate.moveRight;                                               //sets current state to moveRight
                stepCounter = 0;
            }
            else {
                gameState = Enemystate.moveLeft;                                                //sets current state to moveLeft
                stepCounter = 0;
            }
        }
        else {

            if (gameState != Enemystate.moveDown) {                                             //checks if current state is not moveDown
                previousGamestate = gameState;                                                  //sets previous state to current state
            }
        }

        switch (gameState) {                                                                    //checks what the current state is

            case Enemystate.moveLeft:                                                           //current state = moveLeft
                if (enemies[currentRow, currentColumn] != null) {

                    enemies[currentRow, currentColumn].MoveLeft();                              //makes enemy moves to left
                }
                break;
            case Enemystate.moveRight:                                                          //current state = moveRight
                if (enemies[currentRow, currentColumn] != null) {

                    enemies[currentRow, currentColumn].MoveRight();                             //makes enemy moves to right
                }
                break;
            case Enemystate.moveDown:                                                           //current state = moveDown
                if (enemies[currentRow, currentColumn] != null) {

                    enemies[currentRow, currentColumn].MoveDown();                              //makes enemy moves down 
                }
                break;
            default:
                break;
        }

        currentColumn++;
        if (currentColumn >= column) {              //checks if the last enemy of a row has moved

            currentColumn = 0;                      //moves enemy pointer to the beginning of a row
            currentRow++;                           //and to the next row
        }
        if (currentRow >= row) {                    //checks if the last enemy has moved

            currentRow = 0;                         //resets enemy pointer
            stepCounter++;                          //increase steps by one        
        }
        clockwork = 0;                              //resets clockwork counter
    }
}
