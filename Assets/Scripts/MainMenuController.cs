using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button PlayGameButton;
    [SerializeField] private Button QuitGameButton;
    
    private void Awake()
    {
        PlayGameButton.onClick.AddListener(PlayGame);
        QuitGameButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        LevelLoader.Instance.LoadGameScreen();
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
