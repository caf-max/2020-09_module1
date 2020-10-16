using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void ExitGame()
    {
        Application.Quit();
    }
}
