using System.Collections;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] int powerUpsCoolDownTime;
    [SerializeField] int powerUpScoreMultilpier;
    [SerializeField] int powerUpSpeedMultiplier;

    private Coroutine runningShieldPowerUpCoroutine;
    private Coroutine runningScoreBoostPowerUpCoroutine;
    private Coroutine runningSpeedUpPowerUpCoroutine;

    private static PowerUpsManager instance;
    public static PowerUpsManager Instance { get { return instance; } }


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

    public void ActivateShield(Snake snake)
    {            
        if (runningShieldPowerUpCoroutine != null)
        {
             StopCoroutine(runningShieldPowerUpCoroutine);
        }              
        snake.setIsShieldActive(true);
        snake.SetShieldPowerUpTextVisible(true);
        runningShieldPowerUpCoroutine = StartCoroutine(DelayCoolDownShieldPowerUp(snake));        
    }

    public void BoostScore(Snake snake)
    {                  
        if (runningScoreBoostPowerUpCoroutine != null)
        {
            StopCoroutine(runningScoreBoostPowerUpCoroutine);
        }

        snake.setScoreMultiplier(powerUpScoreMultilpier);
        snake.SetScoreMultiplierPowerUpTextVisible(true);
        runningScoreBoostPowerUpCoroutine = StartCoroutine(DelayCoolDownScoreBoostPowerUp(snake));        
    }


    public void BoostSpeed(Snake snake)
    {
        if (runningSpeedUpPowerUpCoroutine != null)
        {
            StopCoroutine(runningSpeedUpPowerUpCoroutine);
        }
 
        snake.setSpeedMultiplier(powerUpSpeedMultiplier);
        snake.SetSpeedUpPowerUpTextVisible(true);
        runningSpeedUpPowerUpCoroutine = StartCoroutine(DelayCoolDownSpeedMultiplierPowerUp(snake));       
    }

    public void ActivatePowerUp(PowerUps powerUpName, Snake snake)
    {
        switch (powerUpName)
        {
            case PowerUps.Shield:
                ActivateShield(snake);
                break;

            case PowerUps.ScoreBoost:
                BoostScore(snake);
                break;

            case PowerUps.SpeedUp:
                BoostSpeed(snake);
                break;
        }
    }

    private void PowerCoolDown(PowerUps powerUpName, Snake snake)
    {
        switch (powerUpName)
        {
            case PowerUps.Shield:
                snake.setIsShieldActive(false);
                break;

            case PowerUps.ScoreBoost:
                snake.setScoreMultiplier(snake.getInitialScoreMultiplier());
                break;

            case PowerUps.SpeedUp:
                snake.setSpeedMultiplier(snake.getInitialSpeedMultiplier());
                break;
        }
    }

    IEnumerator DelayCoolDownShieldPowerUp(Snake snake)
    {
        yield return new WaitForSeconds(powerUpsCoolDownTime);
        if(snake != null)
        {
            PowerCoolDown(PowerUps.Shield, snake);
            snake.SetShieldPowerUpTextVisible(false);
            runningShieldPowerUpCoroutine = null;
        }        
    }

    IEnumerator DelayCoolDownScoreBoostPowerUp(Snake snake)
    {
        yield return new WaitForSeconds(powerUpsCoolDownTime);
        if (snake != null)
        {
            PowerCoolDown(PowerUps.ScoreBoost, snake);
            snake.SetScoreMultiplierPowerUpTextVisible(false);
            runningScoreBoostPowerUpCoroutine = null;
        }
    }

    IEnumerator DelayCoolDownSpeedMultiplierPowerUp(Snake snake)
    {
        yield return new WaitForSeconds(powerUpsCoolDownTime);
        if (snake != null)
        {
            PowerCoolDown(PowerUps.SpeedUp, snake);
            snake.SetSpeedUpPowerUpTextVisible(false);
            runningSpeedUpPowerUpCoroutine = null;
        }
    }
}



public enum PowerUps
{
    Shield,
    ScoreBoost,
    SpeedUp
}

