using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button PauseButton;
    [SerializeField] private PauseGameController pauseGameObject;

    private void Awake()
    {
        PauseButton.onClick.AddListener(PauseGame);
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
          PauseGame();
       }
    }

    private void PauseGame()
    {
        pauseGameObject.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
