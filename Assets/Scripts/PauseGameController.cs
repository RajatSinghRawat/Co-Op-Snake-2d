using UnityEngine;
using UnityEngine.UI;

public class PauseGameController : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button MainMenuButton;

    private void Awake()
    {
        ResumeButton.onClick.AddListener(ResumeGame);
        RestartButton.onClick.AddListener(RestartLevel);
        MainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    
    private void RestartLevel()
    {
        LevelLoader.Instance.ReloadLevel();
    }

    private void LoadMainMenu()
    {
        LevelLoader.Instance.LoadMainMenu();
    }
}
