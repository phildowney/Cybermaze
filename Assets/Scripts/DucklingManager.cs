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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animator duckAnimator = gameObject.GetComponent<Animator>();

        var i = 1;

        foreach (var duckling in ducklings)
        {
            Animator anim = duckling.GetComponent<Animator>();

            Vector3 displacement = gameObject.transform.position - duckling.transform.position;
            Vector3 followOffset = displacement.normalized * minimumDistance * i;
            Vector3 target = displacement - followOffset;

            if (target.magnitude > followOffset.magnitude)
            {
                duckling.transform.position += target * Time.deltaTime * moveSpeed;
            }

            anim.SetFloat("MoveX", duckAnimator.GetFloat("MoveX"));
            anim.SetFloat("MoveY", duckAnimator.GetFloat("MoveY"));

            if (duckAnimator.GetFloat("MoveX") != 0 && duckAnimator.GetFloat("MoveY") != 0) {
                duckling.transform.position += new Vector3(0, 2 * jumpHeight * Mathf.PingPong(jumpSpeed * Time.time, 1) - jumpHeight);
            }

            i += 1;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Duckling"))
        {
            other.enabled = false;
            ducklings.Add(other.gameObject);
        }
    }
}
