using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    float originY;

    public float Distance = 0.5f;
    public float Speed = 2;

    private void Start()
    {
        originY = transform.position.y;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, originY + (Distance * Mathf.Sin(Time.fixedTime * Speed)), transform.position.z);
        transform.Rotate(transform.up, Time.fixedDeltaTime * 45f);
    }
}