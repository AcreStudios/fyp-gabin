using UnityEngine;
using System.Collections;

public class StartPause : MonoBehaviour {

    public Canvas initiatePauseMenu;

	// Use this for initialization
	void Start () {
        initiatePauseMenu = initiatePauseMenu.GetComponent<Canvas>();

        initiatePauseMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	if((Input.GetKeyDown("P")) && (initiatePauseMenu.enabled = false)) {
            initiatePauseMenu.enabled = true;
            Time.timeScale = 0;
        }
	}
}
