using System.Collections;
using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;

public class DucklingManager : MonoBehaviour
{
    private float moveSpeed = 10f;
    private float minimumDistance = 0.3f;
    private float jumpHeight = 0.003f;
    private float jumpSpeed = 3.0f;
    public HashSet<GameObject> ducklings = new HashSet<GameObject>();
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator duckAnimator = gameObject.GetComponent<Animator>();

        var i = 1;

        Vector3 lastDuckPosition = gameObject.transform.position;
        foreach (var duckling in ducklings)
        {
            Animator anim = duckling.GetComponent<Animator>();

            Vector3 displacement = lastDuckPosition - duckling.transform.position;
            Vector3 followOffset = displacement.normalized * minimumDistance;
            Vector3 target = displacement - followOffset;

            if (target.magnitude > followOffset.magnitude)
            {
                duckling.transform.position += target * Time.deltaTime * moveSpeed;
                anim.SetFloat("MoveX", target.x / Mathf.Abs(target.x));
                anim.SetFloat("MoveY", target.y / Mathf.Abs(target.y));
                duckling.transform.position += new Vector3(0, 2 * jumpHeight * Mathf.PingPong(jumpSpeed * (Time.time + i * Time.time / 10), 1) - jumpHeight);
            }
            else
            {
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 0);
            }

            lastDuckPosition = duckling.transform.position;
            i += 1;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Duckling"))
        {
            GlobalData.KeyCount++;
            audio.Play();
            other.enabled = false;
            ducklings.Add(other.gameObject);
        }
    }
}
