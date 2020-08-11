using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Mainmenu;
    public GameObject Settingsmenu;

    public Slider EffectsVolumeSlider;
    public Slider MusicVolumeSlider;

    public TextMeshProUGUI EffectsVolumeText;
    public TextMeshProUGUI MusicVolumeText;

    void Start()
    {
        EffectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        Settingsmenu.SetActive(false);
    }

    private void Update()
    {
        EffectsVolumeText.text = "Effects : " + EffectsVolumeSlider.value.ToString() + "%";
        MusicVolumeText.text = "Music : " + MusicVolumeSlider.value.ToString() + "%";
    }
    public void ShowSettings()
    {
        Settingsmenu.SetActive(true);
        Mainmenu.SetActive(false);
    }

    public void HideSettings()
    {
        Settingsmenu.SetActive(false);
        Mainmenu.SetActive(true);

        Settings.EffectsVolume = Mathf.RoundToInt(EffectsVolumeSlider.value); // Despite slider having "Whole Value" setting on is still float type
        Settings.MusicVolume = Mathf.RoundToInt(MusicVolumeSlider.value);

        PlayerPrefs.SetFloat("EffectsVolume", EffectsVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Settings.Difficulty = 1;
    }
}
