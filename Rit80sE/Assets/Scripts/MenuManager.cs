using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject helpMenu;
    public GameObject playMenu;
    private bool helpIsOn;
    private bool playIsOn;

    private void Start()
    {
        helpIsOn = false;
        playIsOn = false;

    }

    public void ToggleHelp()
    {
        if (!helpIsOn)
        {
            helpMenu.SetActive(true);
            helpIsOn = true;
            playMenu.SetActive(false);
            playIsOn = false;

        }
        else
        {
            helpMenu.SetActive(false);
            helpIsOn = false;
        }
    }

    public void TogglePlay()
    {
        if (!playIsOn)
        {
            playMenu.SetActive(true);
            playIsOn = true;
            helpMenu.SetActive(false);
            helpIsOn = false;
        }
        else
        {
            playMenu.SetActive(false);
            playIsOn = false;
        }
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(3);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
