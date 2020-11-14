using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        None,
        Main,
        Settings,
        Level,
    }

    public CanvasGroup mainScreen;
    public CanvasGroup settingsScreen;
    public CanvasGroup levelScreen;
    public AudioMixer audioMixer;
    public Slider music;
    public Slider sound;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(mainScreen, screen == Screen.Main);
        Utility.SetCanvasGroupEnabled(settingsScreen, screen == Screen.Settings);
        Utility.SetCanvasGroupEnabled(levelScreen, screen == Screen.Level);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentScreen(Screen.Main);
        _ = audioMixer.GetFloat("musicVolume", out float value1);
        _ = audioMixer.GetFloat("soundVolume", out float value2);
        music.value = (value1 + 80) / 100.0f;
        sound.value = (value2 + 80) / 100.0f;
        AudioManager.instance.PlaySound("easy");
    }

    public void StartNewGame()
    {
        SetCurrentScreen(Screen.Level);
    }

    public void StartLevel(string level)
    {
        SetCurrentScreen(Screen.None);
        LoadingScreen.instance.LoadScene(level);
    }

    public void OpenSettings()
    {
        SetCurrentScreen(Screen.Settings);
    }

    public void CloseSettings()
    {
        SetCurrentScreen(Screen.Main);
    }

    public void MusicChanged(float value)
    {
        _ = audioMixer.SetFloat("musicVolume", (value * 100) - 80);
    }

    public void SoundChanged(float value)
    {
        _ = audioMixer.SetFloat("soundVolume", (value * 100) - 80);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
