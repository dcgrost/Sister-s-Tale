using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public Slider slider;
    public float sliderValue;
    public AudioSource buttonSound;
    public AudioSource returnSound;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("audioVolume", 0.5f);
        AudioListener.volume = slider.value;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        buttonSound.Play();
    }
    public void Options()
    {
        optionsPanel.SetActive(true);
        buttonSound.Play();
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Back()
    {
        optionsPanel.SetActive(false);
        returnSound.Play();
    }
    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("audioVolume", sliderValue);
        AudioListener.volume = slider.value;
    }
}
