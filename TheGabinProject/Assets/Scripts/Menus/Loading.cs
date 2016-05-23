using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(TimerThingy());
    }

    IEnumerator TimerThingy() {
        yield return new WaitForSeconds(5);
        Application.LoadLevel("World_Test");
    }
}
