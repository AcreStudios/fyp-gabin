using UnityEngine;
using System.Collections;

public class ExplosionEffectScript : MonoBehaviour {

    // Use this for initialization
    Color black = Color.black;
    Renderer current;

    void Start () {
        current = GetComponent<Renderer>();

        current.material.color = Color.black;
        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }
	
	// Update is called once per frame
	void Update () {
        black.a -= 0.01f;
        current.material.color = black;

        if (black.a <= 0){
            Destroy(gameObject);
        }
    }
}
