
using UnityEngine;

public class Block : MonoBehaviour
{
    public int lifes = 5;                                       //hits the block can take before destroying itself

    private void OnCollisionEnter2D(Collision2D collision) {    //checks for collision

        lifes--;                                                //reduces lifes by 1
        if(lifes <= 0) {                                        //checks if lifes are lower or equal to 0

            Destroy(gameObject);                                //destroys itself
        }
    }
}
