using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour,IHitObject
{
    public Vector2 SetPosition(Rigidbody2D rb)
    {
        return rb.position;
    }
}
