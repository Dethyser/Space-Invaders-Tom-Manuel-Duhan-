using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    float input;
    public float movementSpeed = 5f;        //the speed at which the ship moves

    public string axisName;

    private Rigidbody2D rb;
    public GameObject shotPrefab;

    private float shootCooldown;            //the smallest possible amount of time between 2 different shots

    [SerializeField] private Text lifeText; //reference to the text-object for the life-count

    private int lifes = 5;                  //amount of hits the ship can take before destroying

    private bool gameStart = false;         //check if the game has started
    private bool endGame = false;           //check if the game has ended
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {

        lifeText.text = "" + lifes;         //sets the life-text to 0
    }

    void Update() {
        if(shootCooldown > 0.0f) {                                      //checks if the cooldown is still above 0

            shootCooldown -= Time.deltaTime;                            //substracts the time since last frame from cooldown
        }
        if (!gameStart && Input.GetButtonDown("Enter")) {               //checks if the game didnt started yet and wether "enter" has been pressed

            gameStart = true;                                           //starts the game
        }
        if (gameStart && !endGame) {                                    //checks if the game has started and if it didnt ended yet

            input = Input.GetAxisRaw(axisName);                         //gives input a value of [1-, 1] depending on pressing left(-1),nothing(0) or right(1) 
            if (Input.GetButton("Jump") && shootCooldown <= 0.0f) {     //checks if "jump"(space) has been pressed and wether the cooldown is smaller or equal to 0
                shootCooldown = 0.75f;                                  //sets the cooldown to 0.75
                SpawnShot();
            }
        }
    }

    private void FixedUpdate() {

        rb.velocity = Vector2.right * input * movementSpeed;            //moves on the x-axis depending on input
    }

    private void OnCollisionEnter2D(Collision2D collision) {                //checks for collision
        if (collision.gameObject.CompareTag("Block")) {                     //checks if the collided object has the tag "Block" and if so stops the funktion
            return;
        }

        lifes--;                                                            //decreases lifes by 1
        lifeText.text = "" + lifes;                                         //refreshes the text-field for lifes
        if(lifes <= 0) {                                                    //checks of lifes are lower or equal to 0
            GameObject.Find("Spawner").GetComponent<Spawner>().EndGame();   //sends endgame-message to spawner
            Destroy(gameObject);                                            //destroys itself
        }
    }

    private void SpawnShot() {

        Instantiate(shotPrefab, transform.position + Vector3.up, Quaternion.identity);  //spawns a shot slightly above it
    }

    public void EndGame() {

        endGame = true;
    }
}
