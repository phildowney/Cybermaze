using UnityEngine;
using System.Collections;

public class StartManager : MonoBehaviour
{

    private GameObject Text;

	// Use this for initialization
	void Start () {

        Text = GameObject.Find("TeleportPrompt");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Text == null) return;

        Text.GetComponent<MeshRenderer>().enabled = true;
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (Text == null) return;

        Text.GetComponent<MeshRenderer>().enabled = false;
    }
}
