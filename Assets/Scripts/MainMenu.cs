using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject highScorePanel;
    public GameObject highScoreValueText;
    public AudioClip menuSelect;
    public AudioClip startGame;
    public AudioSource audioSource;

    void Awake()
    {
        highScoreValueText.GetComponent<Text>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        Invoke("LoadGame", 1.0f);
        audioSource.PlayOneShot(startGame);
    }

    void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
        audioSource.PlayOneShot(menuSelect);
    }

    public void LeaveCredits()
    {
        creditsPanel.SetActive(false);
        audioSource.PlayOneShot(menuSelect);
    }

    public void ShowHighScore()
    {
        highScorePanel.SetActive(true);
        audioSource.PlayOneShot(menuSelect);
    }

    public void LeaveHighScore()
    {
        highScorePanel.SetActive(false);
        audioSource.PlayOneShot(menuSelect);
    }

    public void Exit()
	{
        audioSource.PlayOneShot(menuSelect);
		Application.Quit(0);
	}
}
