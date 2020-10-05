using System;
using UnityEngine;

public class KeyAction : MonoBehaviour, IEnemy
{
    private Vector3[] _speed;
    private Dir _dir;
    private float _time;
    private float _startTime;
    private Vector3 _start;
    private Vector3 _end;
    StageGrider _sg;
    RaycastHit2D _raycast;

    void Start()
    {
        _speed = new Vector3[(int)Dir.Max] {
            new Vector3(1f,0),
            new Vector3(-1f,0),
            new Vector3(0,1f),
            new Vector3(0,-1f),
            new Vector3(0,0)
        };
        _dir = Dir.Right;
        _sg = GameObject.Find("Stage").GetComponent<StageGrider>();
    }

    void Update()
    {
        if (_time <= 0.0)
        {
            _time = 1.0f;
            _startTime = Time.timeSinceLevelLoad;
            _start = transform.position;
            var move = _speed[(int)_dir];
            _end = transform.position + move;
            SetDir();
        }
        _time -= Time.deltaTime;
        Move();
        Debug.Log(_dir);
    }
    public void Move()
    {
        var diff = Time.timeSinceLevelLoad - _startTime;
        transform.position = Vector3.Lerp(_start, _end, diff);
    }
    void SetDir()
    {
        int length = 0;
        int maxNum = 0;
        Dir next = _dir;
        for (int i = 0; i < (int)Dir.Stop; i++)
        {
            if ((int)_dir != i)
            {
                while (IsHitObject(transform.position + _speed[length]))
                {
                    length++;
                }
                if (maxNum < length)
                {
                    maxNum = length;
                    next = (Dir)i;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.parent.name == "Stage" || col.transform.parent.name == "_blocks")
        {
            // 方向の設定
            SetDir();
        }
    }

    public bool IsHitObject(Vector3 pos)
    {
        pos = new Vector3((int)pos.x, (int)pos.y);
        return _sg.GetBlock(pos);
    }
}
