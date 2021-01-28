using UnityEngine;

public class FollowAI : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 3;

    void Start()
    {
    }

    void Update()
    {
        Vector3 displacement = target.transform.position - transform.position;
        if (displacement.magnitude > 1)
        {
            transform.position += displacement * Time.deltaTime * moveSpeed;
        }
    }
}