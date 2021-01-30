using UnityEngine;

public class HuePulse : MonoBehaviour
{
    public float startHue = 0;
    public float endHue = 100;
    public float shiftDuration = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        float hueShift = Mathf.Lerp(startHue, endHue, Mathf.PingPong(Time.time/shiftDuration, 1));
        renderer.material.SetFloat("_HueShift", hueShift);
    }
}

