using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonFloar : MonoBehaviour,IHitObject
{
    Vector3 _movePos;
    private float _speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _movePos = new Vector2(Mathf.Sin(Time.time) * _speed,0);
        transform.position -= _movePos;
    }

    public Vector2 SetPosition(Rigidbody2D rb)
    {
        rb.position -= new Vector2(_movePos.x,_movePos.y) * 2;
        return rb.position;
    }
}
