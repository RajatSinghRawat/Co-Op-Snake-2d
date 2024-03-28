using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Snake Player1;
    [SerializeField] private Snake Player2;
    [SerializeField] private string Player1WinStatement;
    [SerializeField] private string Player2WinStatement;
    [SerializeField] private string NoPlayerWinStatement;
    [SerializeField] private TextMeshProUGUI winningMessage;
    [SerializeField] private TextMeshProUGUI gameStatus;
    [SerializeField] private GameOverController gameOverControllerObject;
    public void CompareScores(string Message)
    {     
       if(Player1.getScore() > Player2.getScore())
       {
            gameStatus.text = Message;
            winningMessage.text = Player1WinStatement;
       }
       else if (Player1.getScore() < Player2.getScore())
       {
            gameStatus.text = Message;
            winningMessage.text = Player2WinStatement;
       }
       else if(Player1.getScore() == Player2.getScore())
       {
         if (Player1.gameObject.activeInHierarchy)
         {
             gameStatus.text = Message;
             winningMessage.text = Player1WinStatement;
         }
         else if(Player2.gameObject.activeInHierarchy)
         {
             gameStatus.text = Message;
             winningMessage.text = Player2WinStatement;
         }
         else
         {
             gameStatus.text = Message;
             winningMessage.text = NoPlayerWinStatement;
         }
       }

       gameOverControllerObject.gameObject.SetActive(true);
    }

    public void OnlyDisplayMessage(string Message, Snake SnakePlayer)
    {
        gameStatus.text = Message;
        if(SnakePlayer == Player1)
        {
            winningMessage.text = Player2WinStatement;
        }
        else if(SnakePlayer == Player2)
        {
            winningMessage.text = Player1WinStatement;
        }

        gameOverControllerObject.gameObject.SetActive(true);
    }
}
