using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        optionsPanel.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Back()
    {
        optionsPanel.SetActive(false);
    }
}
