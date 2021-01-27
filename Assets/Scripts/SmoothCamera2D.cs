﻿using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour
{
    public float dampTime = 0f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            var playerCamera = GetComponent<Camera>();

            Vector3 point = playerCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
}