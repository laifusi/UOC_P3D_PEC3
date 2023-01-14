using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MusicOptions()
    {
        MusicManager.Instance.SwitchAudio(true, false);

    }

    public void SFXOptions()
    {
        MusicManager.Instance.SwitchAudio(false, true);
    }
}
