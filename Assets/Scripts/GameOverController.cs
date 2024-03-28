using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
   [SerializeField] private Button RestartButton;
   [SerializeField] private Button MainMenuButton;

    private void Awake()
    {
        RestartButton.onClick.AddListener(RestartLevel);
        MainMenuButton.onClick.AddListener(LoadMainMenu);
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
