using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour {

    public Canvas pauseMenu;
    public Button resumeBtn;
    public Button restartBtn;
    public Button exitBtn;

	// Use this for initialization
	void Start () {
        pauseMenu = pauseMenu.GetComponent<Canvas>();
        resumeBtn = resumeBtn.GetComponent<Button>();
        restartBtn = restartBtn.GetComponent<Button>();
        exitBtn = exitBtn.GetComponent<Button>();

        //pauseMenu.gameObject.SetActive(false);
	}
	
	public void Resume() {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
    }

    public void Restart() {
        Time.timeScale = 1;
        Application.LoadLevel("Loading");
    }

    public void Exit() {
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    }
}
