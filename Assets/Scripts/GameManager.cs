using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private Scene scene;
	private int level;
	private int score;
	public GameObject spaceShip;
	public GameObject asteroidLarge;
	public GameObject asteroidMedium;
	public GameObject ufo;
	public Text levelText;
	public Text scoreText;
	public Text livesText;
	public Text damageText;
	public GameObject gameOverPanel;
	public GameObject newHighScoreText;
	public GameObject pausePanel;
	private GameObject spawnedUfo;

    void Start()
    {
		Application.targetFrameRate = 60;
		scene = SceneManager.GetActiveScene();
		Cursor.visible = false;
		level = 1;
		UpdateLevel();
		Instantiate(spaceShip, transform.position, Quaternion.identity);
		Instantiate(asteroidLarge, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity);
		Invoke("InstantiateUfo", 5.0f);
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
    }

	void InstantiateUfo()
	{
		spawnedUfo = Instantiate(ufo, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity);
		InvokeRepeating("CheckUfoStatus", 0.0f, 2.0f);
	}

	void CheckUfoStatus()
	{
		if (!spawnedUfo.activeSelf)
			CheckAsteroidCount();
	}
	
	void CheckAsteroidCount()
	{
		List<GameObject> allObjects = new List<GameObject>();
		scene.GetRootGameObjects(allObjects);
		int asteroidCount = 0;
		foreach (GameObject go in allObjects)
		{
			if (go.CompareTag("Asteroid"))
				asteroidCount++;
		}
		if (asteroidCount == 0)
		{
			Invoke("ChangeLevel", 2.0f);
		}
	}

	void ChangeLevel()
	{
		level++;
		UpdateLevel();
		if (level % 2 == 0)
		{
			for(int i = 0; i < level; i++)
			{
				Instantiate(asteroidMedium, new Vector2(Random.Range(-5.75f, 5.75f), 9.9f), Quaternion.identity);
				Instantiate(asteroidMedium,new Vector2(Random.Range(-5.75f, 5.75f), 9.9f), Quaternion.identity);
			}
		}
		else
		{
			for(int i = 0; i < level; i++)
			{
				Instantiate(asteroidLarge, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity);
			}
		}
		Invoke("ActivateUfo", 10.0f-(level*0.5f));	
	}

	void ActivateUfo()
	{
		spawnedUfo.SetActive(true);
		spawnedUfo.GetComponent<UfoControl>().canShoot = true;
		spawnedUfo.GetComponent<UfoControl>().speed *= 1.10f;
		spawnedUfo.GetComponent<UfoControl>().bulletSpeed += 10;
		spawnedUfo.GetComponent<UfoControl>().hitPoints++;
		spawnedUfo.GetComponent<UfoControl>().sfxUfoEngine.Play();
	}

	void UpdateLevel() { levelText.text = "LEVEL " + level; }

	void UpdateScore(int newScore)
	{
		score += newScore;
		scoreText.text = "SCORE " + score; 
	}
	void UpdateLives(int lives) { livesText.text = "LIVES " + lives; }

	void UpdateDamage(int damage)
	{
		damage = (damage > 100) ? 100 : damage;
		damageText.text = "DAMAGE " + damage + "%";
		if (damage >= 80)
			damageText.color = Color.red;
		else
			damageText.color = Color.white;
	}

	void GameOver()
	{
		Cursor.visible = true;
		gameOverPanel.SetActive(true);
		if (HighScore())
			newHighScoreText.SetActive(true);
		else
			newHighScoreText.SetActive(false);
		Debug.Log("score_" + score + " \thighScore_" + PlayerPrefs.GetInt("HighScore"));	
	}

	void PauseGame()
	{
		Cursor.visible = true;
		pausePanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		pausePanel.SetActive(false);
		Time.timeScale = 1;
		Cursor.visible = false;
	}

	bool HighScore()
	{
		if (score > PlayerPrefs.GetInt("HighScore"))
		{
			PlayerPrefs.SetInt("HighScore", score);
			return true;
		}
		else
			return false;
	}

	public void ReturnToMenu()
	{
		if (Time.timeScale == 0)
			Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}
}
