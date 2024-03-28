using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private int UnitsForMassGainer;
    [SerializeField] private int UnitsForMassBurner;
   
    private static FoodManager instance;
    public static FoodManager Instance { get { return instance; } }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    public void FoodOutput(FoodItems foodType, Snake playerSnake)
    {
        switch (foodType)
        {
            case FoodItems.MassBurner:
                BurnMass(playerSnake);
                break;

            case FoodItems.MassGainer:
                GainMass(playerSnake);
                break;
        }
    }

    private void BurnMass(Snake snake)
    {
        snake.Shrink(UnitsForMassBurner); 
    }

    private void GainMass(Snake snake) 
    {
        snake.Grow(UnitsForMassGainer);
    }
}

public enum FoodItems
{
    MassGainer,
    MassBurner
}
