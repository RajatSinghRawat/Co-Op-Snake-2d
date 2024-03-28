using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> PowerUps;
    [SerializeField] private List <GameObject> FoodItems;
    [SerializeField] private Collider2D gridArea;
    [SerializeField] private Snake[] snakes;
    [SerializeField] private float minimumSpawningTimeForPowerUps, maximumSpawningTimeForPowerUps;
    [SerializeField] private float minimumSpawningTimeForFoodItems, maximumSpawningTimeForFoodItems;
    [SerializeField] private float spacingBetweenSpawnedItems;
    [SerializeField] private List<Vector2Int> spawnedPositions;
    [SerializeField] private float waitTimeForFoodItemSelfDestroy;   
    private Vector2Int randomPosition;
    private float delayTime;
    [SerializeField] private List<List<GameObject>> categories;
    private GameObject plannedItem;
    private Coroutine runningSpawnedFoodCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        categories = new List<List<GameObject>>
        {
            PowerUps,
            FoodItems
        };

        for (int i = 0; i < categories.Count; i++)
        {
            StartSpawningTime(categories[i]);
        }
    }

    public void startSpawningTimeForFoodItems(Transform spawnedFoodItem)
    {

        if (runningSpawnedFoodCoroutine != null)
        {
            StopCoroutine(runningSpawnedFoodCoroutine);
            runningSpawnedFoodCoroutine = null;
        }

        Vector2Int positionToRemove = Vector2Int.RoundToInt(spawnedFoodItem.position);       
        if (spawnedPositions.Contains(positionToRemove))
        {
            spawnedPositions.Remove(positionToRemove);
            Destroy(spawnedFoodItem.gameObject);
        }
        StartSpawningTime(FoodItems);
    }

    public void startSpawningTimeForPowerUpsItems(Transform spawnedPowerUpItem)
    {
        Vector2Int positionToRemove = Vector2Int.RoundToInt(spawnedPowerUpItem.position);
        if (spawnedPositions.Contains(positionToRemove))
        {
            spawnedPositions.Remove(positionToRemove);
        }
        StartSpawningTime(PowerUps);
    }

    private void StartSpawningTime(List<GameObject> spawnCategoryList)
    {
        if(spawnCategoryList == PowerUps)
        {
            delayTime = Random.Range(minimumSpawningTimeForPowerUps, maximumSpawningTimeForPowerUps);
            StartCoroutine(PickRandomItemAfterDelay(delayTime, spawnCategoryList));
        }
        else if(spawnCategoryList == FoodItems)
        {
            delayTime = Random.Range(minimumSpawningTimeForFoodItems, maximumSpawningTimeForFoodItems);
            if(CheckIfLengthIsInitial())
            {
                plannedItem = FoodItems.Find(obj => obj.GetComponent<Food>().foodType == global::FoodItems.MassGainer);
                StartCoroutine(SpawnPlannedItemAfterDelay(delayTime, plannedItem));
            }
            else
            {
                StartCoroutine(PickRandomItemAfterDelay(delayTime, spawnCategoryList));
            }
        }               
    }

    private bool CheckIfLengthIsInitial()
    {
        for(int i = 0; i < snakes.Length; i++)
        {
            if (snakes[i].getSnakeLength() <= snakes[i].getInitialLength())
            {
                return true;
            }
        }
        return false;
    }

    private void PickRandomItemAndPlace(List<GameObject> category)
    {
        int pickedItem = Random.Range(0, category.Count);
        PlaceItem(category[pickedItem]);
    }

    private void PlacePlannedItem(GameObject plannedItem)
    {
        PlaceItem(plannedItem);
    }

    private void PlaceItem(GameObject itemToPlace)
    {
        do
        {
            randomPosition = RandomizePosition();
        } while (CheckIfOverlappingOtherItem(randomPosition.x, randomPosition.y));

        spawnedPositions.Add(randomPosition);
        Vector2 Position = randomPosition;
        GameObject spawningItem = Instantiate(itemToPlace);
        spawningItem.transform.position = Position;

        Food foodItem = spawningItem.GetComponent<Food>();
        if (foodItem != null)
        {
            runningSpawnedFoodCoroutine = StartCoroutine(DestroyFoodAfterAssignedTime(foodItem));
        }
    }

    private IEnumerator DestroyFoodAfterAssignedTime(Food foodItem)
    {
        yield return new WaitForSeconds(waitTimeForFoodItemSelfDestroy);
        Destroy(foodItem.gameObject);
        startSpawningTimeForFoodItems(foodItem.transform);        
    }

    private bool CheckIfOverlappingOtherItem(int x, int y)
    {          
        foreach (Vector2Int spawnedPosition in spawnedPositions)
        {
            if (spawnedPosition.x == x && spawnedPosition.y == y)
            {
               return true;
            }
        }            
        return false;
    }

    private IEnumerator PickRandomItemAfterDelay(float delayTime, List<GameObject> spawnItemCategory)
    {
        yield return new WaitForSeconds(delayTime);
        PickRandomItemAndPlace(spawnItemCategory);
    }

    private IEnumerator SpawnPlannedItemAfterDelay(float delayTime, GameObject spawnItem)
    {
        yield return new WaitForSeconds(delayTime);
        PlacePlannedItem(spawnItem);
    }

    public Vector2Int RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        // Prevent the food from spawning on the snake
        while (AnySnakeOccupies(x, y))
        {
            x++;

            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;

                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }
        return new Vector2Int(x, y);
    }

    private bool AnySnakeOccupies(int x, int y)
    {
        for(int i = 0; i < snakes.Length; i++ )
        {
            if (snakes[i].Occupies(x, y))
            {
                return true;
            }
        }
        return false;
    }
}
