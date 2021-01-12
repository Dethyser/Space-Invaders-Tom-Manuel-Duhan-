
using UnityEngine;

public class Block : MonoBehaviour
{
    public int lifes = 5;

    private void OnCollisionEnter2D(Collision2D collision) {
        lifes--;
        if(lifes <= 0) {

            Destroy(gameObject);
        }
    }
}
