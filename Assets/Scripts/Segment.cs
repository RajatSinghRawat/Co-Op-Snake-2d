using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] PlayerSegment segmentBelongTo;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Snake snakeController = collision.gameObject.GetComponent<Snake>();
        if (snakeController != null)
        {
            Debug.Log("Snake Collided");
            snakeController.Kill(segmentBelongTo);
        }
    }
}

public enum PlayerSegment
{
    Player1Segment,
    Player2Segment
}