using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

    public Canvas mainMenu;
    public Button startText;
    public Button exitText;

	// Use this for initialization
	void Start () {
        mainMenu = mainMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
	}
	
    public void StartGame() {
        Application.LoadLevel("Loading");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
