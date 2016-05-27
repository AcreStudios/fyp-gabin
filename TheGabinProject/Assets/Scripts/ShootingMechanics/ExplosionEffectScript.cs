using UnityEngine;
using System.Collections;

public class ExplosionEffectScript : MonoBehaviour {

    // Use this for initialization
    Color color;
    Renderer current;
    public string bulletState;

    void Start () {
        current = GetComponent<Renderer>();
        switch (bulletState) {
            case "Frozen":
                color = Color.blue;
                break;
            case "Shock":
                color = Color.yellow;
                break;
            case "Fire":
                color = Color.red;
                break;
            case "Normal":
                color = Color.black;
                break;
            default:
                color = Color.grey;
                break;
        }
        
        GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }
	
	// Update is called once per frame
	void Update () {
        color.a -= 0.01f;
        current.material.color = color;

        if (color.a <= 0){
            Destroy(gameObject);
        }
    }
}
