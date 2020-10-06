using UnityEngine;

class KeyAction : MonoBehaviour, IEnemy
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
        Debug.DrawRay(_dirPos,_speed[(int)_dir],Color.red);
    }
    public EnemyType GetEnemyType()
    {
        return EnemyType.Key;
    }
}

//public class KeyAction : MonoBehaviour, IEnemy
//{
//    private Vector2[] _speed;
//    private Dir _dir;
//    private Dir _beforedir;
//    private float _time;
//    private float _startTime;
//    private Vector3 _start;
//    private Vector3 _end;
//    RaycastHit2D[] _raycast;
//    int _mask;
//    MoveDir[] _direct;

//    void Start()
//    {
//        _speed = new Vector2[(int)Dir.Max] {
//            new Vector2(1f,0),
//            new Vector2(-1f,0),
//            new Vector2(0,1f),
//            new Vector2(0,-1f),
//            new Vector2(0,0)
//        };
//        _direct = new MoveDir[5];
//        _dir = Dir.Stop;
//        int num = LayerMask.NameToLayer("Default");
//        _mask = 1 << num;
//    }

//    void Update()
//    {
//        if (_time <= 0.0)
//        {
//            _beforedir = _dir;
//            for (int i = 0; i < 5; i++)
//            {
//                // 方向ごとに進んで長いとこに移動
//                SerchDirection(i);
//            }
//            float f = 0;
//            for (int i = 0; i < 4; i++)
//            {
//                if (f < _direct[i]._distance)
//                {
//                    f = _direct[i]._distance;
//                    _dir = (Dir)i;
//                }             
//            }
//            if(_dir == _beforedir)
//            {
//                _dir = Dir.Stop;
//            }
//            _time = 1.0f * f;
//            _startTime = Time.timeSinceLevelLoad;
//            _start = transform.position;
//            var move = _speed[(int)_dir] * f;
//            _end = new Vector2(transform.position.x, transform.position.y) + move;
//        }
//        for (int i = 0; i < 4; i++)
//        {
//            Debug.DrawRay(_direct[i]._posS, _direct[i]._posE - _direct[i]._posS, Color.red);
//        }
//        Move();
//        _time -= Time.deltaTime;
//        Debug.Log(_dir);
//    }

//    void SerchDirection(int dir)
//    {
//        _direct[dir]._posS = (Vector2)transform.position + _speed[dir] * 0.5f;
//        var ray = Physics2D.Raycast(_direct[dir]._posS, _speed[dir], 100f, _mask);
//        _direct[dir]._posE = ray.point;
//        _direct[dir]._distance = ray.distance;
//        Debug.DrawRay(_direct[dir]._posS, _direct[dir]._posE - _direct[dir]._posS, Color.red);
//    }

//    public void Move()
//    {
//        var diff = Time.timeSinceLevelLoad - _startTime;
//        transform.position = Vector3.Lerp(_start, _end, diff / _direct[(int)_dir]._distance);
//    }

//    private void OnCollisionEnter2D(Collision2D col)
//    {
//        if (col.transform.parent.name == "Stage" || col.transform.parent.name == "_blocks")
//        {
//            // 方向の設定
//        }
//    }
//}
