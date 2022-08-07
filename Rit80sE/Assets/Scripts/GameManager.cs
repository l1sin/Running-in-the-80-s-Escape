using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private string pitch = "Pitch";
    public Text timer;
    public Text pointCounter;
    public Text timeFinal;
    public Text pointsFinal;
    public Text rublesFinal;
    public Text finalRank;
    public GameObject levelCompleteDialogue;
    private PlayerController playerController;
    public float competition;

    private int seconds;
    private int minutes;
    public int totalSeconds;
    private string secondsString;
    private string minutesString;
    private float time;
    public float speedrunTime;
    public float rublesInLevel;
    public float rublesCollected;
    public GameObject[] rubles;
    public bool levelComplete;

    public GameObject deathDialogue;
    public Text timeDeath;
    public Text pointsDeath;
    public Text rublesDeath;
    public Text deathReasonText;
    public string deathReason;

    public bool dead;

    public GameObject pauseDialogue;
    public bool paused;
    private float timeScaleOnPause;

    public float points;
    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        levelComplete = false;
        rubles = GameObject.FindGameObjectsWithTag("Ruble");
        rublesInLevel = rubles.Length;
        audioMixer.SetFloat(pitch, 1);
        Time.timeScale = 1;
    }

    private void Update()
    {
        Timer();
        PointCounter();
        CalculateFinal();
        LevelComplete();
        PlayerDead();
        Pause();
    }

    private void Timer()
    {
        time += Time.deltaTime;

        seconds = Mathf.RoundToInt(time);
        if (seconds == 60)
        {
            time -= 60;
            seconds -= 60;
            minutes++;
        }
        secondsString = seconds.ToString();
        minutesString = minutes.ToString();
        totalSeconds = minutes * 60 + seconds;
        if (seconds >= 10)
        {
            timer.text = minutesString + ":" + secondsString;
        }
        else
        {
            timer.text = minutesString + ":0" + secondsString;
        }
    }

    private void PointCounter()
    {
        pointCounter.text = points.ToString();
    }

    private void CalculateFinal()
    {
        if (levelComplete)
        {
            competition = rublesCollected / rublesInLevel;
            if (seconds >= 10)
            {
                timeFinal.text = "Время: " + minutesString + ":" + secondsString;
            }
            else
            {
                timeFinal.text = "Время: " + minutesString + ":0" + secondsString;
            }
            pointsFinal.text = "Очки: " + points.ToString();
            rublesFinal.text = "Собрано рублей: " + rublesCollected + "/" + rublesInLevel;
            if (competition == 0)
            {
                finalRank.text = "Ранг: F";
            }
            else if (competition < 0.25f)
            {
                finalRank.text = "Ранг: D";
            }
            else if (competition < 0.50f)
            {
                finalRank.text = "Ранг: C";
            }
            else if (competition < 0.75f)
            {
                finalRank.text = "Ранг: B";
            }
            else if (competition == 1f && totalSeconds > speedrunTime)
            {
                finalRank.text = "Ранг: A";
            }
            else if (competition == 1f && totalSeconds < speedrunTime)
            {
                finalRank.text = "Ранг: S";
            }
        }      
    }

    private void PlayerDead()
    {
        if (dead)
        {
            Time.timeScale = 0;

            if (seconds >= 10)
            {
                timeDeath.text = "Время: " + minutesString + ":" + secondsString;
            }
            else
            {
                timeDeath.text = "Время: " + minutesString + ":0" + secondsString;
            }
            pointsDeath.text = "Очки: " + points.ToString();
            rublesDeath.text = "Рублей собрано: " + rublesCollected + "/" + rublesInLevel;
            deathReasonText.text = deathReason;
            deathDialogue.SetActive(true);
        }
    }

    private void LevelComplete()
    {
        if (levelComplete)
        {
            playerController.levelComplete = true;
            audioMixer.SetFloat(pitch, 1);
            Time.timeScale = 0;
            levelCompleteDialogue.SetActive(true);
        }
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            paused = true;
            playerController.paused = true;
            pauseDialogue.SetActive(true);
            timeScaleOnPause = Time.timeScale;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            paused = false;
            playerController.paused = false;
            pauseDialogue.SetActive(false);
            Time.timeScale = timeScaleOnPause;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Continue()
    {
        paused = false;
        playerController.paused = false;
        pauseDialogue.SetActive(false);
        Time.timeScale = timeScaleOnPause;
    }
}
