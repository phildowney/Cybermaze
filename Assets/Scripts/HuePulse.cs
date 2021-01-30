using UnityEngine;

public class HuePulse : MonoBehaviour
{
    public Color colourOne = new Color(200, 100, 100);
    public Color colourTwo = new Color(100, 200, 100);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Color color = Color.Lerp(colourOne, colourTwo, Mathf.PingPong(Time.time/3, 1));
        renderer.material.SetColor("_Color", color);
    }
}
