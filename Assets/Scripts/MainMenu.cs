using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private IEnumerator StartGame(string levelName)
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadScene(levelName);
    }
	
    // choose level 1
	public void onLevel1ButtonPressed()
    {
        StartCoroutine(StartGame("Level1"));
    }
	
    // quit app
	public void onExitGameButtonClicked()
	{
		#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
