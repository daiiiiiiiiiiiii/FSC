using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalFloar : MonoBehaviour,IHitObject
{
    Vector2 _movePos;
    private float _speed = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var move = Mathf.Sin(Time.time) * _speed;
        _movePos = new Vector2(transform.position.x,transform.position.y - move);
        transform.position = _movePos;
    }

    public Vector2 SetPosition(Rigidbody2D rb)
    {
        rb.position = new Vector2(rb.position.x,rb.position.y);
        return rb.position;
    }
}
