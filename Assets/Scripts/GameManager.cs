using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		GS_PAUSEMENU,
		GS_GAME,
		GS_LEVELCOMPLETED,
		GS_OPTIONS
	}
	
	public GameState currentGameState;
	public static GameManager instance;
	
	public Canvas inGameCanvas;
	public Canvas pauseMenuCanvas;
	public Canvas levelCompletedCanvas;
	public Canvas optionsCanvas;
	
	public Text gemsText;
	private int gems = 0;
	public Text enemyText;
	private int enemies = 0;
	public Text qualityText;
	
	// increases the number of collected gems
	public void addGems(int gemNumber)
    {
		gems += gemNumber;
		gemsText.text = gems.ToString();
    }
	
	// increases the number of defeated enemies
	public void addEnemies(int enemyNumber)
    {
		enemies += enemyNumber;
		enemyText.text = enemies.ToString();
    }
	
	void Awake()
	{
		instance = this;
		gemsText.text = gems.ToString();
		enemyText.text = enemies.ToString();
		qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
	}
	
	// choose the canvas of the current game state: in game, paused, finished or options
	void SetGameState (GameState newGameState)
	{
		currentGameState = newGameState;
		inGameCanvas.enabled = (currentGameState == GameState.GS_GAME);
		pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
		levelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED);
		optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);
	}
	
	// in game state
	public void InGame()
	{
		SetGameState(GameState.GS_GAME);
	}

	// pause state
	public void PauseMenu()
	{
		SetGameState(GameState.GS_PAUSEMENU);
	}
	
	// level finished state
	public void LevelCompleted()
	{
		SetGameState(GameState.GS_LEVELCOMPLETED);
	}
	
	// options state
	public void Options()
	{
		SetGameState(GameState.GS_OPTIONS);
	}
	
	// click on resume button in pause menu
	public void OnResumeButtonClicked()
    {
		InGame();
    }

	// click on restart button in pause menu
	public void OnRestartButtonClicked()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	// click on exit button in pause menu - changes to main menu scene
	public void OnExitButtonClicked()
    {
		SceneManager.LoadScene("MainMenu");
    }
	
	// click on options button in pause menu
	public void OnOptionsButtonClicked()
    {
		Options();
    }
	
	// increase quality level in options
	public void OnPlusButtonClicked()
    {
		QualitySettings.IncreaseLevel();
		qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
	}

	// decrease quality level in options
	public void OnMinusButtonClicked()
    {
		QualitySettings.DecreaseLevel();
		qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()].ToString();
	}
	
	// change volume level in options
	public void setVolume(float vol)
    {
		AudioListener.volume = vol;
    }
	
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_PAUSEMENU)
			InGame();
		else if (Input.GetKeyDown(KeyCode.Escape) && currentGameState == GameState.GS_GAME)
			PauseMenu();
    }
}
