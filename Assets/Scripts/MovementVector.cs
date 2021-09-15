using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementVector
{
    public Vector3 pos;
    public Vector3 vel;
    public MovementVector(Vector3 pos, Vector3 vel)
    {
        this.pos = pos;
        this.vel = vel;
    }
}
