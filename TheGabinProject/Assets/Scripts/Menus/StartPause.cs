using UnityEngine;
using System.Collections;

public class StartPause : MonoBehaviour {

    public GameObject canvas;
    public Canvas initiatePauseMenu;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!initiatePauseMenu.enabled) {
            initiatePauseMenu.enabled = true;
        }
        
        if (Input.GetKeyDown("p")) {
            Debug.Log("PauseDown");
            canvas.SetActive(true);
            Time.timeScale = 0;
        }
	}
}
