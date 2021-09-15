using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    public Vector3 velocity = Vector3.forward;
    public Vector3 toPosition;

    public void AddToPosition()
    {
        toPosition = transform.position + velocity;
    }

    private void Start()
    {
        toPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, toPosition, 0.3f);
    }
}
