﻿using UnityEngine;

class BlackPacAction : MonoBehaviour
{
    MoveType _dir;
    Vector2[] _speed;
    float _time;
    int _mask;
    Vector2 _dirPos;
    RaycastHit2D _ray;
    float _startTime;
    void Start()
    {
        _dir = MoveType.Stop;
        _time = 2f;
        _dirPos = transform.position;
        _speed = new Vector2[(int)MoveType.Max]
        {
            new Vector2(1f,0),
            new Vector2(-1f,0),
            new Vector2(0,1f),
            new Vector2(0,-1f),
            new Vector2(0,0)
        };
        int num = LayerMask.NameToLayer("Default");
        _mask = 1 << num;
    }

    void Update()
    {
        if (_time <= 0)
        {
            // 1秒ごとに方向を決めて障害物がなければ移動
            int num = (int)Random.Range(0, (float)MoveType.Stop);
            if (AbleWalk(num))
            {
                _dir = (MoveType)num;
                _time = 1f;
                Debug.Log(transform.position + ((Vector3)_speed[(int)_dir] * 0.5f) + "Dir" + _dir);
                transform.position = transform.position + (Vector3)_speed[(int)_dir];
            }
            // 障害物があればstopして0.5秒後に方向を再設定
            else
            {
                _dir = MoveType.Stop;
                _time = 0.5f;
            }
            _startTime = Time.timeSinceLevelLoad;
        }
        _time -= Time.deltaTime;
        _dirPos = (Vector2)transform.position + _speed[(int)_dir] * 0.25f;
        if (!_ray.collider)
        {
            DrawRay();
        }
    }

    void Move()
    {
        var diff = Time.timeSinceLevelLoad - _startTime;
        transform.position = Vector3.Lerp(transform.position, (Vector2)transform.position + _speed[(int)_dir], diff);
        Debug.Log(_dir);
    }

    bool AbleWalk(int num)
    {
        var pos = (Vector2)transform.position + (_speed[num] * 0.25f);
        _ray = Physics2D.Raycast(pos, _speed[num], 1f, _mask);
        if (_ray.collider)
        {
            return false;
        }
        return true;
    }

    void DrawRay()
    {
        Debug.DrawRay(_dirPos, _speed[(int)_dir], Color.red);
    }
    public EnemyType GetEnemyType()
    {
        return EnemyType.Pac_B;
    }
}
