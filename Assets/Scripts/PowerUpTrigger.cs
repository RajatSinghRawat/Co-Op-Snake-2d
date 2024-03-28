using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    public PowerUps powerUpType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Snake snakeController = other.gameObject.GetComponent<Snake>();
        if (snakeController != null)
        {
            snakeController.PowerUp(powerUpType, gameObject.transform);
            Destroy(gameObject);
        }
    }
}
