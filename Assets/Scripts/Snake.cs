using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction = Vector2Int.right;
    public float speed = 20f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public List<Transform> segments = new List<Transform>();
    private Vector2Int input;
    private float nextUpdate;
    public bool isShieldActive;
    public int scoreMultiplier;
    private float initialSpeedMultiplier;
    private int initialScoreMultiplier;
    public KeyCode Left,Right,Up,Down;
    private Vector2 initialPosition;
    public Transform grid;
    public Vector2 changedPosition;
    private int gridRightSideBoundary, gridLeftSideBoundary, gridTopSideBoundary, gridBottomSideBoundary;
    private Vector3 lastPosition;
    private Vector2 currentPosition;
    [SerializeField] private Snake opponent;
    [SerializeField] private Spawner itemsSpawner;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int score;
    [SerializeField] private int points;
    [SerializeField] private ScoreManager scoreManagerController;
    [SerializeField] private TextMeshProUGUI SpeedUpPowerUpText;
    [SerializeField] private TextMeshProUGUI ScoreMultiplierPowerUpText;
    [SerializeField] private TextMeshProUGUI ShieldPowerUpText;
    [SerializeField] private string PlayerBiteMessage;
    [SerializeField] private string PlayerItselfBiteMessage;
    [SerializeField] private string HeadCrashedMessage;
    [SerializeField] private PlayerSegment SegmentPlayerNeed;

    //getters
    public int getScore()
    {
        return score;
    }

    public int getInitialScoreMultiplier()
    {
        return initialScoreMultiplier;
    }

    public float getInitialSpeedMultiplier()
    {
        return initialSpeedMultiplier;
    }

    public float getSnakeLength()
    {
        return segments.Count;
    }

    public float getInitialLength()
    {
        return initialSize;
    }


    //setters
    public void setIsShieldActive(bool activeValue)
    {
        isShieldActive = activeValue;
    }

    public void setScoreMultiplier(int multiplier)
    {
        scoreMultiplier = multiplier;
    }

    public void setSpeedMultiplier( float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void SetSpeedUpPowerUpTextVisible(bool isActive)
    {
        SpeedUpPowerUpText.gameObject.SetActive(isActive);
    }

    public void SetScoreMultiplierPowerUpTextVisible(bool isActive)
    {
        ScoreMultiplierPowerUpText.gameObject.SetActive(isActive);
    }

    public void SetShieldPowerUpTextVisible(bool isActive)
    {
        ShieldPowerUpText.gameObject.SetActive(isActive);
    }


    private void Start()
    {
        score = 0;
        CalculateBoundary();
        initialPosition = transform.position;
        ResetState();
        initialScoreMultiplier = scoreMultiplier;
        initialSpeedMultiplier = speedMultiplier;  
        currentPosition  = transform.position;
    }

    private void Update()
    {
        InputHandler();
    }

    private void FixedUpdate()
    {
        // Wait until the next update before proceeding
        if (Time.time < nextUpdate)
        {
            return;
        }

        // Set the new direction based on the input
        if (input != Vector2Int.zero)
        {
            direction = input;
        }

        SegmentMovement();

        // Move the snake in the direction it is facing
        // Round the values to ensure it aligns to the grid
        currentPosition.x = Mathf.RoundToInt(transform.position.x) + direction.x;
        currentPosition.y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = currentPosition;

        // Set the next update time based on the speed
        nextUpdate = Time.time + (2f / (speed * speedMultiplier));
        Traverse();
    }

    private void SegmentMovement()
    {
        lastPosition = segments[segments.Count - 1].position;
        // Set each segment's position to be the same as the one it follows. We
        // must do this in reverse order so the position is set to the previous
        // position, otherwise they will all be stacked on top of each other.
        for (int i = segments.Count - 1; i > 0; i--)
        {
            segments[i].position = segments[i - 1].position;
        }
    } 

    public void Grow(int numberOfSegments)
    {
        for(int i = 0; i < numberOfSegments; i++)
        {            
            Transform segment = Instantiate(segmentPrefab, lastPosition, transform.rotation);
            segments.Add(segment);            
        }
    }

    public void Shrink(int numberOfSegmentsToDestroy)
    {
        if((segments.Count - numberOfSegmentsToDestroy) < initialSize)
        {
           numberOfSegmentsToDestroy = segments.Count - initialSize;
        }
        DestroySegments(numberOfSegmentsToDestroy);
    }

    private void DestroySegments(int units)
    {
        for (int i = 0; i < units; i++)
        {
            GameObject segmentToBeDestroyed = segments[segments.Count - 1].gameObject;
            segments.Remove(segments[segments.Count - 1]);
            Destroy(segmentToBeDestroyed);
        }
        score -= points;
        scoreText.text = score.ToString();
    }

    private void CalculateBoundary()
    {
        gridLeftSideBoundary = -(int)grid.localScale.x / 2;
        gridTopSideBoundary = (int)grid.localScale.y / 2;
        gridRightSideBoundary = (int)grid.localScale.x / 2;
        gridBottomSideBoundary = -(int)grid.localScale.y / 2;
    }

    private void ResetState()
    {
        transform.position = initialPosition;            
        Time.timeScale = 1f;
        input = Vector2Int.zero;
        direction = Vector2Int.right;
        lastPosition = transform.position + (new Vector3(-direction.x, -direction.y));

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        Grow(initialSize - 1);            
    }

    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x &&
                Mathf.RoundToInt(segment.position.y) == y) {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Snake snakeController = other.gameObject.GetComponent<Snake>();       
        if (snakeController != null)
        {
             if(snakeController.direction.x != 0 && direction.x != 0 )
             {
                  if(snakeController.transform.position.y == transform.position.y)
                  {                    
                        KillOpponentWithMessage(HeadCrashedMessage);
                  }
             }
             else if (snakeController.direction.y != 0 && direction.y != 0)
             {
                  if (snakeController.transform.position.x == transform.position.x)
                  {                        
                        KillOpponentWithMessage(HeadCrashedMessage);
                  }
             }
        }   
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Snake snakeController = collision.gameObject.GetComponent<Snake>();

        if (snakeController != null)
        {
            if (snakeController.direction.x != 0 && direction.y != 0)
            {
                if (snakeController.transform.position.y == transform.position.y)
                {
                    KillOpponentWithMessage(HeadCrashedMessage);
                }
            }
            else if (snakeController.direction.y != 0 && direction.x != 0)
            {
                if (snakeController.transform.position.x == transform.position.x)
                {
                    KillOpponentWithMessage(HeadCrashedMessage);
                }
            }
        }
    }

    public void Traverse()
    {
      if (transform.position.x > gridRightSideBoundary)
      {
           // Wrap to the left side
           changedPosition.x = gridLeftSideBoundary;
           changedPosition.y = transform.position.y;
           transform.position = changedPosition;
      }
      else if (transform.position.x < gridLeftSideBoundary)
      {
           // Wrap to the right side
           changedPosition.x = gridRightSideBoundary;
           changedPosition.y = transform.position.y;
           transform.position = changedPosition;
      }
      else if (transform.position.y > gridTopSideBoundary)
      {
           // Wrap to the bottom side
           changedPosition.x = transform.position.x;
           changedPosition.y = gridBottomSideBoundary;
           transform.position = changedPosition;
      }
      else if (transform.position.y < gridBottomSideBoundary)
      {
           // Wrap to the top side
           changedPosition.x = transform.position.x;
           changedPosition.y = gridTopSideBoundary;
           transform.position = changedPosition;
      }        
    }

    private void InputHandler()
    {
        // Only allow turning up or down while moving in the x-axis
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(Up))
            {
                input = Vector2Int.up;
            }
            else if (Input.GetKeyDown(Down))
            {
                input = Vector2Int.down;
            }
        }
        // Only allow turning left or right while moving in the y-axis
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(Right))
            {
                input = Vector2Int.right;
            }
            else if (Input.GetKeyDown(Left))
            {
                input = Vector2Int.left;
            }
        }
    }

    private void SetPlayerInactive()
    {        
        Time.timeScale = 0f;
        foreach (Transform playerSegment in segments)
        {
           playerSegment.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);        
    }

    public void KillPlayer(string Message)
    {
        if(!isShieldActive)
        {
            SetPlayerInactive();
            callToCompareScores(Message);
        }
    }

    public void KillOpponentWithMessage(string message)
    {
        opponent.KillPlayer(message);       
    }

    public void Kill(PlayerSegment segmentBelongTo)
    {
        if(segmentBelongTo == SegmentPlayerNeed)
        {
            if (!isShieldActive)
            {
                SetPlayerInactive();
                scoreManagerController.OnlyDisplayMessage(PlayerItselfBiteMessage, this);
            }
        }
        else
        {
            KillOpponentWithMessage(PlayerBiteMessage);
        }
    }

    public void PowerUp(PowerUps powerUpType, Transform powerUpItem)
    {
        PowerUpsManager.Instance.ActivatePowerUp(powerUpType, this);
        itemsSpawner.startSpawningTimeForPowerUpsItems(powerUpItem);
    }

    public void EatFood(FoodItems foodType, Transform foodItem)
    {
        FoodManager.Instance.FoodOutput(foodType, this);
        itemsSpawner.startSpawningTimeForFoodItems(foodItem);
        score += points * scoreMultiplier;
        string scoreInString =  Convert.ToString(score);
        scoreText.text = scoreInString;
    }

    private void callToCompareScores(string BiteMessage)
    {
        scoreManagerController.CompareScores(BiteMessage);
    }
}
