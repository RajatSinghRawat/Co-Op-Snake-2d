using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{   
    public FoodItems foodType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Snake snakeController = collision.gameObject.GetComponent<Snake>();
        if (snakeController != null) 
        {
            snakeController.EatFood(foodType, gameObject.transform);
        }
    }
}
